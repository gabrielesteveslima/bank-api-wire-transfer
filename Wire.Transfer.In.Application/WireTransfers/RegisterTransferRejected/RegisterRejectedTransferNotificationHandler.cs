using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransferRejected
{
    public class
        RegisterRejectedTransferNotificationHandler : INotificationHandler<RegisterRejectedTransferNotification>
    {
        private readonly IWireTransferRepository _repository;

        public RegisterRejectedTransferNotificationHandler(IWireTransferRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(RegisterRejectedTransferNotification notification, CancellationToken cancellationToken)
        {
            await _repository.RegisterIncomingTransferRejected(notification.WireTransfer,
                notification.BusinessRuleValidationException);
        }
    }
}