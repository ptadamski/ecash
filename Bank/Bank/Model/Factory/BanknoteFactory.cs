using Bank.Data;
using Bank.Interface;
using System;

namespace Bank.Model
{
    public class BanknoteFactory : IBanknoteFactory
    {
        private IRepository<Guid, Banknote> _repository;

        public BanknoteFactory(IRepository<Guid, Banknote> aRepository)
        {
            this._repository = aRepository;
        }

        public Banknote Construct(int aInput)
        {
            var serial = Guid.NewGuid();
            while (_repository.Contains(serial))
                serial = Guid.NewGuid();

            var item = new Banknote();
            item.Serial = serial;
            item.Value = aInput;
            return item;
        }

        public void Destruct(Banknote aItem)
        {
            _repository.Remove(aItem.Serial);
        }
    }
}


