using System;
using System.Threading.Tasks;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer
{
    /// <summary>
    ///     A wire transfer
    /// </summary>
    public class WireTransfer : Entity, IAggregateRoot
    {
        public WireTransfer(WireTransferId id, WireTransferType wireTransferType, ProtocolTrackable protocolTrackable,
            BankAccount senderBankAccount, BankAccount beneficiaryBankAccount, MoneyValue amount,
            DateTime tradeDate, TransactionId transactionId)
        {
            Id = id;
            TransactionId = transactionId;
            ProtocolTrackable = protocolTrackable;
            SenderBankAccount = senderBankAccount;
            BeneficiaryBankAccount = beneficiaryBankAccount;
            WireTransferType = wireTransferType;
            Amount = amount;
            TradeDate = tradeDate;
            CreatedBy = "SampleBankWireTransferApi";
        }

        public WireTransferId Id { get; }
        public WireTransferType WireTransferType { get; }
        public ProtocolTrackable ProtocolTrackable { get; }
        public BankAccount SenderBankAccount { get; }
        public MoneyValue Amount { get; }
        public DateTime TradeDate { get; }
        public TransactionId TransactionId { get; private set; }
        public BankAccount BeneficiaryBankAccount { get; private set; }
        public string CreatedBy { get; }

        public static async Task<WireTransfer> AddAmountTransferOnBeneficiaryBalanceAsync(WireTransfer wireTransfer,
            IBeneficiaryAccountBalance beneficiaryAccountBalance, IAccountUniquenessChecker accountUniquenessChecker)
        {
            wireTransfer.BeneficiaryBankAccount = await BankAccount.BeneficiaryUniquenessChecker(
                wireTransfer.BeneficiaryBankAccount,
                accountUniquenessChecker);

            wireTransfer.TransactionId =
                new TransactionId(await beneficiaryAccountBalance.AddAmountToBeneficiaryBalanceAsync(wireTransfer));

            return wireTransfer;
        }
    }
}