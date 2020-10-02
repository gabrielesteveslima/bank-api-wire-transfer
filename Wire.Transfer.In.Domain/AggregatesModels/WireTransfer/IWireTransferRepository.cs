using System.Collections.Generic;
using System.Threading.Tasks;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer
{
    public interface IWireTransferRepository
    {
        Task<IEnumerable<WireTransfer>> GetIncomingTransfers(BankAccountId beneficiaryBankAccountId,
            TransactionId transactionId);

        Task<WireTransfer> GetIncomingTransferById(BankAccountId beneficiaryBankAccountId,
            WireTransferId wireTransferId);

        Task RegisterIncomingTransfer(WireTransfer wireTransfer);
        Task RegisterIncomingTransferRejected(WireTransfer wireTransfer, BusinessRuleValidationException exception);
    }
}