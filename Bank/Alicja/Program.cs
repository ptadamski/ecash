using Common;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alicja
{
    class Program
    {
        static void Main(string[] args)
        {
            PublicSecret pub = new PublicSecret();

            Sha256Digest digester = new Sha256Digest();
            byte[] digest = new byte[digester.GetDigestSize()];
            byte[] data = "some string".GetBytes();
            digester.BlockUpdate(data, 0 , data.Length);
            digester.DoFinal(digest,0);

            pub.hash = digest.GetString();
            pub.random1 = Guid.NewGuid();

            System.IO.StreamWriter sw = new System.IO.StreamWriter("somexml.xml");
            sw.Write(pub.ToXml());
            sw.Close();
        }
    }
}
