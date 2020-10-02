using System.Collections.Generic;
using NMoneys;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer
{
    /// <summary>
    ///     A amount <see cref="WireTransfer" />
    /// </summary>
    public class MoneyValue : ValueObject
    {
        public MoneyValue(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        public decimal Value { get; }

        public Currency Currency { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Currency;
        }
    }
}