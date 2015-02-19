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
        private RsaKeyParameters _pub; 
        private RsaKeyParameters _prv;
        //data
        private Banknote _banknote;    
        private BlindAgreement _agreement;   
        //repository                               
        private IRepository<Guid, Banknote> _banknotes;
        private IRepository<Guid, BlindAgreement> _agreements;
        private Dictionary<PublicSecret, PrivateSecret> _secrets = new Dictionary<PublicSecret, PrivateSecret>();
         
        //factory                  
        private IBanknoteFactory _banknoteFactory;  
        private IBlindAgreementFactory _agreementFactory;
        //callback                                         
        private IBankService _service;                     
        private IBankServiceCallback _callback;       

        public Bank(IBanknoteFactory aBanknoteFactory, IRepository<Guid, Banknote> aBanknoteRepository, 
            IBankService aService, IBankServiceCallback aCallback)
        {
            this._banknotes = aBanknoteRepository;
            this._service = aService;
            this._banknoteFactory = aBanknoteFactory;
            this._callback = aCallback;
            this._agreements = new BlindAgreementRepository();
            this._agreementFactory = new BlindAgreementFactory(_agreements);
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

        public void onVerifyAgreement(PublicSecret aBanknote, byte[] aSignature, bool aAgreed)
        {
            _callback.onVerifyAgreement(aBanknote, aSignature.GetString(), aAgreed);
        }

        public void onVerifySecret(PublicSecret aSecret, bool aAgreed)
        {
            _callback.onVerifySecret(aSecret, aAgreed);
        }

        #endregion
                                
        #region IBank

        public void doInit(Banknote aBanknote)
        {
            _banknote = _banknoteFactory.Construct(aBanknote.Value);
            _banknotes.Add(_banknote.Serial, _banknote);
            onInit(_banknote, BanknoteCount, _pub);
        }

        public void doCreateSecret(PublicSecret aSecret)
        {
            _secrets.Add(aSecret, null);
            onCreateSecret(count);
        }

        public void doCreateAgreement(IList<byte[]> aBlindMessages)
        {
            if (aBlindMessages.Count != BanknoteCount)
                return;

            foreach (var msg in aBlindMessages)
                if (msg == null || msg.Length == 0)
                    return;

            _agreement = _agreementFactory.Construct(_banknote, aBlindMessages, _rand.Next);
            onCreateAgreement(_agreement.Ignore);
        }

        public void doVerifyAgreement(PublicSecret[] aSecrets, BigInteger[] aBlindingFactors)
        {
            if (_agreement == null)
                return;
            aSecrets[0].hash

            //weryfikacja poprawnosci haszy
            if (_agreement.Verify(aSecrets[0].hash, aBlindingFactors, _pub))
            {
                var signature = _agreement.Sign(_prv);
                onVerify(_banknote, signature);
            }
            
            //ustalanie poprawnosci danych
            //powinien wyslac wczesniej hasze
        }

        public void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate)
        {
            onVerifySecret(IsValidSecret(aSecretId, aRandom2, aData));
        }

        public bool IsValidSecret(PublicSecret aPublic, PrivateSecret aPrivate)
        {
            Secret item = _secrets[aSecretId];
            Sha256Digest digester = new Sha256Digest();
            var digestedBytes = new byte[digester.GetDigestSize()];
            digester.BlockUpdate(item.Private.data, 0, item.Private.data.Length);
            digester.BlockUpdate(item.Public.random1, 0, item.Public.random1.Length);
            digester.BlockUpdate(item.Private.random2, 0, item.Private.random2.Length);
            digester.DoFinal(digestedBytes, 0);
            return digestedBytes.IsEqual(item.Public.hash);
        }


        public void doFinalize()
        {
            throw new NotImplementedException();
        }

        #endregion

        public RsaKeyParameters PublicKey
        {
            get { return _pub; }
            set { _pub = value; }
        }

        public RsaKeyParameters PrivateKey
        {
            get { return _prv; }
            set { _prv = value; }
        }
    }        


}
