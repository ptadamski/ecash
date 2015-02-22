using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Bank.Model
{
    public class BanknoteRepository
    {
        public class About
        {
            public Guid PartialId { get; set; }
            public int Count { get; set; }
        }

        public const int MaxSubIdentityCount = 2;
        public const int MaxIdentityCount = 100;

        Dictionary<Banknote, About[]> storage = new Dictionary<Banknote, About[]>();

        private bool IsAvalible(Banknote aBanknote)
        {
            About[] item;
            return !storage.TryGetValue(aBanknote, out item);
        }

        public Banknote Construct()
        {
            var item = new Banknote();
            item.Serial = Guid.NewGuid();

            while (!IsAvalible(item))
                item.Serial = Guid.NewGuid();

            item.UserId = new Identity[MaxIdentityCount];

            return item;
        }

        public void Add(Banknote aBanknote)
        {
            if (IsAvalible(aBanknote))
                storage.Add(aBanknote, new About[MaxSubIdentityCount]);
        }

        public bool Update(Banknote aBanknote, int aIndex, Guid aPartialId)
        {
            About[] items;
            if (storage.TryGetValue(aBanknote, out items))
            {
                var item = items[aIndex] ?? new About() { Count = 0 , PartialId = aPartialId};
                if (item.PartialId.Equals(aPartialId))
                {
                    item.Count++;  
                    items[aIndex] = item; 
                    return true;
                }
            }
            return false;
        }

        public bool Verify(Guid aSender, Banknote aBanknote, out Guid aSuspect) 
        {             
            About[] items;
            aSuspect = new Guid();
            if (storage.TryGetValue(aBanknote, out items))
            {
                int k = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null) 
                    {                                            
                        aSuspect = items[i].PartialId.Xor(aSuspect);
                        k++;
                    }
                    if (items[i].Count>1)
                    {
                        aSuspect = aSender;
                        return false;
                    }
                }
                return k != items.Length;
            }
            return false;
        }

    }
}
