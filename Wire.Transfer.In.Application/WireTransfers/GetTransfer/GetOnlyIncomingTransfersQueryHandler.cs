using System;
using System.Threading;
using System.Threading.Tasks;
using Wire.Transfer.In.Application.Configuration;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;

namespace Wire.Transfer.In.Application.WireTransfers.GetTransfer
{
    public class GetOnlyIncomingTransfersQueryHandler :
        IQueryHandler<GetOnlyIncomingTransferQuery, WireTransferDto>
    {
        private readonly IWireTransferRepository _wireTransferRepository;

        public GetOnlyIncomingTransfersQueryHandler(IWireTransferRepository wireTransferRepository)
        {
            _wireTransferRepository =
                wireTransferRepository ?? throw new ArgumentException(nameof(wireTransferRepository));
        }

        public async Task<WireTransferDto> Handle(GetOnlyIncomingTransferQuery query,
            CancellationToken cancellationToken)
        {
            var incomingTransfer =
                await _wireTransferRepository.GetIncomingTransferById(
                    new BankAccountId(query.Request.Account.Id),
                    new WireTransferId(query.Request.Id));

            return incomingTransfer.WrapToWireTransferDto();
        }
    }
}