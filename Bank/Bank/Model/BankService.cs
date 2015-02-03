using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.ServiceModel;
using Bank.Interfaces;

namespace Bank.Model
{
    using Interfaces;
    using Org.BouncyCastle.Crypto.Parameters;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class BankService : IBankService, IBankServiceProxyCallback
    {
        #region fields

        private static IBanknoteRepository _repository = new BanknoteRepository();
        private static IBanknoteFactory _factory = new BanknoteFactory(_repository);
        private IBankServiceCallback _callback;
        private BankServiceImpl _service;
        private string sessionId = OperationContext.Current.SessionId;

        #endregion

        public BankService()
        {
            OperationContext.Current.InstanceContext.Closed += Cleanup;
            this._callback = OperationContext.Current.GetCallbackChannel<IBankServiceCallback>();
            this._service = new BankServiceImpl(_factory, _repository, this, this);
        }

        public void doHandshake(int value)
        {
            _service.doHandshake(value);
        }

        public void doInitialize(string[] blindedBanknotes)
        {
            IList<byte[]> _blindedBanknoteList = new List<byte[]>(blindedBanknotes.Length);
            for (int i = 0; i < blindedBanknotes.Length; i++)
                _blindedBanknoteList.Add(blindedBanknotes[i].GetBytes());
            _service.doInitialize(_blindedBanknoteList);
        }

        public void doVerify(string[] banknotes, string[] secrets) { }

        public void doValidate(string banknote, string signature)
        {
            Banknote _banknote;
            _banknote = banknote.FromXml<Banknote>();

            byte[] _signature = signature.GetBytes();
            _service.doValidate(_banknote, _signature);
        }

        public void doFinalize() { }

        private void Cleanup(object sender, EventArgs e)
        {
            OperationContext.Current.InstanceContext.Closed -= Cleanup;
        }

        #region props

        public readonly IBanknoteFactory Factory { get { return _factory; } }

        public readonly IBanknoteRepository Repository { get { return _repository; } }

        public readonly string SessionId { get { return sessionId; } }

        #endregion


        public void onHandshake(Banknote banknote, int banknoteCount, int idPerBanknoteCount, RsaKeyParameters publicKey)
        {
            _callback.onHandshake(banknote.ToXml(), banknoteCount, idPerBanknoteCount, publicKey.AsPublic());
        }

        public void onInitialize(Banknote banknote, int excludeFromAgreement)
        {
            _callback.onInitialize(banknote.ToXml(), excludeFromAgreement);
        }

        public void onVerification(Banknote banknote, byte[] blindSignature)
        {
            _callback.onVerification(banknote.ToXml(), blindSignature.GetString());
        }

        public void onValidate(Banknote banknote, byte[] signature, bool result)
        {
            _callback.onValidate(banknote.ToXml(), signature.GetString(), result);
        }
    }


}
