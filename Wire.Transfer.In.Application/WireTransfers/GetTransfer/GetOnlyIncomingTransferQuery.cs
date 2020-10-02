namespace Wire.Transfer.In.Application.WireTransfers.GetTransfer
{
    public class GetOnlyIncomingTransferQuery : IQuery<WireTransferDto>
    {
        public GetOnlyIncomingTransferQuery(WireTransferDto request)
        {
            Request = request;
        }

        public WireTransferDto Request { get; }
    }
}