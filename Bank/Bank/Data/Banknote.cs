using Common;
using System;
using System.Runtime.Serialization;

namespace Bank.Data
{          
    [DataContract()]
    public class Banknote
    {
        [DataMember()]
        public Guid Serial { get; set; }

        [DataMember()]
        public int Value { get; set; }

        [DataMember()]
        public PublicSecret LeftId { get; set; }

        [DataMember()]
        public PublicSecret RightId { get; set; }
    }
}