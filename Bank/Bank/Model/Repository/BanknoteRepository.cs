using Bank.Data;
using Bank.Interface;
using System;
using System.Collections.Generic;

namespace Bank.Model
{
    public class BanknoteRepository : IRepository<Guid, Banknote>
    {
        private static Dictionary<Guid, Banknote> _banknotes = new Dictionary<Guid, Banknote>();

        public bool Add(Guid aKey, Banknote aItem)
        {
            if (_banknotes.ContainsKey(aKey))
                return false;
            _banknotes[aKey] = aItem;
            return true;
        }

        public bool Remove(Guid aKey)
        {
            if (!_banknotes.ContainsKey(aKey))
                return false;
            _banknotes.Remove(aKey);
            return true;
        }

        public bool Contains(Guid aKey)
        {
            return _banknotes.ContainsKey(aKey);
        }

        public bool Get(Guid aKey, out Banknote aValue)
        {
            return _banknotes.TryGetValue(aKey, out aValue);
        }
    }
}
