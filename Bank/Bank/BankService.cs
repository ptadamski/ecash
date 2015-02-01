using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using System.Xml.Serialization;
using System.IO;
using Org.BouncyCastle.Crypto.Digests;
using Common;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Bank
{
    public class Agreement
    {
        public string[] BlindedMessages { get; set; }
        public int ExcludedItem { get; set; }
        public int Nominal { get; set; }
        public Guid SerialNumber { get; set; }
        public int Count { get; set; }
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class BankService : IBankService
    {
        static Dictionary<Guid, int> banknotes = new Dictionary<Guid, int>();
        static Dictionary<string, Guid> sesPerGuid = new Dictionary<string, Guid>();



        //klucz powinien byc zczytywany z pliku 

        static private AsymmetricCipherKeyPair _keyPair = ReadKeyPair(1024);
        static private AsymmetricCipherKeyPair ReadKeyPair(int strength) 
        {
            var kg = new RsaKeyPairGenerator();
            kg.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            return kg.GenerateKeyPair(); 
        }



        //zapisywane na potrzeby porozumienia bitowego
        static Dictionary<string, Agreement> agreement = new Dictionary<string, Agreement>();

        public void doCreate(BankNote banknote)
        {

            Sha256Digest digester = new Sha256Digest();
            digester.DoFinal()

            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            System.Console.WriteLine(banknote.Serial);
            System.Console.WriteLine(banknote.Value);

            callback.onBeforeAgreementInit(banknote, 100);

            callback.onPublicKey((_keyPair.Public as RsaKeyParameters).AsPublic());
        }

        public void doValidate(string banknote, string signature)  
        {                             
            var data = banknote.GetBytes();
            var sign = signature.GetBytes();

            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            RsaEngine eng = new RsaEngine();
            eng.Init(false, _keyPair.Private);
            var encoded = eng.ProcessBlock(data, 0, data.Length);

            bool result = encoded.Length == sign.Length;
            if (result)
                for (int i = 0; i < encoded.Length; i++)
                    if (encoded[i] != sign[i])
                    {
                        result = false;
                        break;
                    }
            callback.onBankNoteValidate(banknote, signature, result);
        }

        public void doAgreementInit(string[] blindedMessageList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;
        }

        public void doAgreementVerf(string[] messageList, string[] blindingFactorList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;
        }
    }
             
}

/*
            RsaBlindingFactorGenerator blindFactorGen = new RsaBlindingFactorGenerator();
            RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
            blindFactorGen.Init(pub);

            BigInteger blindFactor = new BigInteger("7");//blindFactorGen.GenerateBlindingFactor();
            RsaBlindingParameters parameters = new RsaBlindingParameters(pub, blindFactor);//Z^E (mod N)
            //blind message
            blindingEngine.Init(true, parameters);
            blindedData = blindingEngine.ProcessBlock(msg, 0, msg.Length);//Y = M*Z^E 

            RsaBlindedEngine rsaBlinded = new RsaBlindedEngine();
            rsaBlinded.Init(true, prv);
            byte[] blinded = rsaBlinded.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D =(M*Z^E)^D=M^D *Z^(E*D)= M^D * Z^1

            // unblind the signature
            RsaBlindingEngine unblindingEngine = new RsaBlindingEngine();
            unblindingEngine.Init(true, parameters);
            byte[] s = unblindingEngine.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D * Z^-1 = M^D * Z^1 * Z^-1 = M^D
 */
