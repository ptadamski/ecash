using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer.Bank;
using Org.BouncyCastle.Crypto.Digests;

namespace Customer
{
    interface IBanknoteRepository
    {
    }

    public class BankCallbackHandler : IBankServiceCallback
    {
        IBanknoteRepository _repository;
        IBankService _service;
        RsaKeyParameters _pub;
        string[] _messageList;
        string[] _blindedMessageList;
        BigInteger[] _blindFactorList;

        public BankCallbackHandler(IBankService service, RsaKeyParameters pub, IBanknoteRepository repository)
        {
            this._service = service;
            this._pub = pub;   
        }

        private BankNote BankNoteCreate(Guid serial, int value) 
        {
            var result = new BankNote();
            return result;
        }

        private IdSeq IdSeqCreate(Guid userId)
        {
            var result = new IdSeq();
            return result;
        }


        public void onBeforeAgreementInit(BankNote banknote, int count)
        {
            Console.WriteLine("onBeforeAgreementInit");        

            RsaBlindingFactorGenerator blindFactorGen = new RsaBlindingFactorGenerator();
            RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
            blindFactorGen.Init(_pub);

            BigInteger blindFactor = blindFactorGen.GenerateBlindingFactor();
            RsaBlindingParameters parameters = new RsaBlindingParameters(_pub, blindFactor);//Z^E (mod N)  
            Sha256Digest digester = new Sha256Digest();

            //blind message
            blindingEngine.Init(true, parameters);

            BankNote[] banknoteList = new BankNote[Common.BanknoteConstants.BANKNOTE_COUNT];
            for (int i = 0; i < banknoteList.Length; i++)
			{
                var item = banknoteList[i] = new BankNote();
                item.Serial = banknote.Serial;
                item.Value = banknote.Value;
                item.UserIdentity = new IdSeq[Common.BanknoteConstants.IDSEQ_COUNT];
                for (int j = 0; j < item.UserIdentity.Length; j++)
                {
                    IdSeq idseq = item.UserIdentity[j] = new IdSeq();
                    idseq.RandNum = Guid.NewGuid();   
  
                    digester.BlockUpdate(input, 0, input.Length);
                    byte[] digestedInput = new byte[digester.GetByteLength()];
                    digester.DoFinal(digestedInput, 0);
                    digester.Reset();

                    idseq.Hash = 
                }
			}

            var blindedData = blindingEngine.ProcessBlock(msg, 0, msg.Length);//Y = M*Z^E 
                                                                      
            //uzyc                                     
            _blindFactorList = new BigInteger[_messageList.Length];

            _service.doAgreementInit(_blindedMessageList);
        }

        public void onBeforeAgreementVerf(int excludeFromAgreement)
        {

            string[] blindFactorList = new string[_messageList[]]

            //uzyc ponizszej funkcji
            _service.doAgreementVerf(_messageList, );



            RsaBlindedEngine rsaBlinded = new RsaBlindedEngine();
            rsaBlinded.Init(true, prv);
            byte[] blinded = rsaBlinded.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D =(M*Z^E)^D=M^D *Z^(E*D)= M^D * Z^1

            // unblind the signature
            RsaBlindingEngine unblindingEngine = new RsaBlindingEngine();
            unblindingEngine.Init(true, parameters);
            byte[] s = unblindingEngine.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D * Z^-1 = M^D * Z^1 * Z^-1 = M^D
        }

        public void onAfterAgreementVerf(string blindSignature)
        {
            Console.WriteLine("onAfterAgreementVerf");
        }

        public void onBankNoteValidate(string banknote, string signature, bool result)
        {
            Console.WriteLine("onBankNoteValidate");
        }


        public void onPublicKey(string pubKey)
        {
            Console.WriteLine("onPublicKey");
        }





        private static void CreatePartialId(Guid user, out Guid left, out Guid right)
        {
            right = Guid.NewGuid();
            byte[] Rarr = right.ToByteArray();
            byte[] Uarr = user.ToByteArray();
            byte[] Larr = new byte[user.ToByteArray().Length];

            for (int i = 0; i < Larr.Length; i++)
                Larr[i] = (byte)(Rarr[i] ^ Uarr[i]);

            left = new Guid(Larr);
        }

        private static void CreateIdSequence(Guid user, out Guid rand1, out Guid rand2, out byte[] hash)
        {
            rand1 = Guid.NewGuid();
            rand2 = Guid.NewGuid();
            Sha256Digest digester = new Sha256Digest();
            hash = new byte[digester.GetByteLength()];

            byte[] Uarr = user.ToByteArray();
            byte[] R1arr = rand1.ToByteArray();
            byte[] R2arr = rand2.ToByteArray();

            digester.BlockUpdate(Uarr, 0, Uarr.Length);
            digester.BlockUpdate(R1arr, 0, R1arr.Length);
            digester.BlockUpdate(R2arr, 0, R2arr.Length);
            digester.DoFinal(hash, 0);
        }
   


    }
}
