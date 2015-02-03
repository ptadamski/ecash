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

namespace Bank.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IBankServiceCallback))]
    interface IBankService
    {
        [OperationContract(IsOneWay=true, IsInitiating=true)]
        void doHandshake(int value);
                     
        [OperationContract(IsOneWay = true)]
        void doInitialize(string[] blindedBanknotes);

        [OperationContract(IsOneWay = true)]
        void doVerify(string[] banknotes, string[] secrets); 
 
        [OperationContract(IsOneWay = true)]
        void doValidate(string banknote, string signature);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        public void doFinalize();

        public readonly IBanknoteFactory Factory;

        public readonly IBanknoteRepository Repository;

        public readonly string SessionId;
    }
}
