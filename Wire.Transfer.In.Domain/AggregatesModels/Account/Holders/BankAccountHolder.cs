using System.Collections.Generic;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account.Holders
{
    /// <summary>
    ///     Represent Bank account holder
    /// </summary>
    public class BankAccountHolder : ValueObject
    {
        public BankAccountHolder(string holderName, string holderDocument)
        {
            HolderName = holderName;
            HolderDocument = new Document(holderDocument);
        }

        public string HolderName { get; }
        public Document HolderDocument { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return HolderName;
            yield return HolderDocument;
        }
    }
}