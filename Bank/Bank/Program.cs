using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

namespace Bank
{
    class Program
    {
        static private byte[] slt1a = Hex.Decode("dee959c7e06411361420ff80185ed57f3e6776af");

        static private RsaKeyParameters pub1 = new RsaKeyParameters(false,
            new BigInteger("a56e4a0e701017589a5187dc7ea841d156f2ec0e36ad52a44dfeb1e61f7ad991d8c51056ffedb162b4c0f283a12a88a394dff526ab7291cbb307ceabfce0b1dfd5cd9508096d5b2b8b6df5d671ef6377c0921cb23c270a70e2598e6ff89d19f105acc2d3f0cb35f29280e1386b6f64c4ef22e1e1f20d0ce8cffb2249bd9a2137", 16),
            new BigInteger("010001", 16));

        static private byte[] sig1a = Hex.Decode("9074308fb598e9701b2294388e52f971faac2b60a5145af185df5287b5ed2887e57ce7fd44dc8634e407c8e0e4360bc226f3ec227f9d9e54638e8d31f5051215df6ebb9c2f9579aa77598a38f914b5b9c1bd83c4e2f9f382a0d0aa3542ffee65984a601bc69eb28deb27dca12c82c2d4c3f66cd500f1ff2b994d8a4e30cbb33c");


        static private RsaKeyParameters prv1 = new RsaPrivateCrtKeyParameters(
                    new BigInteger("a56e4a0e701017589a5187dc7ea841d156f2ec0e36ad52a44dfeb1e61f7ad991d8c51056ffedb162b4c0f283a12a88a394dff526ab7291cbb307ceabfce0b1dfd5cd9508096d5b2b8b6df5d671ef6377c0921cb23c270a70e2598e6ff89d19f105acc2d3f0cb35f29280e1386b6f64c4ef22e1e1f20d0ce8cffb2249bd9a2137", 16),
                    new BigInteger("010001", 16),
                    new BigInteger("33a5042a90b27d4f5451ca9bbbd0b44771a101af884340aef9885f2a4bbe92e894a724ac3c568c8f97853ad07c0266c8c6a3ca0929f1e8f11231884429fc4d9ae55fee896a10ce707c3ed7e734e44727a39574501a532683109c2abacaba283c31b4bd2f53c3ee37e352cee34f9e503bd80c0622ad79c6dcee883547c6a3b325", 16),
                    new BigInteger("e7e8942720a877517273a356053ea2a1bc0c94aa72d55c6e86296b2dfc967948c0a72cbccca7eacb35706e09a1df55a1535bd9b3cc34160b3b6dcd3eda8e6443", 16),
                    new BigInteger("b69dca1cf7d4d7ec81e75b90fcca874abcde123fd2700180aa90479b6e48de8d67ed24f9f19d85ba275874f542cd20dc723e6963364a1f9425452b269a6799fd", 16),
                    new BigInteger("28fa13938655be1f8a159cbaca5a72ea190c30089e19cd274a556f36c4f6e19f554b34c077790427bbdd8dd3ede2448328f385d81b30e8e43b2fffa027861979", 16),
                    new BigInteger("1a8b38f398fa712049898d7fb79ee0a77668791299cdfa09efc0e507acb21ed74301ef5bfd48be455eaeb6e1678255827580a8e4e8e14151d1510a82a3f2e729", 16),
                    new BigInteger("27156aba4126d24a81f3a528cbfb27f56886f840a9f6e86e17a44b94fe9319584b8e22fdde1e5a2e3bd8aa5ba8d8584194eb2190acf832b847f13a3d24a79f4d", 16));
    


        public static AsymmetricCipherKeyPair generateKeys(int keySize)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();

            r.Init(new RsaKeyGenerationParameters(new BigInteger("10001", 16), new SecureRandom(),
                       keySize, 80));

            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();

            return keys;
        }

        public static BigInteger generateBlindingFactor(ICipherParameters pubKey)
        {
            RsaBlindingFactorGenerator gen = new RsaBlindingFactorGenerator();

            gen.Init(pubKey);

            return gen.GenerateBlindingFactor();
        }

        public static byte[] blind(ICipherParameters key, BigInteger factor, byte[] msg)
        {
            RsaBlindingEngine eng = new RsaBlindingEngine();

            RsaBlindingParameters param = new RsaBlindingParameters((RsaKeyParameters)key, factor);
            PssSigner blindSigner = new PssSigner(eng, new Sha1Digest(), 15);
            blindSigner.Init(true, param);

            blindSigner.BlockUpdate(msg, 0, msg.Length);

            byte[] blinded = null;
            try
            {
                blinded = blindSigner.GenerateSignature();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ");
            }

            return blinded;
        }

        public static byte[] unblind(ICipherParameters key, BigInteger factor, byte[] msg)
        {
            RsaBlindingEngine eng = new RsaBlindingEngine();

            RsaBlindingParameters param = new RsaBlindingParameters((RsaKeyParameters)key, factor);

            eng.Init(false, param);

            return eng.ProcessBlock(msg, 0, msg.Length);
        }

        public static byte[] sign(ICipherParameters key, byte[] toSign)
        {
            Sha1Digest dig = new Sha1Digest();
            RsaEngine eng = new RsaEngine();

            PssSigner signer = new PssSigner(eng, dig, 15);
            signer.Init(true, key);
            signer.BlockUpdate(toSign, 0, toSign.Length);

            byte[] sig = null;

            try
            {
                sig = signer.GenerateSignature();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ");
            }

            return sig;
        }

        public static bool verify(ICipherParameters key, byte[] msg, byte[] sig)
        {
            PssSigner signer = new PssSigner(new RsaEngine(), new Sha1Digest(), 15);
            signer.Init(false, key);

            signer.BlockUpdate(msg, 0, msg.Length);

            return signer.VerifySignature(sig);
        }

        public static byte[] signBlinded(ICipherParameters key, byte[] msg)
        {
            RsaEngine signer = new RsaEngine();
            signer.Init(true, key);
            return signer.ProcessBlock(msg, 0, msg.Length);
        }

        private class FixedRandom
            : SecureRandom
        {
            private readonly byte[] vals;

            public FixedRandom(
                byte[] vals)
            {
                this.vals = vals;
            }

            public override void NextBytes(
                byte[] bytes)
            {
                Array.Copy(vals, 0, bytes, 0, vals.Length);
            }
        }

        static private void testSig(int id, RsaKeyParameters pub, RsaKeyParameters prv, byte[] msg)
        {
           /* RsaBlindingFactorGenerator blindFactorGen = new RsaBlindingFactorGenerator();
            RsaBlindingEngine blindingEngine = new RsaBlindingEngine();
            PssSigner blindSigner = new PssSigner(blindingEngine, new Sha1Digest(), 20);
            PssSigner signer = new PssSigner(new RsaEngine(), new Sha1Digest(), 20);



            blindFactorGen.Init(pub);

            BigInteger blindFactor = new BigInteger("7");//blindFactorGen.GenerateBlindingFactor();
            RsaBlindingParameters parameters = new RsaBlindingParameters(pub, blindFactor);//Z^E (mod N)

            //pub = (E,N)
            //priv = (D,N)
            //msg = M


            // generate a blind signature
            blindSigner.Init(true, parameters);

            blindSigner.BlockUpdate(msg, 0, msg.Length);

            byte[] blindedData = blindSigner.GenerateSignature();//Y= M*Z^E

            RsaEngine signerEngine = new RsaEngine();

            signerEngine.Init(true, prv);

            byte[] blindedSig = signerEngine.ProcessBlock(blindedData, 0, blindedData.Length);//Y^D =(M*Z^E)^D=M^D *Z^(E*D)= M^D

            //blind message
            blindingEngine.Init(true, parameters);
            blindedData = blindingEngine.ProcessBlock(msg, 0, msg.Length);

            RsaBlindedEngine rsaBlinded = new RsaBlindedEngine();
            rsaBlinded.Init(true, prv);
            byte[] rsaBlinded.ProcessBlock(blindedData, 0, blindedData.Length);

            // unblind the signature
            RsaBlindingEngine unblindingEngine = new RsaBlindingEngine();
            RsaBlindingParameters revParameters =
                new RsaBlindingParameters(
                    new RsaKeyParameters(false, 
                        parameters.PublicKey.Modulus,
                        parameters.PublicKey.Modulus.Subtract(parameters.PublicKey.Exponent)), 
                    blindFactor);
            unblindingEngine.Init(true, revParameters);
            byte[] s = unblindingEngine.ProcessBlock(blindedData, 0, blindedData.Length);

            string pp = Encoding.ASCII.GetString(s);
            //signature verification
            //if (!AreEqual(s, sig))
            //{
               // Fail("test " + id + " failed generation");
            //}

            //verify signature with PssSigner
            //signer.Init(false, pub);
            //signer.BlockUpdate(msg, 0, msg.Length);

            //if (!signer.VerifySignature(s)) 
            //{
            //    string h = "";
            //}
            //{
           //     Fail("test " + id + " failed PssSigner verification");
           // }       */
        }

        static void Main(string[] args)
        {

            /*AsymmetricCipherKeyPair bob_keyPair = generateKeys(1024);
            AsymmetricCipherKeyPair alice_keyPair = generateKeys(1024);

            try
            {
                byte[] msg = Encoding.ASCII.GetBytes("OK");

                //----------- Bob: Generating blinding factor based on Alice's public key -----------//
                BigInteger blindingFactor = generateBlindingFactor(alice_keyPair.Public);

                //----------------- Bob: Blinding message with Alice's public key -----------------//
                byte[] blinded_msg =
                        blind(alice_keyPair.Public, blindingFactor, msg);

                byte[] unblinded_msg =
                        unblind(alice_keyPair.Public, blindingFactor, blinded_msg);

                //------------- Bob: Signing blinded message with Bob's private key -------------//
                byte[] sig = sign(bob_keyPair.Private, blinded_msg);

                //------------- Alice: Verifying Bob's signature -------------//
                if (verify(bob_keyPair.Public, blinded_msg, sig))
                {

                    //---------- Alice: Signing blinded message with Alice's private key ----------//
                    byte[] sigByAlice =
                            signBlinded(alice_keyPair.Private, blinded_msg);

                    //------------------- Bob: Unblinding Alice's signature -------------------//
                    byte[] unblindedSigByAlice =
                            unblind(alice_keyPair.Public, blindingFactor, sigByAlice);

                    //---------------- Bob: Verifying Alice's unblinded signature ----------------//
                    Console.WriteLine(verify(alice_keyPair.Public, msg,
                            unblindedSigByAlice));
                    // Now Bob has Alice's signature for the original message
                    //Console.WriteLine(Encoding.ASCII.GetString(unblindedSigByAlice));
                }



                Console.WriteLine(Encoding.ASCII.GetString(unblinded_msg));
            }
            catch (Exception e)
            {

            }

            Console.ReadLine();*/


            //byte[] msg = Encoding.ASCII.GetBytes("I feel great");
            //testSig(1, pub1, prv1, msg);

        }
    }

}
