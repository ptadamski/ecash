using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Org.BouncyCastle.Crypto.Digests;
using Alicja.BankService;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;

namespace Alicja
{
    public class BankServiceCallback : BankService.IBankServiceCallback
    {
        private Guid _identity = Guid.NewGuid();
        RsaKeyParameters _publicKey;

        private IBankService _service;

        public IBankService Service
        {
            get { return _service; }
            set { _service = value; }
        }


        public class IdentitySplitter
        {
            public IdentitySplitter(Guid aId, int aParts)
            {
                this.parts = new Guid[aParts];

                Guid id = new Guid();                    
                for (int i = 1; i < aParts; i++) 
                {
                    parts[i] = Guid.NewGuid();
                    id = id.Xor(parts[i]);  
                }

                if (aParts>0)
                    parts[0] = id.Xor(aId);
            }

            private Guid[] parts;

            public Guid[] Parts
            {
                get { return parts; }
                set { parts = value; }
            }

        }

        public class BanknoteAgreement
        {
            public Banknote Banknote { get; set; }
            public Secret[][] Identities { get; set; }
           // public PrivateSecret[] L_UserId { get; set; }
           // public PrivateSecret[] R_UserId { get; set; }  
            public BigInteger BlindingFactor { get; set; }                          
            public Secret BanknoteHash { get; set; }
        }

        public class Agreement
        {
            public BanknoteAgreement[] Banknotes { get; set; }
            public int ForSignature { get; set; }
        }

        Agreement _agreement = null;

        public void onInit(Banknote aBanknote, int aBanknoteCount, string aPublicKey)
        {
            _agreement = new Agreement();
            _agreement.Banknotes = new BanknoteAgreement[aBanknoteCount];

            int MaxIdentityCount = aBanknote.UserId.Length;
            int MaxSubIdentityCount = 2;
                                
            //generowanie banknotow
            Sha256Digest digester = new Sha256Digest();
            List<Banknote> banknotes = new List<Banknote>(aBanknoteCount);

            for (int i = 0; i < aBanknoteCount; i++) //dla kazdego banknotu
            {
                var banknote = new Banknote() { Serial = aBanknote.Serial, Value = aBanknote.Value, UserId = new Identity[aBanknote.UserId.Length] };

                _agreement.Banknotes[i] = new BanknoteAgreement() { Banknote = banknote, Identities = new Secret[MaxIdentityCount][] };

                //generowanie ciagow identyfikacyjnych 
                for (int j = 0; j < MaxIdentityCount; j++) //dla kazdego ciagu identyfikacyjnego w banknocie
                {                            
                    banknote.UserId[j] = new Identity();
                    _agreement.Banknotes[i].Identities[j] = new Secret[MaxSubIdentityCount];
                    IdentitySplitter ident = new IdentitySplitter(_identity, MaxSubIdentityCount);
                    for (int k = 0; k < MaxSubIdentityCount; k++)
                    {
                        _agreement.Banknotes[i].Identities[j][k] = new Secret(ident.Parts[k].ToByteArray(), digester);   
                        banknote.UserId[j].PartialId = new PublicSecret[MaxSubIdentityCount];
                        banknote.UserId[j].PartialId[k] = _agreement.Banknotes[i].Identities[j][k].Public;
                    }
                }
            }      //*/

            //generowanie skrotow banknotow
            List<byte[]> digestedBanknoteList = new List<byte[]>(aBanknoteCount);
            for (int i = 0; i < digestedBanknoteList.Capacity; i++)
            {                                                    
                digester.Reset();                                             
                var banknoteHash = new byte[digester.GetByteLength()];
                                                                              
                var xmlString = _agreement.Banknotes[i].Banknote.ToXml();
                var xmlBanknote = xmlString.GetBytes();
                _agreement.Banknotes[i].BanknoteHash = new Secret(xmlBanknote, digester);
                digestedBanknoteList.Add(_agreement.Banknotes[i].BanknoteHash.Public.hash.GetBytes());
            }

            //zaslepianie wiadomosci
            _publicKey = aPublicKey.ToRsaPublicKey();

            RsaBlindingEngine eng = new RsaBlindingEngine();
            RsaBlindingFactorGenerator gen = new RsaBlindingFactorGenerator();
            gen.Init(_publicKey);
            var factor = gen.GenerateBlindingFactor();
            RsaBlindingParameters param = new RsaBlindingParameters(_publicKey, factor);
            eng.Init(true, param);

            string[] blindedBanknoteList = new string[aBanknoteCount];
            for (int i = 0; i < blindedBanknoteList.Length; i++)
            {                                                 
                _agreement.Banknotes[i].BlindingFactor = factor;
                var blindedData = eng.ProcessBlock(digestedBanknoteList[i], 0, digestedBanknoteList[i].Length);
                blindedBanknoteList[i] = blindedData.GetString();
            }

            //wyslanie zaslepionych wiadomosci do banku
            _service.doCreateAgreement(blindedBanknoteList);
        }

        public void onCreateAgreement(int aIndex)
        {
            _agreement.ForSignature = aIndex;
            PublicSecret[] _secrets = new PublicSecret[_agreement.Banknotes.Length];
            string[] _factors = new string[_agreement.Banknotes.Length];
            for (int i = 0; i < _agreement.Banknotes.Length; i++)
            {                                                        
                _secrets[i] = _agreement.Banknotes[i].BanknoteHash.Public;
                _factors[i] = _agreement.Banknotes[i].BlindingFactor.ToString(16);
            }
            _service.doVerifyAgreement(_secrets, _factors);
        }

        public void onVerifyAgreement(PublicSecret aBanknote, string aBlindSignature, bool aAgreed)
        {
            if (aAgreed)
            {
                var blindSignature = aBlindSignature.GetBytes();
                RsaBlindingEngine eng = new RsaBlindingEngine();
                RsaBlindingParameters param = new RsaBlindingParameters(_publicKey, _agreement.Banknotes[_agreement.ForSignature].BlindingFactor);
                eng.Init(false, param) ;
                var data = eng.ProcessBlock(blindSignature, 0, blindSignature.Length);
                //data to podpis pod sygnatura
                RsaEngine rsa = new RsaEngine();
                rsa.Init(false, _publicKey);
                var h = rsa.ProcessBlock(data, 0, data.Length).GetString();
                //BigInteger x = new BigInteger(h);
                //BigInteger y = new BigInteger(_agreement.Banknotes[_agreement.ForSignature].BanknoteHash.Public.hash.GetBytes());
                
            }
        }

        public void doUncoverSecret(PublicSecret aSecret)
        {
        }

        public void onVerifySecret(PublicSecret aSecret, bool aAgreed)
        {
        }
    }

}
