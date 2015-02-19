using System;
namespace Bank.Interface
{
    public interface IRepository<Key, Value>
    {
        bool Add(Key aKey, Value aItem);
        bool Remove(Key aKey);
        bool Contains(Key aKey);
        bool Get(Key aKey, out Value aValue);
    }
}


