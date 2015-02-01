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
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Common;
using System.ServiceModel;

namespace Customer
{
    

    class Program
    {
        //static void Main(string[] args)
        //{

            /*string str = "ala ma pchly";
            byte[] msg = str.GetBytes();
                        
            RsaKeyPairGenerator keyGen =  new RsaKeyPairGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            AsymmetricCipherKeyPair keys =  keyGen.GenerateKeyPair();
            var pub = keys.Public;                                      
            var prv = keys.Private;

            RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
                                                                            
            RsaBlindingFactorGenerator blindFactorGen = new RsaBlindingFactorGenerator();  
            blindFactorGen.Init(pub);
            BigInteger blindFactor = blindFactorGen.GenerateBlindingFactor(); //Z


            RsaBlindingParameters parameters = new RsaBlindingParameters((RsaKeyParameters)pub, blindFactor);//Z^E (mod N)

            //blind message - alice do banku
            blindingEngine.Init(true, parameters);
            var blindedData = blindingEngine.ProcessBlock(msg, 0, msg.Length);//Y = M*Z^E 

            RsaBlindedEngine rsaBlinded = new RsaBlindedEngine();
            rsaBlinded.Init(true, prv);
            var blindedSignature = rsaBlinded.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D =(M*Z^E)^D=M^D *Z^(E*D)= M^D * Z^1

            // unblind the signature - bank do alice
            RsaBlindingEngine unblindingEngine = new RsaBlindingEngine();
            unblindingEngine.Init(false, parameters);
            var unblindedSignature = unblindingEngine.ProcessBlock(blindedSignature, 0, blindedSignature.Length);//Y^D * Z^-1 = M^D * Z^1 * Z^-1 = M^D


            //test - czy faktycznie bank podpisal banknot swoim podpisem
            RsaEngine validator = new RsaEngine();
            validator.Init(true, pub);
            var forValidation = validator.ProcessBlock(unblindedSignature, 0, unblindedSignature.Length);



            RsaEngine rsaPrivate = new RsaEngine();
            rsaPrivate.Init(true, prv);
            var prvMsg =  rsaPrivate.ProcessBlock(msg, 0, msg.Length);



                                */

            //Bank.BankService service = new Bank.BankService();
            //Bank.BankNote banknote =  new Bank.BankNote();
            //banknote.Serial = Guid.NewGuid().ToString();
           // banknote.Value = 1000;
           // service.doCreate(banknote);
        //}

        public class BankCallbackHandler : Bank.IBankServiceCallback 
        {
            public void onBeforeAgreementInit(Bank.BankNote banknote, int count)
            {
                Console.WriteLine("onBeforeAgreementInit");
            }

            public void onPublicKey(string e, string n)
            {
                Console.WriteLine("onPublicKey");
            }

            public void onBeforeAgreementVerf(int excludeFromAgreement)
            {
                Console.WriteLine("onBeforeAgreementVerf");
            }

            public void onAfterAgreementVerf(string blindSignature)
            {
                Console.WriteLine("onAfterAgreementVerf");
            }

            public void onBankNoteValidate(string banknote, string signature, bool result)
            {
                Console.WriteLine("onBankNoteValidate");
            }
        }

        static public void Main(string[] args)
        {
            try
            {
                Bank.BankServiceClient service = new Bank.BankServiceClient(new InstanceContext(new BankCallbackHandler()));
                Bank.BankNote banknote = new Bank.BankNote();
                banknote.Serial = Guid.NewGuid();
                banknote.Value = 9000;
                service.doCreate(banknote);
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();

        }


    }
}
