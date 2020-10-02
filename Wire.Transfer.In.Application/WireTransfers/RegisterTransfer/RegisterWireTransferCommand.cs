namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer
{
    public class RegisterWireTransferCommand : CommandBase<WireTransferDto>
    {
        public RegisterWireTransferCommand(WireTransferDto request)
        {
            Request = request;
        }

        public WireTransferDto Request { get; }
    }
}