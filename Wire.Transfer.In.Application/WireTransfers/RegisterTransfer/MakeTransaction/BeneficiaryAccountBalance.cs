using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Wire.Transfer.In.Application.Configuration.Validation;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.MakeTransaction
{
    /// <summary>
    ///     Make transaction wire transfer to balance beneficiary account
    /// </summary>
    public class BeneficiaryAccountBalance : IBeneficiaryAccountBalance
    {
        private const string WireTransferBrokerId = "3";
        private const int WireTransferCategoryId = 15;
        private const string WireTransferTypeOperation = "Credito";
        private const string WireTransferTypeTransaction = "JD";
        private readonly ILogging _logging;

        private readonly TransactionConfiguration _transactionApiConfiguration;

        public BeneficiaryAccountBalance(TransactionConfiguration transactionApiConfiguration, ILogging logging)
        {
            _transactionApiConfiguration = transactionApiConfiguration ??
                                           throw new ArgumentException(nameof(transactionApiConfiguration));
            _logging = logging ?? throw new ArgumentException(nameof(logging));
        }

        public async Task<Guid> AddAmountToBeneficiaryBalanceAsync(WireTransfer wireTransfer)
        {
            var transactionId = await _transactionApiConfiguration.Url
                .AppendPathSegment(_transactionApiConfiguration.CreateTransactionPathSegment)
                .ConfigureRequest(settings =>
                {
                    settings.BeforeCall = call =>
                    {
                        _logging.Information(call.Request.RequestUri);

                        var transactionIdResponse = GetTransactionId(wireTransfer);

                        if (transactionIdResponse != null)
                            throw new DuplicateException(
                                "Cannot create another transaction for the same wire transfer",
                                DuplicateRuleValidationEnumeration.ERROR_WIRE_TRANSFER_TRANSACTION_DUPLICATED);
                    };
                })
                .PostJsonAsync(new
                {
                    IdContaBancaria = wireTransfer.BeneficiaryBankAccount.BankAccountLegacyId.Value,
                    TipoOperacao = WireTransferTypeOperation,
                    TipoLancamento = WireTransferTypeTransaction,
                    Descricao = $"Recebeu de {wireTransfer.SenderBankAccount.BankAccountHolder.HolderName}",
                    Valor = wireTransfer.Amount.Value,
                    IdCategoria = WireTransferCategoryId,
                    DataTransacao = DateTime.Now,
                    UniqueId = wireTransfer.ProtocolTrackable.ProtocolNumber
                }).ReceiveJson<TransactionIdResponse>();

            return transactionId.TransactionId;
        }

        private TransactionIdResponse GetTransactionId(WireTransfer wireTransfer)
        {
            return _transactionApiConfiguration.Url
                .AppendPathSegment(_transactionApiConfiguration.GetDetailsTransactionPathSegment)
                .SetQueryParam("bankAccountId", wireTransfer.BeneficiaryBankAccount.BankAccountLegacyId.Value)
                .SetQueryParam("brokerId", WireTransferBrokerId)
                .SetQueryParam("uniqueId", wireTransfer.ProtocolTrackable.ProtocolNumber)
                .GetJsonAsync<TransactionIdResponse>().GetAwaiter().GetResult();
        }
    }
}