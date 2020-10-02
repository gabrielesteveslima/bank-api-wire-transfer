using System;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction
{
    /// <summary>
    ///     A transaction id
    /// </summary>
    public class TransactionId : TypedIdValueBase
    {
        public TransactionId(Guid value) : base(value)
        {
        }
    }
}