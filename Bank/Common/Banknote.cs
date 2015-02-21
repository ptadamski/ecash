using Common;
using System;
using System.Runtime.Serialization;

namespace Common
{
    public class Identity
    {
        [DataMember()]
        public PublicSecret LeftId { get; set; }

        [DataMember()]
        public PublicSecret RightId { get; set; }
    }

    public class Banknote
    {
        [DataMember()]
        public Guid Serial { get; set; }

        [DataMember()]
        public int Value { get; set; }

        [DataMember()]
        public Identity[] UserId { get; set; }
    }
}