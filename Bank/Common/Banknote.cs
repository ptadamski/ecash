using Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common
{
    public class Identity
    {
        [DataMember()]
        public PublicSecret[] PartialId { get; set; }
    }

    public class Banknote : IEqualityComparer<Banknote>
    {
        [DataMember()]
        public Guid Serial { get; set; }

        [DataMember()]
        public int Value { get; set; }

        [DataMember()]
        public Identity[] UserId { get; set; }

        public bool Equals(Banknote x, Banknote y)
        {
            return x.Serial.Equals(y.Serial);
        }

        public int GetHashCode(Banknote obj)
        {
            return obj.Serial.GetHashCode();
        }
    }
}