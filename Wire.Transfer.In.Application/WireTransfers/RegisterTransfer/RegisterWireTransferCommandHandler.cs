using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wire.Transfer.In.Application.Configuration;
using Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.PublishOnTopic;
using Wire.Transfer.In.Application.WireTransfers.RegisterTransferRejected;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer
{
    public class RegisterWireTransferCommandHandler : ICommandHandler<RegisterWireTransferCommand, WireTransferDto>
    {
        private readonly IAccountUniquenessChecker _accountUniquenessChecker;
        private readonly IBeneficiaryAccountBalance _beneficiaryAccountBalance;
        private readonly IMediator _mediator;
        private readonly IWireTransferRepository _repository;

        public RegisterWireTransferCommandHandler(IWireTransferRepository repository,
            IAccountUniquenessChecker accountUniquenessChecker,
            IBeneficiaryAccountBalance beneficiaryAccountBalance, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
            _accountUniquenessChecker = accountUniquenessChecker;
            _beneficiaryAccountBalance = beneficiaryAccountBalance;
        }

        public async Task<WireTransferDto> Handle(RegisterWireTransferCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var wireTransferRegistered =
                    await WireTransfer.AddAmountTransferOnBeneficiaryBalanceAsync(
                        command.Request.WrapToWireTransfer(),
                        _beneficiaryAccountBalance, _accountUniquenessChecker);

                var result = command.Request.SeedRelationships(wireTransferRegistered);

                await _repository.RegisterIncomingTransfer(wireTransferRegistered);
                await _mediator.Publish(new PublishWireTransferOnTopicNotification(result),
                    cancellationToken);

                return result;
            }
            catch (BusinessRuleValidationException exception)
            {
                await _mediator.Publish(
                    new RegisterRejectedTransferNotification(command.Request.WrapToWireTransfer(), exception),
                    cancellationToken);
                throw;
            }
        }
    }
}