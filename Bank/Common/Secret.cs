using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract()]
    public class PublicSecret : IEqualityComparer<PublicSecret>
    {
        public PublicSecret(string hash, Guid random1)
        {
            this.hash = hash;
            this.random1 = random1;
        }

        public PublicSecret()
        {

        }

        [DataMember()]
        public string hash;

        [DataMember()]
        public Guid random1;

        public bool Equals(PublicSecret x, PublicSecret y)
        {
            return x.hash.Equals(y.hash) && x.random1.Equals(y.random1);
        }

        public int GetHashCode(PublicSecret obj)
        {
            return hash.GetHashCode() ^ random1.GetHashCode();
        }
    }

    [DataContract()]
    public class PrivateSecret
    {
        public PrivateSecret(string data, Guid random2)
        {
            this.data = data;
            this.random2 = random2;
        }

        public PrivateSecret()
        {

        }

        [DataMember()]
        public Guid random2;

        [DataMember()]
        public string data;
    }

    [DataContract()]
    public class Secret
    {
        public Secret(byte[] aData, IDigest aDigester)
        {
            var r1 = Guid.NewGuid();
            var r2 = Guid.NewGuid();
            var digested = new byte[aDigester.GetByteLength()];
            aDigester.Reset();
            aDigester.BlockUpdate(aData, 0, aData.Length);
            aDigester.BlockUpdate(r1.ToByteArray(), 0, r1.ToByteArray().Length);
            aDigester.BlockUpdate(r2.ToByteArray(), 0, r2.ToByteArray().Length);
            aDigester.DoFinal(digested, 0);
            Public = new PublicSecret(digested.GetString(), r1);
            Private = new PrivateSecret(aData.GetString(), r2);
        }

        public Secret(byte[] aData, Guid aRandom1, Guid aRandom2, IDigest aDigester)
        {
            var r1 = aRandom1.ToByteArray();
            var r2 = aRandom2.ToByteArray();
            var digested = new byte[aDigester.GetByteLength()];
            aDigester.Reset();
            aDigester.BlockUpdate(aData, 0, aData.Length);
            aDigester.BlockUpdate(r1, 0, r1.Length);
            aDigester.BlockUpdate(r2, 0, r2.Length);
            aDigester.DoFinal(digested, 0);
            Public = new PublicSecret(digested.GetString(), aRandom1);
            Private = new PrivateSecret(aData.GetString(), aRandom2);
        }

        private PrivateSecret _private;

        [DataMember()]
        public PrivateSecret Private
        {
            get { return _private; }
            set { _private = value; }
        }

        private PublicSecret _public;
                    
        [DataMember()]
        public PublicSecret Public
        {
            get { return _public; }
            set { _public = value; }
        }
    }

}
