using System;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer
{
    /// <summary>
    ///     Wire transfer id
    /// </summary>
    public class WireTransferId : TypedIdValueBase
    {
        public WireTransferId(Guid value) : base(value)
        {
        }
    }
}