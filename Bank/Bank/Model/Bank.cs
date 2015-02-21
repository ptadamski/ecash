using Bank.Data;
using Bank.Interface;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using Common;
using Bank.Model;
using Org.BouncyCastle.Crypto.Digests;

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
        private Dictionary<Guid, Banknote> _banknotes;
        private Dictionary<Guid, BanknoteAgreement> _agreements = new Dictionary<Guid, BanknoteAgreement>();
        private Dictionary<PublicSecret, PrivateSecret> _secrets = new Dictionary<PublicSecret, PrivateSecret>();
         
        //factory                  
        //private IBanknoteFactory _banknoteFactory;  
        //private IBlindAgreementFactory _agreementFactory;
        //callback                                         
        private IBankService _service;                     
        private IBankServiceCallback _callback;

        public Bank(Dictionary<Guid, Banknote> aBanknoteRepository, IBankService aService, IBankServiceCallback aCallback)
        {
            this._banknotes = aBanknoteRepository;
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
                                                            
        public void doUncoverSecret(PublicSecret aSecret)
        {
            _callback.doUncoverSecret(aSecret);
        }

        public void onVerifyAgreement(PublicSecret aBanknote, byte[] aBlindSignature, bool aAgreed)
        {
            _callback.onVerifyAgreement(aBanknote, aBlindSignature.GetString(), aAgreed);
        }

        public void onVerifySecret(PublicSecret aSecret, bool aAgreed)
        {
            _callback.onVerifySecret(aSecret, aAgreed);
        }

        #endregion
                                
        #region IBank

        public void doInit(Banknote aBanknote)
        {
            var serial = Guid.NewGuid();
            while (_banknotes.ContainsKey(serial))
                serial = Guid.NewGuid();


            _banknote = new Banknote();
            _banknote.Serial = serial;
            _banknote.Value = aBanknote.Value;
                                                   
            _banknotes.Add(_banknote.Serial, _banknote);
                                                         
            Console.WriteLine(_banknote.Serial);
            Console.WriteLine(_banknote.Value);

            Console.WriteLine("try callback");
            onInit(_banknote, BanknoteCount, _pub); //callback    
            Console.WriteLine("end callback");
        }

        public void doCreateAgreement(IList<byte[]> aBlindMessages)
        {
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
                          
        public void doCreateSecret(PublicSecret aSecret)
        {
            _secrets.Add(aSecret, null); 
        }

        public void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate)
        {              
            //albo przyjmuje aPrivate jako dane, albo pozostawia null w repo
            //PrivateSecret item = null;
            //_secrets.TryGetValue(aPublic, out item);

            //Sha256Digest digester = new Sha256Digest();
            //var digestedBytes = new byte[digester.GetDigestSize()];

            //var d = item.data.GetBytes();
            //var r1 = aPublic.random1.ToByteArray();
            //var r2 = item.random2.ToByteArray(); 

            //digester.BlockUpdate(d, 0, d.Length);
            //digester.BlockUpdate(r1, 0, r1.Length);
            //digester.BlockUpdate(r2, 0, r2.Length);
            //digester.DoFinal(digestedBytes, 0);
            //return digestedBytes.IsEqual(item.Public.hash.GetBytes());
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
    }        


}
