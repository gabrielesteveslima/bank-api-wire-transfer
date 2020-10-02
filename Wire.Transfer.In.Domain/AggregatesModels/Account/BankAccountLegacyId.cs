using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    /// <summary>
    ///     Bank account legacy id <see cref="BankAccount" />
    /// </summary>
    public class BankAccountLegacyId : TypedIdValueBase
    {
        public BankAccountLegacyId(int value) : base(value)
        {
        }
    }
}