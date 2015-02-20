using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Alicja
{
    public class BankServiceCallback : DuplexClientBase<BankService.IBankServiceCallback>, BankService.IBankServiceCallback 
    {

        public void onInit(BankService.Banknote aBanknote, int aBanknoteCount, string aPublicKey)
        {
            throw new NotImplementedException();
        }

        public void onCreateAgreement(int aIndex)
        {
            throw new NotImplementedException();
        }

        public void onVerifyAgreement(Common.PublicSecret aBanknote, string aSignature, bool aAgreed)
        {
            throw new NotImplementedException();
        }

        public void doUncoverSecret(Common.PublicSecret aSecret)
        {
            throw new NotImplementedException();
        }

        public void onVerifySecret(Common.PublicSecret aSecret, bool aAgreed)
        {
            throw new NotImplementedException();
        }
    }

}
