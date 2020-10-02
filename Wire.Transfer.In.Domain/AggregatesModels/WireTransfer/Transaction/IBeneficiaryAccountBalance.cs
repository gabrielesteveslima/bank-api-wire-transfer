using System;
using System.Threading.Tasks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction
{
    public interface IBeneficiaryAccountBalance
    {
        Task<Guid> AddAmountToBeneficiaryBalanceAsync(WireTransfer wireTransfer);
    }
}