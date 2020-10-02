using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;
using Wire.Transfer.In.Application.WireTransfers;
using Wire.Transfer.In.Application.WireTransfers.GetTransfer;
using Wire.Transfer.In.Application.WireTransfers.GetTransfers;
using Wire.Transfer.In.Application.WireTransfers.RegisterTransfer;
using Wire.Transfer.In.Infrastructure.Logs;

namespace Wire.Transfer.In.API.Features.v1.WireTransfers
{
    [ApiController]
    [ServiceFilter(typeof(ProblemDetailsFilter))]
    [ApiVersion("1")]
    [Route("wire-transfers/v{version:apiVersion}")]
    [Produces("application/json")]
    public class WireTransfersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WireTransfersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string RequestHost => Request.Host.ToString();

        [HttpGet("checking-accounts/{beneficiaryAccountId}/wire-transfers-in/{wireTransferId}")]
        public async Task<IActionResult> GetIncomingTransfer(Guid beneficiaryAccountId,
            Guid wireTransferId)
        {
            Log.LogInformation($"Beneficiary account: {wireTransferId}, Wire Transfer Id: {wireTransferId}");

            var incomingTransferQueryResult =
                await _mediator.Send(new GetOnlyIncomingTransferQuery(new WireTransferDto
                {
                    Id = wireTransferId,
                    Account = new AccountDto
                    {
                        Id = beneficiaryAccountId
                    }
                }));

            incomingTransferQueryResult.AddSelfLik(RequestHost);
            return Ok(incomingTransferQueryResult);
        }

        [HttpGet("checking-accounts/{beneficiaryAccountId}/wire-transfers-in")]
        public async Task<IActionResult> GetIncomingTransfers(Guid beneficiaryAccountId,
            [FromQuery] TransactionFilter transactionFilter)
        {
            Log.LogInformation(
                $"Beneficiary Account: {beneficiaryAccountId}, TransactionFilter {transactionFilter.TransactionId}");

            var incomingTransfersQueryResult =
                await _mediator.Send(new GetIncomingTransfersQuery(new WireTransferDto
                {
                    Transaction = new TransactionDto
                    {
                        Id = transactionFilter.TransactionId
                    },
                    Account = new AccountDto
                    {
                        Id = beneficiaryAccountId
                    }
                }));

            var incomingTransfers = incomingTransfersQueryResult.ToList()
                .Select(wireTransferDto =>
                {
                    wireTransferDto.AddSelfLik(RequestHost);
                    return wireTransferDto;
                });

            return Ok(incomingTransfers);
        }

        [HttpPost("register-incoming-transfer")]
        [ProducesResponseType(typeof(WireTransferDto), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterIncomingTransfer(
            [FromBody] WireTransferDto registerWireTransferRequest)

        {
            Log.LogInformation(registerWireTransferRequest);

            var registerIncomingTransferCommandResult =
                await _mediator.Send(new RegisterWireTransferCommand(registerWireTransferRequest));

            registerIncomingTransferCommandResult.AddSelfLik(RequestHost);
            return Created(string.Empty, registerIncomingTransferCommandResult);
        }
    }
}