using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PublicSecret  : IEqualityComparer<PublicSecret>
    {
        public PublicSecret(string hash, Guid random1)
        {
            this.hash = hash;
            this.random1 = random1;
        }

        public PublicSecret()
        {
        }

        public string hash;
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

        public Guid random2;
        public string data;
    }

    public class Secret
    {
        public Secret(string hash, Guid random1)
        {
            this._public = new PublicSecret(hash, random1);
            this._private = new PrivateSecret();
        }

        public Secret(string hash, Guid random1, Guid random2, string data)
        {
            this._public = new PublicSecret(hash, random1);
            this._private = new PrivateSecret(data, random2);
        }

        private PrivateSecret _private;

        public PrivateSecret Private
        {
            get { return _private; }
            set { _private = value; }
        }

        private PublicSecret _public;

        public PublicSecret Public
        {
            get { return _public; }
            set { _public = value; }
        }
    }

}
