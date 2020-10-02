using System.Threading.Tasks;
using Wire.Transfer.In.Domain.AggregatesModels.Account.Holders;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    public interface IAccountUniquenessChecker
    {
        Task<BankAccount> AccountUniquenessChecker(BankAccountHolder beneficiaryAccountHolder,
            BankAccountDetails beneficiaryBankAccountDetails);
    }
}