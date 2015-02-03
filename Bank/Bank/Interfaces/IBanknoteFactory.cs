using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Interfaces
{
    public interface IBanknoteFactory
    {
        Banknote CreateBanknote(string agreementId);
        void MakeAgreement(string agreementId, IList<byte[]> blindedBanknotes, int excludeFromAgreement);
        Banknote GetAgreementContext(string agreementId);
        void VerifyAgreement(Banknote[] banknotes, IList<UserIdentity[]> userIdentitySequences);
        void DiscardAgreement(string agreementId);
        void AproveAgreement(string agreementId);
    }
}
