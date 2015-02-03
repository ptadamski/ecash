using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Interfaces
{
    public class Banknote
    {
        public int value;
        public Guid serial;
    }

    public class UserIdentity
    {
        public string hash;
        public Guid random;
    }

    public class UserSecretIdentity
    {
        public string hash;
        public Guid random;

    }


}
