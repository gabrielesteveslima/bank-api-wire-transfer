using System;
using NMoneys;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers
{
    public class WireTransferBuilder : Builder<WireTransfer>
    {
        public WireTransferId Id { get; private set; }
        public WireTransferType WireTransferType { get; private set; }
        public ProtocolTrackable ProtocolTrackable { get; private set; }
        public BankAccount SenderBankAccount { get; private set; }
        public BankAccount BeneficiaryBankAccount { get; private set; }
        public MoneyValue Amount { get; private set; }
        public DateTime TradeDate { get; private set; }
        public TransactionId TransactionId { get; set; }

        public WireTransferBuilder WithId(Guid id)
        {
            Id = new WireTransferId(id);
            return this;
        }

        public WireTransferBuilder WithType(string type)
        {
            WireTransferType = (WireTransferType) Enum.Parse(typeof(WireTransferType), type);
            return this;
        }

        public WireTransferBuilder WithProtocol(string statusCode, string statusInformation, string protocolNumber)
        {
            ProtocolTrackable = new ProtocolTrackable(statusCode, statusInformation, protocolNumber);
            return this;
        }

        public WireTransferBuilder WithSenderBankAccount(BankAccount senderBankAccountBuilder)
        {
            SenderBankAccount = senderBankAccountBuilder;
            return this;
        }

        public WireTransferBuilder WithBeneficiaryBankAccount(BankAccount beneficiaryBankAccountBuilder)
        {
            BeneficiaryBankAccount = beneficiaryBankAccountBuilder;
            return this;
        }

        public WireTransferBuilder WithAmount(decimal amount, Currency currency)
        {
            Amount = new MoneyValue(amount, currency);
            return this;
        }

        public WireTransferBuilder WithTradeDate(DateTime tradeDate)
        {
            TradeDate = tradeDate;
            return this;
        }

        public WireTransferBuilder WithTransactionId(Guid transactionId)
        {
            TransactionId = new TransactionId(transactionId);
            return this;
        }

        public override WireTransfer Build()
        {
            return new WireTransfer(Id, WireTransferType, ProtocolTrackable,
                SenderBankAccount, BeneficiaryBankAccount, Amount, TradeDate, TransactionId);
        }
    }
}