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
        /// <summary>
        /// tworzenie banknotu
        /// </summary>
        /// <param name="nominal">wartosc nominalu</param>
        [OperationContract(IsOneWay=true, IsInitiating=true)]
        void doBankNoteInit(int nominal);

        /// <summary>
        /// weryfikacja banknotu
        /// </summary>
        /// <param name="banknote">zserializowany i zaszyfrowany banknot</param>
        /// <param name="signature">signatura nadana przez bank</param>               
        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void doBankNoteValidate(string banknote, string signature);

        [OperationContract(IsOneWay = true)]
        void doAgreementInit(string[] blindedMessageList);

        [OperationContract(IsOneWay = true)]
        void doAgreementVerf(string[] messageList, string[] blindingFactorList);
    }


    interface IOnBankServiceCallback
    {
        /// <summary>
        /// zdarzenie odpalane podczas procedury tworzenia banknotu
        /// </summary>
        /// <param name="serialNumber">128 bitowy numer seryjny dostepny dla klienta</param>
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementInit(Guid serialNumber, int nominal, int copiesCount, AsymmetricKeyParameter pubKey);
                              
        [OperationContract(IsOneWay = true)]
        void onBeforeAgreementVerf(int excludeFromAgreement);

        [OperationContract(IsOneWay = true)]
        void onAfterAgreementVerf(string blindSignature);

        /// <summary>
        /// odpowiedz na weryfikacje bannotu
        /// </summary>
        /// <param name="banknote">zserializowany i zaszyfrowany banknot</param>
        /// <param name="signature">signatura nadana przez bank</param>
        /// <param name="result">wynik sprawdzenia poprawnosci</param>
        [OperationContract(IsOneWay = true)]
        void onBankNoteValidate(string banknote, string signature, bool result);
    }
}
