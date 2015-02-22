using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System.ServiceModel;


namespace Sklep.Interface
{
    [ServiceContract(CallbackContract = typeof(IShopServiceCallback))]
    interface IShopService
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void doInit(Banknote aBanknote);

        [OperationContract(IsOneWay = true)]
        void doVerifySignature();      

        [OperationContract(IsOneWay = true)]
        void doChooseSides();    

        [OperationContract(IsOneWay = true)]
        void doVerifyBanknote();

        //[OperationContract(IsOneWay = true)]
        //void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate);
    }

    public interface IShopServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onInit();

        [OperationContract(IsOneWay = true)]
        void onVerifySignature();

        [OperationContract(IsOneWay = true)]
        void onChooseSides();

        [OperationContract(IsOneWay = true)]
        void doVerifyBanknote();

        //[OperationContract(IsOneWay = true)]
        //void onVerifySecret(PublicSecret aSecret, bool aAgreed);
    }

    interface IShop
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void doInit(Banknote aBanknote);

        [OperationContract(IsOneWay = true)]
        void doVerifySignature();

        [OperationContract(IsOneWay = true)]
        void doChooseSides();

        [OperationContract(IsOneWay = true)]
        void doVerifyBanknote();

        //[OperationContract(IsOneWay = true)]
        //void doVerifySecret(PublicSecret aPublic, PrivateSecret aPrivate);
    }

    public interface IShopCallback
    {
        [OperationContract(IsOneWay = true)]
        void onInit();

        [OperationContract(IsOneWay = true)]
        void onVerifySignature();

        [OperationContract(IsOneWay = true)]
        void onChooseSides();

        [OperationContract(IsOneWay = true)]
        void doVerifyBanknote();

        //[OperationContract(IsOneWay = true)]
        //void onVerifySecret(PublicSecret aSecret, bool aAgreed);
    }
}
