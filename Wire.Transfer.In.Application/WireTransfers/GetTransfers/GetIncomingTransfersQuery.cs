using System.Collections.Generic;

namespace Wire.Transfer.In.Application.WireTransfers.GetTransfers
{
    public class GetIncomingTransfersQuery : IQuery<IEnumerable<WireTransferDto>>
    {
        public GetIncomingTransfersQuery(WireTransferDto request)
        {
            Request = request;
        }

        public WireTransferDto Request { get; }
    }
}