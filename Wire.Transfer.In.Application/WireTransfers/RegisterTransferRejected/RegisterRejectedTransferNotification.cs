using MediatR;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransferRejected
{
    public class RegisterRejectedTransferNotification : INotification
    {
        public RegisterRejectedTransferNotification(WireTransfer wireTransfer,
            BusinessRuleValidationException businessRuleValidationException)
        {
            WireTransfer = wireTransfer;
            BusinessRuleValidationException = businessRuleValidationException;
        }

        public WireTransfer WireTransfer { get; }
        public BusinessRuleValidationException BusinessRuleValidationException { get; }
    }
}