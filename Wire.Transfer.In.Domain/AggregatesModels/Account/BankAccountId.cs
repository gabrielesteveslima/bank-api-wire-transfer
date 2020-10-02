using System;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    /// <summary>
    ///     A bank account id
    /// </summary>
    public class BankAccountId : TypedIdValueBase
    {
        public BankAccountId(Guid value) : base(value)
        {
        }
    }
}