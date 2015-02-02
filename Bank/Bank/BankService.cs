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
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Signers;

namespace Bank
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class BankService : IBankService
    {

        public class Agreement
        {
            public string[] blindedMessages;
            public int without;
            public Guid serial;
        }

        static Random _rand = new Random();
        static Dictionary<Guid, Agreement> _agreements = new Dictionary<Guid, Agreement>();
        static Dictionary<string, Guid> _guids = new Dictionary<string, Guid>();

        //klucz powinien byc zczytywany z pliku 

        static private AsymmetricCipherKeyPair _keyPair = ReadKeyPair(12 * 1024);

        static private AsymmetricCipherKeyPair ReadKeyPair(int strength)
        {
            var kg = new RsaKeyPairGenerator();
            kg.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            return kg.GenerateKeyPair();
        }

        public BankService()
        {
            OperationContext.Current.InstanceContext.Closed += Cleanup;
        }

        public void Cleanup(object sender, EventArgs e)
        {
            string sesId = OperationContext.Current.SessionId;

            var guid = _guids[sesId];
            var agreement = _agreements[guid];

            _guids.Remove(sesId);
            _agreements.Remove(guid);

            OperationContext.Current.InstanceContext.Closed -= Cleanup;
        }

        public void doCreate(int value)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            var banknote = new BankNote();
            var serial = InitBanknote(sesId);

            callback.onBeforeAgreementInit(value, serial, BanknoteConstants.BANKNOTE_COUNT);
            callback.onPublicKey((_keyPair.Public as RsaKeyParameters).AsPublic());
        }

        public void doValidate(BankNote banknote, string signature)
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
            callback.onBankNoteValidate(banknote.Serial, signature, result);
        }

        public void doAgreementInit(string[] blindedBanknoteList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            if (_guids.ContainsKey(sesId))
            {
                var guid = _guids[sesId];
                var agree = _agreements[guid];

                agree.without = _rand.Next(0, Common.BanknoteConstants.BANKNOTE_COUNT);
                agree.serial = guid;
                agree.blindedMessages = blindedMessageList;

                callback.onBeforeAgreementVerf(agree.without);
            }
        }

        public void doAgreementVerf(SecretBankNote[] banknoteList)
        {
            IOnBankServiceCallback callback = OperationContext.Current.GetCallbackChannel<IOnBankServiceCallback>();
            string sesId = OperationContext.Current.SessionId;

            if (_guids.ContainsKey(sesId))
            {
                var guid = _guids[sesId];
                var agree = _agreements[guid];

                if (messageList.Length != agree.blindedMessages.Length && messageList.Length != BanknoteConstants.BANKNOTE_COUNT)
                    return;

                //TO DO: otrzymane niezaciemnione wiadomosci powinny posiadac ciagi identyfikacyjne, takie ze zaden sie nie powtarza

                RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
                Sha512Digest digester = new Sha512Digest(); //TO DO dodatkowy mechanizm kojarzacy skroty z banknotami

                #region banknote validation test
                for (int i = 0; i < messageList.Length; i++)
                {
                    if (i == agree.without)
                        continue;

                    BankNote banknote;
                    messageList[i].FromXml(out banknote);
                    if (!BanknoteIsValid(banknote, digester))
                        return;//exit on cheat
                }
                #endregion

                #region banknote hash agreement test
                for (int i = 0; i < messageList.Length; i++)
                {
                    if (i == agree.without)
                        continue;

                    byte[] input = messageList[i].GetBytes();

                    digester.BlockUpdate(input, 0, input.Length);
                    byte[] digestedInput = new byte[digester.GetByteLength()];
                    digester.DoFinal(digestedInput, 0);
                    digester.Reset();


                    BigInteger blindFactor = new BigInteger(blindingFactorList[i]);
                    RsaBlindingParameters parameters = new RsaBlindingParameters((RsaKeyParameters)_keyPair.Public, blindFactor);
                    blindingEngine.Init(true, parameters);
                    byte[] blindedData = blindingEngine.ProcessBlock(digestedInput, 0, digestedInput.Length);

                    if (!isEqualTo(blindedData, agree.blindedMessages[i].GetBytes()))
                        return; //exit on cheat
                }
                #endregion

                #region blindly sing
                RsaEngine eng = new RsaEngine();
                eng.Init(true, _keyPair.Private);
                var blindedMessage = agree.blindedMessages[agree.without].GetBytes();
                var blindSignature = eng.ProcessBlock(blindedMessage, 0, blindedMessage.Length);

                callback.onBankNoteValidate(agree.blindedMessages[agree.without], blindSignature.GetString(), true);
                //H = Hash(M)
                //S = Sign(H)
                //return (M,S)
                #endregion
            }
        }

        #region Validation
        private bool isEqualTo(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i != a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }
        
        private bool BanknoteIsValid(BankNote banknote, IDigest digester)
        {
            foreach (var seq in banknote.UserIdentity)
            {
                if (!UserIdSeqIsValid(seq, digester))
                    return false;
            }
            return true;
        }

        private bool UserIdSeqIsValid(UserPartId partId, IDigest digester)
        {
            return partId.Hash.Length == digester.GetByteLength();
        }
        #endregion

        private Guid InitBanknote(string sesId)
        {
            Agreement dummy;
            Guid guid = Guid.NewGuid();

            while (_agreements.TryGetValue(guid, out dummy))
                guid = Guid.NewGuid();

            _guids[sesId] = guid;
            var agree = _agreements[guid] = new Agreement();

            return guid;
        }
    }
}
