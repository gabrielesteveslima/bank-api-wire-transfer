using System;
using NMoneys;
using Wire.Transfer.In.Application.Account;
using Wire.Transfer.In.Application.Configuration.Data;
using Wire.Transfer.In.Application.WireTransfers;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;

namespace Wire.Transfer.In.Application.Configuration
{
    /// <summary>
    ///     Helper converter <see cref="WireTransferDto" /> to <see cref="WireTransfer" /> vice versa
    /// </summary>
    public static class WireTransferWrap
    {
        public static WireTransfer WrapToWireTransfer(this WireTransferDto wireTransferDto)
        {
            var wireTransfer = new WireTransferBuilder()
                .WithId(wireTransferDto.Id)
                .WithType(wireTransferDto.WireTransferType)
                .WithProtocol(wireTransferDto.Protocol.Code,
                    wireTransferDto.Protocol.Status,
                    wireTransferDto.Protocol.Number)
                .WithSenderBankAccount(new BankAccountBuilder()
                    .WithBankAccountHolder(wireTransferDto.Sender.Name,
                        wireTransferDto.Sender.Document.Number)
                    .WithBankAccountDetails(wireTransferDto.Sender.Account,
                        wireTransferDto.Sender.Branch,
                        wireTransferDto.Sender.Number)
                    .Build())
                .WithBeneficiaryBankAccount(new BankAccountBuilder()
                    .WithBankAccountHolder(wireTransferDto.Beneficiary.Name,
                        wireTransferDto.Beneficiary.Document.Number)
                    .WithBankAccountDetails(wireTransferDto.Beneficiary.Account,
                        wireTransferDto.Beneficiary.Branch,
                        wireTransferDto.Beneficiary.Number)
                    .Build())
                .WithAmount(wireTransferDto.Amount, Currency.Get("brl"))
                .WithTradeDate(wireTransferDto.TradeDate)
                .Build();

            return wireTransfer;
        }

        public static WireTransfer WrapToWireTransfer(this WireTransferMongoDocument wireTransferMongoDocument)
        {
            var wireTransfer = new WireTransferBuilder()
                .WithId(Guid.Parse(wireTransferMongoDocument.WireTransferId))
                .WithType(wireTransferMongoDocument.WireTransferType.ToString())
                .WithProtocol(wireTransferMongoDocument.ProtocolCode,
                    wireTransferMongoDocument.ProtocolStatus,
                    wireTransferMongoDocument.ProtocolNumber.ToString())
                .WithSenderBankAccount(new BankAccountBuilder()
                    .WithBankAccountHolder(wireTransferMongoDocument.SenderName,
                        wireTransferMongoDocument.SenderDocument)
                    .WithBankAccountDetails(wireTransferMongoDocument.SenderAccountNumber,
                        wireTransferMongoDocument.SenderBranchNumber,
                        wireTransferMongoDocument.SenderBankNumber)
                    .Build())
                .WithBeneficiaryBankAccount(new BankAccountBuilder()
                    .WithBankAccountId(Guid.Parse(wireTransferMongoDocument.BeneficiaryAccountId))
                    .Build())
                .WithAmount(wireTransferMongoDocument.Amount, Currency.Get("brl"))
                .WithTradeDate(wireTransferMongoDocument.TradeDate)
                .WithTransactionId(Guid.Parse(wireTransferMongoDocument.TransactionId))
                .Build();

            return wireTransfer;
        }

        public static WireTransferDto WrapToWireTransferDto(this WireTransfer wireTransfer)
        {
            return new WireTransferDto
            {
                Id = Guid.Parse(wireTransfer.Id.Value.ToString()),
                Sender = new SenderDto
                {
                    Name = wireTransfer.SenderBankAccount.BankAccountHolder.HolderName,
                    Account = wireTransfer.SenderBankAccount.BankAccountDetails.Account,
                    Branch = wireTransfer.SenderBankAccount.BankAccountDetails.Branch,
                    Number = wireTransfer.SenderBankAccount.BankAccountDetails.Number,
                    Document = new DocumentDto
                    {
                        Number = wireTransfer.SenderBankAccount.BankAccountHolder.HolderDocument.Value,
                        Type = wireTransfer.SenderBankAccount.BankAccountHolder.HolderDocument.DocumentType.ToString()
                    }
                },
                Amount = wireTransfer.Amount.Value,
                Protocol = new ProtocolDto
                {
                    Code = wireTransfer.ProtocolTrackable.StatusCode,
                    Status = wireTransfer.ProtocolTrackable.StatusInformation,
                    Number = wireTransfer.ProtocolTrackable.ProtocolNumber
                },
                TradeDate = wireTransfer.TradeDate,
                WireTransferType = ((int) wireTransfer.WireTransferType).ToString(),
                Transaction = new TransactionDto
                {
                    Id = Guid.Parse(wireTransfer.TransactionId.Value.ToString())
                },
                Account = new AccountDto
                {
                    Id = Guid.Parse(wireTransfer.BeneficiaryBankAccount.BankAccountId.Value.ToString())
                },
                Beneficiary = null
            };
        }

        /// <summary>
        ///     Populate <see cref="WireTransferDto" /> with transaction uuid and remove beneficiary details from result
        /// </summary>
        /// <param name="wireTransferDto"></param>
        /// <param name="wireTransfer"></param>
        /// <returns></returns>
        public static WireTransferDto SeedRelationships(this WireTransferDto wireTransferDto,
            WireTransfer wireTransfer)
        {
            wireTransferDto.Transaction = new TransactionDto
            {
                Id = Guid.Parse(wireTransfer.TransactionId.Value.ToString())
            };

            wireTransferDto.Account = new AccountDto
            {
                Id = Guid.Parse(wireTransfer.BeneficiaryBankAccount.BankAccountId.Value.ToString())
            };

            wireTransferDto.Beneficiary = null;

            return wireTransferDto;
        }
    }
}