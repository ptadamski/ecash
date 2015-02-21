using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using Common;

namespace Bank.Data
{

    public class BanknoteAgreement
    {
        IList<byte[]> _content = new List<byte[]>();
        IDictionary<PublicSecret, PrivateSecret> _repository;

        public BanknoteAgreement(IDictionary<PublicSecret, PrivateSecret> aSecretsRepository)
        {
            this._repository = aSecretsRepository;
        }

        public void Init(IList<byte[]> aBlindedContent) 
        {
            this._status = AgreementStatus.Blinded;
            this._content = aBlindedContent;
        }

        public enum AgreementStatus { Empty, Blinded, Good, Bad, Signed }

        private AgreementStatus _status;

        public AgreementStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        //czy przyslane wczesniej wiadomosci pasuja pod wskazany hasz                                               
        public void Verify(PublicSecret[] aSecrets, BigInteger[] aFactorList, RsaKeyParameters aPublicKey)
        {
            if (_status != AgreementStatus.Blinded)
                return;

            _status = AgreementStatus.Bad;

            //(0) testy ilosci informacji
            if (aSecrets.Length != aFactorList.Length)
                return;
            if (aSecrets.Length != _content.Count)
                return;


            RsaBlindingEngine eng = new RsaBlindingEngine();

            //(1) czy aContent[i] po zaslepieniu przy uzyciu aFactorList[i] bedzie tym co zostalo zapisane w _repository[i].Content
            for (int i = 0, len = _content.Count; i < len; ++i)
            {
                if (_forSignature == i)
                    continue;

                RsaBlindingParameters param = new RsaBlindingParameters(aPublicKey, aFactorList[i]);
                eng.Init(true, param);

                var digest = aSecrets[i].hash.GetBytes();
                var data = eng.ProcessBlock(digest, 0, digest.Length);

                if (!data.IsEqual(_content[i]))
                    return;
            }


            //(2) czego skrotem jest _repository[i].Content - sprawdzic w repozytorium (z owczesniej otrzymanych informacji od Alice)
            //problemy z asynchronicznoscia etc... dlugoby myslec, porzucic
            //czyli poki co zostawic w spokoju


            //trzeba oznaczyc sekrety jako : Accepted, Denied, Unknown
            /*foreach (var publicSecret in aSecrets)
            {
                PrivateSecret privateSecret;
                _repository.TryGetValue(publicSecret, out privateSecret);
                if (privateSecret == null)
                    return;
                try
                {             
                    Banknote banknote = privateSecret.data.FromXml<Banknote>();
                    
                }
                catch (Exception)
                {
                }
            }               */

            _status = AgreementStatus.Good;


        }
              
        //podpisanie hasza
        public byte[] Sign(RsaKeyParameters aPrivateKey)
        {
            if (_status == AgreementStatus.Good) 
            {             
                RsaEngine eng = new RsaEngine();
                eng.Init(true, aPrivateKey);
                return eng.ProcessBlock(_content[_forSignature], 0, _content[_forSignature].Length);
            }
            return null;
        }

        private int _forSignature;

        public int ForSignature
        {
            get { return _forSignature; }
            set { _forSignature = value; }
        }

        public void PickupForSignature(int aIndex) 
        {
            this._forSignature = aIndex;
        }
    }
}
