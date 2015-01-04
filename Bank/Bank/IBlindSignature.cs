using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Numerics;
using Org.BouncyCastle.Crypto;

namespace Bank
{
    /// <summary>
    /// protokolu slepego podpisu
    /// </summary>
    [ServiceContract(CallbackContract=typeof(IOnBlindSignatureCallback))]
    interface IBlindSignature //alice, shop
    {
        /// <summary>
        /// inicjalizacja protokolu
        /// </summary>
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void doInitialize();

        /// <summary>
        /// wyslanie zakrytych wiadomosc
        /// </summary>
        /// <param name="blindedMessageList">zakryte wiadomosci</param>
        [OperationContract(IsOneWay = true)]
        void doSendSecret(string[] blindedMessageList);

        /// <summary>
        /// odkrycie zakrytych wiadomosci
        /// </summary>
        /// <param name="blindedMessageList">lsita zakrytych wiadomosci</param>
        /// <param name="blindingFactorList">parametry do odkrycia wiadomosci</param>
        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void doUncoverSecret(string[] blindedMessageList, BigInteger[] blindingFactorList);
    }

    /// <summary>
    /// zdarzenia do protokolu slepego podpisu
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface IOnBlindSignatureCallback //bank
    {
        /// <summary>
        /// zdarzenie odpalane na inicjalizacje protkolu
        /// </summary>
        /// <param name="secretsCount">liczba wiadomosci do wygenerowania</param>
        /// <param name="key">klucz publiczny</param>
        [OperationContract(IsOneWay=true)]
        void onInitialize(int secretsCount, ICipherParameters key);

        /// <summary>
        /// zdarzenie informujace o wykluczeniu wskazanej wiadomosci z fazy odkrywania sekretow
        /// </summary>
        /// <param name="index">indeks wiadomosci na liscie zakrytych wiadomosci</param>
        [OperationContract(IsOneWay = true)]
        void onExcludeItem(int index);

        /// <summary>
        /// zdarzenie odpalone na zakonczenie protokolu
        /// </summary>
        /// <param name="blindedMessage">zakryta wiadomosc</param>
        /// <param name="signature">utworzona sygnatura pod wiadomoscia</param>
        [OperationContract(IsOneWay = true)]
        void onFinalize(string blindedMessage, string signature);
    }
}

