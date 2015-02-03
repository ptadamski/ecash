using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model
{
    using Interfaces;

    public class BanknoteRepository : IBanknoteRepository 
    {
        public void Depone(Banknote banknote, UserIdentity user)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Banknote banknote)
        {
            throw new NotImplementedException();
        }
    }

}
