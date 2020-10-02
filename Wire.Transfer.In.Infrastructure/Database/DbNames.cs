namespace Wire.Transfer.In.Infrastructure.Database
{
    public static class DbNames
    {
        internal const string WireTransfersIn = "WIRE_TRANSFER.dbo.WIRE_TRANSFER_IN";
        internal const string WireTransfersInRejected = "WIRE_TRANSFER.dbo.WIRE_TRANSFER_IN_REJECTED";

        internal const string WireTransfersInCqrsCollectionName = "WIRE_TRANSFER_IN_PROOF";
    }
}