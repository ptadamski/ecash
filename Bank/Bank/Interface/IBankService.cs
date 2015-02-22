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
        void doInit(Banknote aBanknote, bool aUnderCreation);

        [OperationContract(IsOneWay = true)]
        void doCreateAgreement(string[] aBlindMessages);      

        [OperationContract(IsOneWay = true)]
        void doVerifyAgreement(PublicSecret[] aSecrets, string[] aBlindingFactors);

        [OperationContract(IsOneWay = true)]
        void doDepone(Secret aBanknote, string aSignature, int[] aIdIndexList, PrivateSecret[] aPartialIdList); 
    }

    public interface IBankServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onInit(Banknote aBanknote, int aBanknoteCount, string aPublicKey);

        [OperationContract(IsOneWay = true)]
        void onCreateAgreement(int aIndex);

        [OperationContract(IsOneWay = true)]
        void onVerifyAgreement(PublicSecret aBanknote, string aBlindSignature, bool aAgreed);
    }

    public interface IBank
    {
        void doInit(Banknote aBanknote, bool aUnderCreation);
        void doCreateAgreement(IList<byte[]> aBlindMessages);
        void doVerifyAgreement(PublicSecret[] aSecrets, BigInteger[] aBlindingFactors);
        void doDepone(Secret aBanknote, byte[] aSignature, int[] aIdIndexList, PrivateSecret[] aPartialIdList); 
    }

    public interface IBankCallback
    {
        void onInit(Banknote aBanknote, int aBanknoteCount, RsaKeyParameters aPublicKey);
        void onCreateAgreement(int aIndex);
        void onVerifyAgreement(PublicSecret aBanknote, byte[] aBlindSignature, bool aAgreed);
    }
}