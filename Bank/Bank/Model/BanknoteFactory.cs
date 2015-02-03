using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model
{
    using Interfaces;

    public class BanknoteFactory : IBanknoteFactory
    {                  
        private IBanknoteRepository _repository;
        private Dictionary<string, Agreement> _agreements;

        public BanknoteFactory(IBanknoteRepository repository)
        {
            this._repository = repository;
        }

        private class Agreement
        {
            public Banknote banknote;
            public IList<byte[]> blindedMessages;
            public int without;
        }

        public Banknote CreateBanknote(string agreementId)
        {
            Banknote banknote = new Banknote();
            banknote.serial = Guid.NewGuid();
            while (_repository.Exists(banknote))
                banknote.serial = Guid.NewGuid();
            var agreement = _agreements[agreementId] = new Agreement();
            agreement.banknote = banknote;
            return banknote;
        }

        public void MakeAgreement(string agreementId, IList<byte[]> blindedBanknotes, int excludeFromAgreement)
        {
            var agreement = _agreements[agreementId];
            agreement.blindedMessages = blindedBanknotes;
            agreement.without = excludeFromAgreement;
        }

        public Banknote GetAgreementContext(string agreementId) 
        {
            Agreement agreement = null;
            if (_agreements.TryGetValue(agreementId, out agreement))
                return agreement.banknote;
            return null;
        }

        public void VerifyAgreement(string agreementId, Banknote[] banknotes, IList<UserIdentity[]> userIdentitySequences)
        {
            throw new NotImplementedException();
        }

        public void DiscardAgreement(string agreementId)
        {
            throw new NotImplementedException();
        }

        public void AproveAgreement(string agreementId)
        {
            throw new NotImplementedException();
        }
    }
}
