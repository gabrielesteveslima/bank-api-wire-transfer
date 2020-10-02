using System;
using Microsoft.AspNetCore.Mvc;

namespace Wire.Transfer.In.API.Features.v1.WireTransfers
{
    /// <summary>
    ///     Represent possible filter on controller <see cref="WireTransfersController" />
    /// </summary>
    public class TransactionFilter
    {
        [FromQuery(Name = "filter[transactionId]")]
        public Guid TransactionId { get; set; }
    }
}