namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.MakeTransaction
{
    /// <summary>
    ///     Represent class configuration for transaction api
    /// </summary>
    public class TransactionConfiguration
    {
        public string Url { get; set; }
        public string CreateTransactionPathSegment { get; set; }
        public string GetDetailsTransactionPathSegment { get; set; }
    }
}