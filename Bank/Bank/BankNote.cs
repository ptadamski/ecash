using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    [DataContract()]
    public class IdPart
    {
        [DataMember()]
        public string Hash { get; set; }

        [DataMember()]
        public BigInteger RandInt { get; set; }
    }


    [DataContract()]
    public class BankNote
    {
        [DataMember()]
        public int Nominal { get; set; }

        [DataMember()]
        public Guid SerialNumber { get; set; }

        [DataMember()]
        public IdPart Left { get; set; }

        [DataMember()]
        public IdPart Rigth { get; set; }
    }
}
