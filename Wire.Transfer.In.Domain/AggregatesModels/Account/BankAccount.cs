using System;
using System.Threading.Tasks;
using Wire.Transfer.In.Domain.AggregatesModels.Account.Holders;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    /// <summary>
    ///     A bank account
    /// </summary>
    public class BankAccount : Entity, IAggregateRoot
    {
        public BankAccount(BankAccountId bankAccountId, BankAccountLegacyId bankAccountLegacyId,
            BankAccountHolder bankAccountHolder, BankAccountDetails bankAccountDetails, bool blocked,
            bool canceled) : this()
        {
            BankAccountId = bankAccountId;
            BankAccountLegacyId = bankAccountLegacyId;
            BankAccountHolder = bankAccountHolder;
            BankAccountDetails = bankAccountDetails;
            Blocked = blocked;
            Canceled = canceled;
        }

        public BankAccount()
        {
        }

        public BankAccountId BankAccountId { get; }
        [Obsolete] public BankAccountLegacyId BankAccountLegacyId { get; }
        public BankAccountHolder BankAccountHolder { get; }
        public BankAccountDetails BankAccountDetails { get; }
        public bool Blocked { get; }
        public bool Canceled { get; }

        public static async Task<BankAccount> BeneficiaryUniquenessChecker(
            BankAccount beneficiaryAccount,
            IAccountUniquenessChecker accountUniquenessChecker)
        {
            var account =
                await accountUniquenessChecker.AccountUniquenessChecker(beneficiaryAccount.BankAccountHolder,
                    beneficiaryAccount.BankAccountDetails);

            if (account.BankAccountId is null)
                throw new BusinessRuleValidationException(
                    "The beneficiary's bank account was not found",
                    BankAccountStatusType.BankAccountNotFound,
                    BusinessRuleValidationEnumeration.ERROR_BENEFICIARY_ACCOUNT_NOT_FOUND);

            if (account.Canceled)
                throw new BusinessRuleValidationException(
                    "The beneficiary's bank account is canceled",
                    BankAccountStatusType.Canceled,
                    BusinessRuleValidationEnumeration.ERROR_BENEFICIARY_ACCOUNT_IS_CANCELED);

            return account;
        }
    }
}