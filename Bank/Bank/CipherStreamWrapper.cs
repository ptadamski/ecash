using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AsymmetricCipherStream : IAsymmetricBlockCipher  
    {
        public AsymmetricCipherStream(IAsymmetricBlockCipher cipher)
        {

        }

        private IAsymmetricBlockCipher cipher;


        public string AlgorithmName
        {
            get { return cipher.AlgorithmName; }
        }

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            cipher.Init(forEncryption, parameters);
        }

        public int GetInputBlockSize()
        {
            return cipher.GetInputBlockSize();
        }

        public int GetOutputBlockSize()
        {
            return cipher.GetOutputBlockSize();
        }

        public byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
        {
            if(inLen + inOff > inBuf.Length)
                throw new CryptoException("block too small");

            MemoryStream stream = new MemoryStream();
                  
            int blockLength = 0;
            int left = inLen;

            for (int position = inOff; position < inLen + inOff; position += blockLength, left -= blockLength)
			{
                blockLength = GetInputBlockSize() < left ? GetInputBlockSize() : left;   
                byte[] block = cipher.ProcessBlock(inBuf, position + inOff, blockLength);
                stream.Write(block, 0, block.Length);
			}
            return stream.ToArray();
        }
    }
}
