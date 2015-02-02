﻿using Org.BouncyCastle.Crypto;
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
        [OperationContract(IsOneWay=true, IsTerminating=false, IsInitiating=true)]
        void doCreate(int value);

        [OperationContract(IsOneWay = true, IsTerminating = true, IsInitiating = false)]
        void doValidate(BankNote banknote, string signature);

        [OperationContract(IsOneWay = true)]
        void doAgreementInit(string[] blindedBanknoteList);

        [OperationContract(IsOneWay = true)]
        void doAgreementVerf(SecretBankNote[] banknoteList);
    }


    interface IOnBankServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementInit(int value, Guid serial, int count);

        [OperationContract(IsOneWay = true)]
        void onPublicKey(string pubKey);
                              
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementVerf(int excludeFromAgreement);

        [OperationContract(IsOneWay = true)]
        void onAfterAgreementVerf(string blindSignature);

        [OperationContract(IsOneWay = true)]
        void onBankNoteValidate(Guid serial, string signature, bool result);
    }
}
