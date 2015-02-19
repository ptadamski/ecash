using Bank.Data;
using Bank.Model;
using System;
using System.Collections.Generic;

namespace Bank.Interface
{
    public interface IBlindAgreementFactory
    {
        BlindAgreement Construct(Banknote aBanknote, IList<byte[]> aBlindedContent, int aIgnored);
        void Destruct(Guid aItem);
    }

}
