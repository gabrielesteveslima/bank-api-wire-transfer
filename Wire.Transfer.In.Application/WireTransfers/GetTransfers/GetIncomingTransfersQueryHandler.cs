using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wire.Transfer.In.Application.Configuration;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;

namespace Wire.Transfer.In.Application.WireTransfers.GetTransfers
{
    public class
        GetIncomingTransfersQueryHandler : IQueryHandler<GetIncomingTransfersQuery, IEnumerable<WireTransferDto>>
    {
        private readonly IWireTransferRepository _wireTransferRepository;

        public GetIncomingTransfersQueryHandler(IWireTransferRepository wireTransferRepository)
        {
            _wireTransferRepository = wireTransferRepository;
        }

        public async Task<IEnumerable<WireTransferDto>> Handle(GetIncomingTransfersQuery query,
            CancellationToken cancellationToken)
        {
            var incomingTransfers =
                await _wireTransferRepository.GetIncomingTransfers(new BankAccountId(query.Request.Account.Id),
                    new TransactionId(query.Request.Transaction.Id));
            return incomingTransfers.Select(x => x.WrapToWireTransferDto());
        }
    }
}