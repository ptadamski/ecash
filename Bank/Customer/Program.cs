using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer
{
    

    class Program
    {


        static void Main(string[] args)
        {                                      
            //RsaEngine decrypter = new RsaEngine();

            string text = @"jakis tam belkot o ali: ala ma czarnego kocura, ktoremu codzinnie wisi rura i jara sobie sobie dlugiego sznura ktory czarny jest jak smola i po domu lata fujara gola";

            RsaKeyPairGenerator kpg = new RsaKeyPairGenerator();         
            kpg.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            AsymmetricCipherKeyPair kp = kpg.GenerateKeyPair();
            byte[] dataToEncrypt = text.GetBytes();


            AsymmetricCipherStream acs = new AsymmetricCipherStream(new RsaEngine());
            acs.Init(true, kp.Public);
            byte[] encryptedData = acs.ProcessBlock(dataToEncrypt, 0, dataToEncrypt.Length);

            acs.Init(false, kp.Private);
            byte[] decryptedData = acs.ProcessBlock(encryptedData, 0, encryptedData.Length);

            System.Console.WriteLine(encryptedData.GetString());
            System.Console.WriteLine(decryptedData.GetString());
            System.Console.ReadLine();

        }
    }

    public static class StringExt
    {      
        public static byte[] GetBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
