using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MongoDB.Driver;
using Wire.Transfer.In.Application.Configuration;
using Wire.Transfer.In.Application.Configuration.Data;
using Wire.Transfer.In.Application.Configuration.Validation;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;
using Wire.Transfer.In.Infrastructure.Database;
using Wire.Transfer.In.Infrastructure.Resilience;

namespace Wire.Transfer.In.Infrastructure.Domain.WireTransfers
{
    public class WireTransferRepository : IWireTransferRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public WireTransferRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory =
                connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<WireTransfer>> GetIncomingTransfers(
            BankAccountId beneficiaryBankAccountId,
            TransactionId transactionId)
        {
            var builder = Builders<WireTransferMongoDocument>.Filter;
            var filterByTransaction =
                builder.Eq(x => x.BeneficiaryAccountId, beneficiaryBankAccountId.Value.ToString());

            if (!transactionId.Value.Equals(Guid.Empty))
                filterByTransaction &= builder.Eq(x => x.TransactionId, transactionId.Value.ToString());

            var connection = _connectionFactory.GetCqrsConnection();
            var wireTransferCollection =
                connection.GetCollection<WireTransferMongoDocument>(DbNames.WireTransfersInCqrsCollectionName);

            var document = await wireTransferCollection
                .Find(filterByTransaction).ToListAsync();

            if (!document.Any())
                throw new NotFoundException("No wire transfers found for this beneficiary");

            return document.Select(x => x.WrapToWireTransfer());
        }

        public async Task<WireTransfer> GetIncomingTransferById(BankAccountId beneficiaryBankAccountId,
            WireTransferId wireTransferId)
        {
            var builder = Builders<WireTransferMongoDocument>.Filter;
            var filterByTransaction = builder.Where(x =>
                x.WireTransferId == wireTransferId.Value.ToString() &&
                x.BeneficiaryAccountId == beneficiaryBankAccountId.Value.ToString());

            var connection = _connectionFactory.GetCqrsConnection();
            var wireTransferCollection =
                connection.GetCollection<WireTransferMongoDocument>(DbNames.WireTransfersInCqrsCollectionName);

            var document = await wireTransferCollection
                .Find(filterByTransaction).FirstOrDefaultAsync();

            if (document is null)
                throw new NotFoundException("No wire transfers found for this code");

            return document.WrapToWireTransfer();
        }

        public async Task RegisterIncomingTransfer(WireTransfer wireTransfer)
        {
            var connection = _connectionFactory.GetOpenSqlConnection();

            var wireTransferRejectedExists = connection.QueryFirstOrDefault<bool>(
                $@"SELECT COUNT(1) FROM {DbNames.WireTransfersIn} WHERE [WIRE_TRANSFER_IN_UUID] = @WireTransferId",
                new {WireTransferId = wireTransfer.Id.Value});

            if (wireTransferRejectedExists)
                throw new DuplicateException("There is already a wire transfer with the same id",
                    DuplicateRuleValidationEnumeration.ERROR_WIRE_TRANSFER_DUPLICATED);

            var sqlInsert = @$"INSERT INTO {DbNames.WireTransfersIn} (
                                                         [WIRE_TRANSFER_IN_UUID],
                                                         [BACEN_ST],
                                                         [BACEN_RETURNE_CD],
                                                         [WIRE_TRANSFER_IN_PROTOCOL_NU],
                                                         [SENDER_CPF_CNPJ_NU], 
                                                         [WIRE_TRANSFER_IN_TP], 
                                                         [SENDER_NM], 
                                                         [SENDER_BANK_NU], 
                                                         [SENDER_BRANCH_NU], 
                                                         [SENDER_ACCOUNT_NU], 
                                                         [ACCOUNT_UUID],
                                                         [TRADE_DT], 
                                                         [WIRE_TRANSFER_IN_VL], 
                                                         [TRANSACTION_UUID], 
                                                         [CREATED_BY_DS]) 
                                              VALUES(
                                                     @Id, 
                                                     @BacenStatus, 
                                                     @BanceReturn, 
                                                     @ProtocolNumber,
                                                     @SenderDocument, 
                                                     @WireTransferType, 
                                                     @SenderName,
                                                     @SenderBankNumber, 
                                                     @SenderBankBranch, 
                                                     @SenderAccount,
                                                     @BeneficiaryAccountUuid,
                                                     @TradeDate,
                                                     @Amount,
                                                     @TransactionUuid,
                                                     @CreatedBy)";

            await connection.ExecuteAsyncWithRetry(sqlInsert, new
            {
                Id = wireTransfer.Id.Value,
                wireTransfer.WireTransferType,

                BacenStatus = wireTransfer.ProtocolTrackable.StatusCode,
                BanceReturn = wireTransfer.ProtocolTrackable.StatusInformation,
                wireTransfer.ProtocolTrackable.ProtocolNumber,

                SenderName = wireTransfer.SenderBankAccount.BankAccountHolder.HolderName,
                SenderDocument = wireTransfer.SenderBankAccount.BankAccountHolder.HolderDocument.Value,
                SenderBankNumber = wireTransfer.SenderBankAccount.BankAccountDetails.Number,
                SenderBankBranch = wireTransfer.SenderBankAccount.BankAccountDetails.Branch,
                SenderAccount = wireTransfer.SenderBankAccount.BankAccountDetails.Account,

                BeneficiaryAccountUuid = wireTransfer.BeneficiaryBankAccount.BankAccountId.Value,
                wireTransfer.TradeDate,
                Amount = wireTransfer.Amount.Value,
                TransactionUuid = wireTransfer.TransactionId.Value,
                wireTransfer.CreatedBy
            });
        }

        public async Task RegisterIncomingTransferRejected(WireTransfer wireTransfer,
            BusinessRuleValidationException exception)
        {
            var connection = _connectionFactory.GetOpenSqlConnection();

            var wireTransferRejectedExists = connection.QueryFirstOrDefault<bool>(
                $@"SELECT COUNT(1) FROM {DbNames.WireTransfersInRejected} WHERE [WIRE_TRANSFER_IN_UUID] = @WireTransferId",
                new {WireTransferId = wireTransfer.Id.Value});

            if (wireTransferRejectedExists)
                throw new DuplicateException("There is already a wire transfer rejected with the same id",
                    DuplicateRuleValidationEnumeration.ERROR_WIRE_TRANSFER_REJECTED_DUPLICATED);

            var sqlInsert = @$"INSERT INTO {DbNames.WireTransfersInRejected} (
                                                         [WIRE_TRANSFER_IN_UUID],
                                                         [WIRE_TRANSFER_IN_INTEGRATION_EVENT_STATUS_ID],
                                                         [BACEN_ST],
                                                         [BACEN_RETURNE_CD],
                                                         [WIRE_TRANSFER_IN_PROTOCOL_NU],
                                                         [SENDER_CPF_CNPJ_NU], 
                                                         [WIRE_TRANSFER_IN_TP], 
                                                         [SENDER_NM], 
                                                         [SENDER_BANK_NU], 
                                                         [SENDER_BRANCH_NU], 
                                                         [SENDER_ACCOUNT_NU], 
                                                         [ACCOUNT_UUID],
                                                         [TRADE_DT], 
                                                         [WIRE_TRANSFER_IN_VL], 
                                                         [WIRE_TRANSFER_IN_DS], 
                                                         [CREATED_BY_DS]) 
                                              VALUES(
                                                     @Id, 
                                                     @EventStatusId, 
                                                     @BacenStatus, 
                                                     @BanceReturn, 
                                                     @ProtocolNumber,
                                                     @SenderDocument, 
                                                     @WireTransferType, 
                                                     @SenderName,
                                                     @SenderBankNumber, 
                                                     @SenderBankBranch, 
                                                     @SenderAccount,
                                                     @BeneficiaryAccountUuid,
                                                     @TradeDate,
                                                     @Amount,
                                                     @RejectedDescription,
                                                     @CreatedBy)";

            await connection.ExecuteAsyncWithRetry(sqlInsert, new
            {
                Id = wireTransfer.Id.Value,
                wireTransfer.WireTransferType,
                EventStatusId = (int) exception.BankAccountStatusType,

                BacenStatus = wireTransfer.ProtocolTrackable.StatusCode,
                BanceReturn = wireTransfer.ProtocolTrackable.StatusInformation,
                wireTransfer.ProtocolTrackable.ProtocolNumber,

                SenderName = wireTransfer.SenderBankAccount.BankAccountHolder.HolderName,
                SenderDocument = wireTransfer.SenderBankAccount.BankAccountHolder.HolderDocument.Value,
                SenderBankNumber = wireTransfer.SenderBankAccount.BankAccountDetails.Number,
                SenderBankBranch = wireTransfer.SenderBankAccount.BankAccountDetails.Branch,
                SenderAccount = wireTransfer.SenderBankAccount.BankAccountDetails.Account,

                BeneficiaryAccountUuid = wireTransfer.BeneficiaryBankAccount.BankAccountId?.Value ?? Guid.Empty,
                wireTransfer.TradeDate,
                Amount = wireTransfer.Amount.Value,
                RejectedDescription = exception.Details,
                wireTransfer.CreatedBy
            });
        }
    }
}