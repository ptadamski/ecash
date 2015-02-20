using Bank.Data;
using Common;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Bank.Interface
{
    [ServiceContract(CallbackContract = typeof(IBankServiceCallback))]
    public interface IBankService
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void doInit(Banknote aBanknote);

        [OperationContract(IsOneWay = true)]
        void doCreateAgreement(string[] aBlindMessages);      

        [OperationContract(IsOneWay = true)]
        void doVerifyAgreement(PublicSecret[] aSecrets, string[] aBlindingFactors);    

        [OperationContract(IsOneWay = true)]
        void doCreateSecret(PublicSecret aSecret);

        [OperationContract(IsOneWay = true)]
        void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate);
    }

    public interface IBankServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onInit(Banknote aBanknote, int aBanknoteCount, string aPublicKey);

        [OperationContract(IsOneWay = true)]
        void onCreateAgreement(int aIndex);

        [OperationContract(IsOneWay = true)]
        void onVerifyAgreement(PublicSecret aBanknote, string aSignature, bool aAgreed);

        [OperationContract(IsOneWay = true)]
        void doUncoverSecret(PublicSecret aSecret);

        [OperationContract(IsOneWay = true)]
        void onVerifySecret(PublicSecret aSecret, bool aAgreed);
    }

    public interface IBank
    {
        void doInit(Banknote aBanknote);
        void doCreateAgreement(IList<byte[]> aBlindMessages);
        void doCreateSecret(PublicSecret aSecret);
        void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate);
        void doVerifyAgreement(PublicSecret[] aSecrets, BigInteger[] aBlindingFactors);
    }

    public interface IBankCallback
    {
        void onInit(Banknote aBanknote, int aBanknoteCount, RsaKeyParameters aPublicKey);
        void onCreateAgreement(int aIndex);
        void onVerifyAgreement(PublicSecret aBanknote, byte[] aSignature, bool aAgreed);
        void doUncoverSecret(PublicSecret aSecret);
        void onVerifySecret(PublicSecret aSecret, bool aAgreed);
    }
}