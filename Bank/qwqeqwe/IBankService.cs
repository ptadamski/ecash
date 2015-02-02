using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using Org.BouncyCastle.Crypto.Parameters;

namespace Bank
{
    [ServiceContract(CallbackContract = typeof(IOnBankServiceCallback))]
    interface IBankService
    {
        [OperationContract(IsOneWay=true)]
        void doCreate(BankNote banknote);
            
        [OperationContract(IsOneWay = true)]
        void doValidate(string banknote, string signature);

        [OperationContract(IsOneWay = true)]
        void doAgreementInit(string[] blindedMessageList);

        [OperationContract(IsOneWay = true)]
        void doAgreementVerf(string[] messageList, string[] blindingFactorList);
    }


    interface IOnBankServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementInit(BankNote banknote, int count);

        [OperationContract(IsOneWay = true)]
        void onPublicKey(string pubKey);
                              
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementVerf(int excludeFromAgreement);

        [OperationContract(IsOneWay = true)]
        void onAfterAgreementVerf(string blindSignature);

        [OperationContract(IsOneWay = true)]
        void onBankNoteValidate(string banknote, string signature, bool result);
    }
}
