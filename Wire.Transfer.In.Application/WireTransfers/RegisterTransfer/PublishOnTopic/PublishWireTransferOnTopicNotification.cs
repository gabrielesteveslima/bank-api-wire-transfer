using MediatR;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.PublishOnTopic
{
    public class PublishWireTransferOnTopicNotification : INotification
    {
        public PublishWireTransferOnTopicNotification(WireTransferDto wireTransfer)
        {
            WireTransfer = wireTransfer;
        }

        public WireTransferDto WireTransfer { get; }
    }
}