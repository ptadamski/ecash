using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using Common;

namespace Bank.Data
{                               

    public class BlindAgreement
    {
        public enum AgreementStatus { None, Blinded, Valid, Failed }

        private Guid _id;
        private IList<byte[]> _content;
        private AgreementStatus _status;
        private int _ignore;

        public BlindAgreement()
        {
            this._id = Guid.NewGuid();
            this._status = AgreementStatus.None;
            this._content = null;
            this._ignore = 0;
        }

        public BlindAgreement(Guid aId, IList<byte[]> aBlindedContent, int aIgnored)
        {
            Init(aId, aBlindedContent, aIgnored);
        }

        public void Init(Guid aId, IList<byte[]> aBlindedContent, int aIgnored)
        {
            this._id = aId;
            this._content = aBlindedContent;
            this._status = AgreementStatus.Blinded;
            this._ignore = aIgnored;
        }

        public bool Verify(IList<byte[]> aContent, IList<BigInteger> aFactorList, RsaKeyParameters aPublicKey)
        //aContent - kolekcja skrotow banknotow
        //aFactorList - kolekcja parametrow zaslepiajacych wiadomosc
        //aPublicKey - klucz publiczny banku
        {
            if (aContent.Count != aFactorList.Count)
            {
                _status = AgreementStatus.Failed;
                return false;
            }

            RsaBlindingEngine eng = new RsaBlindingEngine();

            for (int i = 0, len = aContent.Count; i < len; ++i)
            {
                if (_ignore == i)
                    continue;

                RsaBlindingParameters param = new RsaBlindingParameters(aPublicKey, aFactorList[i]);
                eng.Init(true, param);
                var data = eng.ProcessBlock(aContent[i], 0, aContent[i].Length);
                if (!data.IsEqual(_content[i]))
                {
                    _status = AgreementStatus.Failed;
                    return false;
                }
            }

            _content = aContent;
            _status = AgreementStatus.Valid;
            return true;
        }

        public byte[] Sign(RsaKeyParameters aPrivateKey)
        {
            if (_status == AgreementStatus.Valid)
            {
                RsaEngine eng = new RsaEngine();
                eng.Init(true, aPrivateKey);
                return eng.ProcessBlock(_content[_ignore], 0, _content[_ignore].Length);
            }
            return null;
        }

        public Guid Id { get { return _id; } }
        public IList<byte[]> Content { get { return _content; } }
        public AgreementStatus Status { get { return _status; } }
        public int Ignore { get { return _ignore; } set { _ignore = value; } }

    }
}
