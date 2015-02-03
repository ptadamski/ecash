using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Interfaces
{
    public interface IBanknoteRepository
    {
        void Depone(Banknote banknote, UserIdentity user);
        bool Exists(Banknote banknote);
    }
}
