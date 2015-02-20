using Common;
using System;
using System.Runtime.Serialization;

namespace Bank.Data
{
    [DataContract()]
    public class Identity
    {
        [DataMember()]
        public PublicSecret[] LeftId { get; set; }

        [DataMember()]
        public PublicSecret[] RightId { get; set; }
    }

    [DataContract()]
    public class Banknote
    {
        [DataMember()]
        public Guid Serial { get; set; }

        [DataMember()]
        public int Value { get; set; }

        [DataMember()]
        public Identity Id { get; set; }
    }
}