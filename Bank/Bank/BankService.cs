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

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class BankService : IBankService
    {
        static Dictionary<Guid, int> banknotes = new Dictionary<Guid, int>();
        static Dictionary<string, Guid> sesPerGuid = new Dictionary<string, Guid>();

        //klucz powinien byc zczytywany z pliku    
        static RsaKeyParameters pubKey = null;
        static RsaKeyParameters prvKey = null;

        //zapisywane na potrzeby porozumienia bitowego
        static Dictionary<string, Agreement> agreement = new Dictionary<string, Agreement>();

        public void doBankNoteInit(int nominal)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            Guid id = Guid.NewGuid();
            int val = 0;
            while (banknotes.TryGetValue(id, out val))
                id = Guid.NewGuid();
            agreement[sesId].Nominal = nominal;  
            agreement[sesId].SerialNumber = id;
            agreement[sesId].Count = 100;
            callback.onBeforeAgreementInit(id, nominal, 100, pubKey);
        }

        public void doBankNoteValidate(string banknote, string signature)
        {
            throw new NotImplementedException();
        }

        public void doAgreementInit(string[] blindedMessageList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;
            agreement[sesId].BlindedMessages = blindedMessageList;
            agreement[sesId].ExcludedItem = new Random().Next(0, blindedMessageList.Length - 1);
            callback.onBeforeAgreementVerf(agreement[sesId].ExcludedItem);
        }

        public void doAgreementVerf(string[] messageList, string[] blindingFactorList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
            RsaBlindingParameters parameters = null;
            BigInteger temp = null;

            for (int i = 0; i < agreement[sesId].ExcludedItem; i++)
            {
                parameters = new RsaBlindingParameters(pubKey, new BigInteger(blindingFactorList[i]));
                blindingEngine.Init(true, parameters);
                temp = new BigInteger(blindingEngine.ProcessBlock(Encoding.UTF8.GetBytes(messageList[i]), 0, messageList[i].Length));
                if (agreement[sesId].BlindedMessages[i] != temp.ToString())
                    return;
            }

            for (int i = agreement[sesId].ExcludedItem + 1; i < messageList.Length; i++)
            {
                parameters = new RsaBlindingParameters(pubKey, new BigInteger(blindingFactorList[i]));
                blindingEngine.Init(true, parameters);
                temp = new BigInteger(blindingEngine.ProcessBlock(Encoding.UTF8.GetBytes(messageList[i]), 0, messageList[i].Length));
                if (agreement[sesId].BlindedMessages[i] != temp.ToString())
                    return;
            }

            BankNote banknote = null;

            for (int i = 0; i < agreement[sesId].ExcludedItem; i++)
            {                                                  
                banknote = FromXml<BankNote>(messageList[i]);
                if(banknote.Nominal != agreement[sesId].Nominal)
                    return;
                if (banknote.SerialNumber != agreement[sesId].SerialNumber)    
                    return;
            }

            for (int i = agreement[sesId].ExcludedItem + 1; i < messageList.Length; i++)
            {
                banknote = FromXml<BankNote>(messageList[i]);
                if (banknote.Nominal != agreement[sesId].Nominal)
                    return;
                if (banknote.SerialNumber != agreement[sesId].SerialNumber)
                    return;
                banknote.Left.
            }


            byte[] data = Encoding.UTF8.GetBytes(agreement[sesId].BlindedMessages[agreement[sesId].ExcludedItem]);
            RsaEngine rsaEngine = new RsaEngine();
            rsaEngine.Init(true, prvKey);
            string blindSignature = Encoding.UTF8.GetString(rsaEngine.ProcessBlock(data, 0, data.Length));
            callback.onAfterAgreementVerf(blindSignature);
        }

        static public string ToXml<T>(T e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            serializer.Serialize(textWriter, e);
            return textWriter.ToString();
        }

        static T FromXml<T>(string str)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StringReader(str);
            return  (T) deserializer.Deserialize(textReader);
        }
    }
             
}
