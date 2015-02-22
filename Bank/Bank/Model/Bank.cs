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
            
           // Console.WriteLine(_banknote.ToXml());
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
            Banknote banknote;
            try
            {
                banknote = aBanknote.Private.data.FromXml<Banknote>();
            }
            catch(Exception)
            {
                banknote = new Banknote();
            }

            if (aIdIndexList.Length != aPartialIdList.Length)
                return;

            if (banknote.UserId.Length != aIdIndexList.Length)
                return;

            var digester = new Sha256Digest();
            var eng = new RsaEngine();

            //(1)czy na pewno hasz jest haszem tego banknotu?
            var banknoteSecret = new Secret(aBanknote.Private.data.GetBytes(),
                aBanknote.Public.random1, aBanknote.Private.random2, digester);

            if (!banknoteSecret.Public.hash.Equals(aBanknote.Public.hash))
                return;

            //(2) czy hasz banknotu ma podpis
            var h = aBanknote.Public.hash.GetBytes();
            eng.Init(true, _prv);
            var s = eng.ProcessBlock(h, 0, h.Length);

            if (!s.IsEqual(aSignature))
                return;

            //(3) falszerstwo?


        }
    }        


}
