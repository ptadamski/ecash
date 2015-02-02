using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank
{
    [DataContract()]
    public class UserPartId
    {
        [DataMember(IsRequired = true)]
        public Guid RandNum { get; set; }

        [DataMember(IsRequired = true)]
        public string Hash { get; set; }
    }

    [DataContract()]
    public class UserId
    {
        [DataMember(IsRequired = true)]
        public UserPartId Left { get; set; }

        [DataMember(IsRequired = true)]
        public UserPartId Right { get; set; }
    }

    [DataContract()]
    public class BankNote
    {
        [DataMember(IsRequired = true)]
        public int Value { get; set; }

        [DataMember(IsRequired = true)]
        public Guid Serial { get; set; }

        [DataMember(IsRequired = false)]
        public UserId[] UserId { get; set; }
    }

    [DataContract()]
    public class SecretUserPartId
    {
        [DataMember(IsRequired = true)]
        public Guid IdPart { get; set; }  
 
        [DataMember(IsRequired = true)]
        public Guid Rand1 { get; set; }

        [DataMember(IsRequired = true)]
        public Guid Rand2 { get; set; }

        [DataMember(IsRequired = true)]
        public string Hash { get; set; }

    }

    [DataContract()]
    public class SecretBankNote
    {
        [DataMember(IsRequired = true)]
        public BankNote Banknote { get; set;}

        [DataMember(IsRequired = true)]
        public string blindFactor { get; set; }

        [DataMember(IsRequired = true)]
        public SecretUserPartId[] UserId { get; set; }
    }

}
