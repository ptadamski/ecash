using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{

    [DataContract(IsReference=true)]
    public class IdSeq
    {
        [DataMember(IsRequired = true)]
        public Guid RandNum { get; set; }

        [DataMember(IsRequired = true)]
        public string Hash { get; set; }
    }

    [DataContract()]
    public class BankNote
    {
        [DataMember(IsRequired=true)]
        public int Value { get; set; }

        [DataMember(IsRequired = true)]
        public Guid Serial { get; set; }

        [DataMember(IsRequired = false)]
        public IdSeq UserIdentity { get; set; }
    }
}
