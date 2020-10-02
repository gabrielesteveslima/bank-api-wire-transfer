using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.MakeTransaction
{
    /// <summary>
    ///     Response from transaction api
    /// </summary>
    [DataContract]
    public class TransactionIdResponse
    {
        [JsonProperty("TransactionUuid")] public Guid TransactionId { get; set; }
    }
}