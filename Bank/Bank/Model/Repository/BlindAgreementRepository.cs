using Bank.Data;
using Bank.Interface;
using System;
using System.Collections.Generic;

namespace Bank.Model
{

    public class BlindAgreementRepository : IRepository<Guid, BlindAgreement>
    {
        private static Dictionary<Guid, BlindAgreement> _agreements = new Dictionary<Guid, BlindAgreement>();

        public bool Add(Guid aKey, BlindAgreement aItem)
        {
            if (_agreements.ContainsKey(aKey))
                return false;
            _agreements[aKey] = aItem;
            return true;
        }

        public bool Remove(Guid aKey)
        {
            if (!_agreements.ContainsKey(aKey))
                return false;
            _agreements.Remove(aKey);
            return true;
        }

        public bool Contains(Guid aKey)
        {
            return _agreements.ContainsKey(aKey);
        }

        public bool Get(Guid aKey, out BlindAgreement aValue)
        {
            return _agreements.TryGetValue(aKey, out aValue);
        }

    }
}