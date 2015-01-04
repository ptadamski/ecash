using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bank
{
    [ServiceContract(CallbackContract=typeof(IOnBankService))]
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode=InstanceContextMode.PerCall)]
    interface IBankService
    {
        /// <summary>
        /// tworzenie banknotu
        /// </summary>
        /// <param name="nominal">wartosc nominalu</param>
        [OperationContract(IsOneWay=true)]
        void doBankNoteInit(int nominal);

        /// <summary>
        /// weryfikacja banknotu
        /// </summary>
        /// <param name="banknote">zserializowany i zaszyfrowany banknot</param>
        /// <param name="signature">signatura nadana przez bank</param>
        [OperationContract(IsOneWay = true)]
        void doValidate(string banknote, string signature);
    }


    interface IOnBankService
    {
        /// <summary>
        /// zdarzenie odpalane podczas procedury tworzenia banknotu
        /// </summary>
        /// <param name="serialNumber">128 bitowy numer seryjny dostepny dla klienta</param>
        [OperationContract(IsOneWay = true)]
        void onBanknoteInit(Guid serialNumber);

        /// <summary>
        /// odpowiedz na weryfikacje bannotu
        /// </summary>
        /// <param name="banknote">zserializowany i zaszyfrowany banknot</param>
        /// <param name="signature">signatura nadana przez bank</param>
        /// <param name="result">wynik sprawdzenia poprawnosci</param>
        [OperationContract(IsOneWay = true)]
        void onValidate(string banknote, string signature, bool result);
    }
}
