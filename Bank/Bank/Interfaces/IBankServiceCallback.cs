using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Interfaces
{
    [ServiceContract()]
    public interface IBankServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void onHandshake(string banknote, int banknoteCount, int idPerBanknoteCount, string publicKey);

        [OperationContract(IsOneWay = true)]
        void onInitialize(string banknote, int excludeFromAgreement);

        [OperationContract(IsOneWay = true)]
        void onVerification(string banknote, string blindSignature);

        [OperationContract(IsOneWay = true)]
        void onValidate(string banknote, string signature, bool result);
    }
}
