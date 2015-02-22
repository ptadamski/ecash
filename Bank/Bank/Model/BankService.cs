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
        private static BanknoteRepository _baknoteRepository = new BanknoteRepository();

        private IBank _bank;
        private IBankServiceCallback _callback;

        public BankService()
        {
            this._callback = OperationContext.Current.GetCallbackChannel<IBankServiceCallback>();
            this._bank = new Bank(_baknoteRepository, this, _callback);
        }

        public void doInit(Banknote aBanknote, bool aUnderCreation)
        {
            _bank.doInit(aBanknote, aUnderCreation);
        }

        public void doCreateAgreement(string[] aBlindMessages)
        {
            IList<byte[]> blindedMessages = new List<byte[]>(aBlindMessages.Length);
            foreach (var item in aBlindMessages)
                blindedMessages.Add(item.GetBytes());
            _bank.doCreateAgreement(blindedMessages);
        }

        public void doVerifyAgreement(PublicSecret[] aSecrets, string[] aBlindingFactors)
        {
            if (aSecrets.Length != aBlindingFactors.Length)
                return;
            BigInteger[] factors = new BigInteger[aBlindingFactors.Length];
            for (int i = 0; i < aSecrets.Length; i++)
                factors[i] = new BigInteger(aBlindingFactors[i], 16);
            _bank.doVerifyAgreement(aSecrets, factors);
        }

        public void doDepone(Secret aBanknote, string aSignature, int[] aIdIndexList, PrivateSecret[] aPartialIdList)
        {
            _bank.doDepone(aBanknote, aSignature.GetBytes(), aIdIndexList, aPartialIdList);
        }
    }
}
