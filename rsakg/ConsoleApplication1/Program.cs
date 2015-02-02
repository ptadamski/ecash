using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("stworzono generator kluczy");
            var kg = new RsaKeyPairGenerator(); 
            int strength = args.Length>2 ? int.Parse(args[1]) : 1024;
            kg.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            var pair = kg.GenerateKeyPair();
            Console.WriteLine("wygerowano klucze");

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(pair.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPrivate = Convert.ToBase64String(serializedPrivateBytes);

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pair.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPublic = Convert.ToBase64String(serializedPublicBytes);

            Console.WriteLine("gotowy do zapisu");

            StreamWriter sw_prv = new StreamWriter("rsa.prv");
            sw_prv.Write(serializedPrivate);
            sw_prv.Close();

            StreamWriter sw_pub = new StreamWriter("rsa.pub");
            sw_pub.Write(serializedPublic);
            sw_pub.Close();

            Console.WriteLine("zapisano");
            //RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters) PrivateKeyFactory.CreateKey(Convert.FromBase64String(serializedPrivate));
            //RsaKeyParameters publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(serializedPublic));

        }
    }
}
