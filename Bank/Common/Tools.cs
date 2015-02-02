using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class RsaKeyParametersSerialization
    {
        public static string AsPublic(this RsaKeyParameters pub)
        {
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }

        public static RsaKeyParameters ToPublicRsaKeyParameter(this string str)
        {
            return (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(str));
        }

        public static string AsPrivate(this RsaKeyParameters prv)
        {
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(prv);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        public static RsaKeyParameters ToPrivateRsaKeyParameter(string str)
        {
            return (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(str));
        }

    }

    public struct BanknoteConstants
    {
        public const int IDSEQ_COUNT = 100;
        public const int BANKNOTE_COUNT = 100;
    }
}
