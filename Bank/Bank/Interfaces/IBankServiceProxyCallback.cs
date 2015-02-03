using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Interfaces
{
    public interface IBankServiceProxyCallback
    {
        void onHandshake(Banknote banknote, int banknoteCount, int idPerBanknoteCount, RsaKeyParameters publicKey);
                                                                              
        void onInitialize(Banknote banknote, int excludeFromAgreement);

        void onVerification(Banknote banknote, byte[] blindSignature);

        void onValidate(Banknote banknote, byte[] signature, bool result);
    }
}
