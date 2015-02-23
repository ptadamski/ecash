using Bank.Data;
using Bank.Interface;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using Common;
using Bank.Model;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;

namespace Bank.Model
{

    public class Bank : IBank, IBankCallback
    {
        private static int BanknoteCount = 100;
        private static Random _rand = new Random();
        private static RsaKeyParameters _pub;
        private static RsaKeyParameters _prv;
        //data
        private Banknote _banknote;
        private BanknoteAgreement _agreement;
        //repository          
        private BanknoteRepository _repository;
        private Dictionary<Guid, BanknoteAgreement> _agreements = new Dictionary<Guid, BanknoteAgreement>();
        private Dictionary<PublicSecret, PrivateSecret> _secrets = new Dictionary<PublicSecret, PrivateSecret>();

        //factory                  
        //private IBanknoteFactory _banknoteFactory;  
        //private IBlindAgreementFactory _agreementFactory;
        //callback                                         
        private IBankService _service;
        private IBankServiceCallback _callback;

        public Bank(BanknoteRepository aBanknoteRepository, IBankService aService, IBankServiceCallback aCallback)
        {
            this._repository = aBanknoteRepository;
            this._service = aService;
            this._callback = aCallback;
        }

        #region IBankCallback

        public void onInit(Banknote aBanknote, int aBanknoteCount, RsaKeyParameters aPublicKey)
        {
            _callback.onInit(aBanknote, aBanknoteCount, aPublicKey.AsPublic());
        }

        public void onCreateAgreement(int aIndex)
        {
            _callback.onCreateAgreement(aIndex);
        }

        public void onVerifyAgreement(PublicSecret aBanknote, byte[] aBlindSignature, bool aAgreed)
        {
            _callback.onVerifyAgreement(aBanknote, aBlindSignature.GetString(), aAgreed);
        }

        #endregion

        #region IBank

        private bool _underCreation;

        public void doInit(Banknote aBanknote, bool aUnderCreation)
        {
            _underCreation = aUnderCreation;
            _banknote = aBanknote;

            if (aUnderCreation)
            {
                _banknote = _repository.Construct();
                _banknote.Value = aBanknote.Value;
                _repository.Add(_banknote);
                onInit(_banknote, BanknoteCount, _pub); //callback   
            }

            Console.WriteLine("Rozpoczeto negocjacje banknotu o \n numerze seryjnym:{0} \n i wartosci:{1}", _banknote.Serial, _banknote.Value);
        }

        public void doCreateAgreement(IList<byte[]> aBlindMessages)
        {
            if (!_underCreation)
                return;

            if (aBlindMessages.Count != BanknoteCount)
                return;

            _agreement = new BanknoteAgreement(_secrets);
            _agreement.Init(aBlindMessages);
            _agreements.Add(_banknote.Serial, _agreement);
            _agreement.PickupForSignature(_rand.Next(0, BanknoteCount));
            onCreateAgreement(_agreement.ForSignature); //callback
        }

        public void doVerifyAgreement(PublicSecret[] aSecrets, BigInteger[] aBlindingFactors)
        {
            if (!_underCreation)
                return;

            //(0) czy istnieje porozumienie w sprawie emisji banknotu
            if (_agreement == null)
                return;

            //(1) czy wiadomosc pokrywa sie z zaslepiona wiadomoscia  
            _agreement.Verify(aSecrets, aBlindingFactors, _pub);

            //(2) czego skrotem jest wyslana wiadomosc 
            //pominiete

            var signature = _agreement.Sign(_prv);

            onVerifyAgreement(aSecrets[_agreement.ForSignature], signature, signature != null);
            Console.WriteLine();
            Console.WriteLine("Podpisano banknot nastepujaca sygnatura: {0}", signature.GetString());
        }

        #endregion

        public static RsaKeyParameters PublicKey
        {
            get { return _pub; }
            set { _pub = value; }
        }

        public static RsaKeyParameters PrivateKey
        {
            get { return _prv; }
            set { _prv = value; }
        }

        public void doDepone(Secret aBanknote, byte[] aSignature, int[] aIdIndexList, PrivateSecret[] aPartialIdList)
        {
            //(0)
            Banknote banknote = null;
            try
            {
                banknote = aBanknote.Private.data.FromXml<Banknote>();
            }
            catch (Exception)
            {
                Console.WriteLine("pusty banknot?");
            }

            if (banknote == null)
                return;

            if (aIdIndexList.Length != aPartialIdList.Length)
                return;

            if (banknote.UserId.Length != aIdIndexList.Length)
                return;

            var digester = new Sha256Digest();
            var eng = new RsaEngine();

            //(1) czy H(d,r,l) = aBanknote.Public.hash; 
            var banknoteSecret = new Secret(aBanknote.Private.data.GetBytes(),
                aBanknote.Public.random1, aBanknote.Private.random2, digester);

            if (!banknoteSecret.Public.hash.Equals(aBanknote.Public.hash))
                return;

            //(2) czy SIG(h) =  aSignature
            var h = aBanknote.Public.hash.GetBytes();
            eng.Init(true, _prv);
            var s = eng.ProcessBlock(h, 0, h.Length);

            if (!s.IsEqual(aSignature))
                return;

            //(3) falszerstwo?
            //czy dostarczone guidy pochodza z banknotu?
            for (int i = 0; i < aPartialIdList.Length; i++)
            {
                var userIdSecret = new Secret(aPartialIdList[i].data.GetBytes(),
                    banknote.UserId[i].PartialId[aIdIndexList[i]].random1, aPartialIdList[i].random2, digester);
                if (!userIdSecret.Public.hash.Equals(banknote.UserId[i].PartialId[aIdIndexList[i]].hash))
                    return;
            }

            Guid client = Guid.NewGuid();
            Guid suspect;
            for (int i = 0; i < aPartialIdList.Length; i++)
            {
                var partialId = new Guid(aPartialIdList[i].data.GetBytes());
                _repository.Update(banknote, aIdIndexList[i], partialId);
                if (!_repository.Verify(client, banknote, out suspect))
                {
                    if (suspect.Equals(new Guid()))
                    {
                        Console.WriteLine("Ciekawe...");
                    }
                    else if (suspect.Equals(client))
                    {
                        Console.WriteLine("Oszukujesz...");
                    }
                    else
                    {
                        Console.WriteLine("Oszukano cie...");
                    }
                    return;
                }

                Console.WriteLine("Banknote zdeponowano \n numer seryjny:{0} \n o wartosci:{1}", banknote.Serial, banknote.Value);
            }
        }


    }
}
