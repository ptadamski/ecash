using Bank.Data;
using Bank.Interface;
using System;
using System.Collections.Generic;

namespace Bank.Model
{
    public class BlindAgreementFactory : IBlindAgreementFactory
    {
        private IRepository<Guid, BlindAgreement> _repository;

        public BlindAgreementFactory(IRepository<Guid, BlindAgreement> aRepository)
        {
            this._repository = aRepository;
        }

        public BlindAgreement Construct(Banknote aBanknote, IList<byte[]> aBlindedContent, int aIgnored)
        {
            BlindAgreement item = new BlindAgreement(aBanknote.Serial, aBlindedContent, aIgnored);
            return item;
        }

        public void Destruct(Guid aKey)
        {
            _repository.Remove(aKey);
        }
    }
}
