using Bank.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Org.BouncyCastle.Math;
using Bank.Data;
using System.ServiceModel;

namespace Bank.Model
{
    //proxy
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class BankService : IBankService //per session 
    {
        private static IRepository<Guid, Banknote> _baknoteRepository = new BanknoteRepository();
        private static IBanknoteFactory _banknoteFactory = new BanknoteFactory(_baknoteRepository);

        private IBank _bank;
        private IBankServiceCallback _callback;

        public BankService()
        {
            this._callback = OperationContext.Current.GetCallbackChannel<IBankServiceCallback>();
            this._bank = new Bank(_banknoteFactory, _baknoteRepository, this, _callback);
        }

        public void doInit(Banknote aBanknote)
        {
            _bank.doInit(aBanknote);
        }

        public void doCreateAgreement(string[] aBlindMessages)
        {
            IList<byte[]> blindedMessages = new List<byte[]>(aBlindMessages.Length);
            foreach (var item in aBlindMessages)
                blindedMessages.Add(item.GetBytes());
            _bank.doCreateAgreement(blindedMessages);
        }

        public void doCreateSecret(PublicSecret aSecret)
        {
            _bank.doCreateSecret(aSecret);
        }

        public void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate)
        {
            _bank.doVerifySecret(aPublic, aPrivate);
        }

        public void doVerifyAgreement(PublicSecret[] aSecrets, string[] aBlindingFactors)
        {
            if (aSecrets.Length != aBlindingFactors.Length)
                return;
            BigInteger[] factors = new BigInteger[aBlindingFactors.Length];
            for (int i = 0; i < aSecrets.Length; i++)
                factors[i] = new BigInteger(aBlindingFactors[i], 10);
            _bank.doVerifyAgreement(aSecrets, factors);
        }

        public void doFinalize()
        {
            _bank.doFinalize();
        }

    }
}
