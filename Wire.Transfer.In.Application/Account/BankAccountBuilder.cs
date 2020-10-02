using System;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.Account.Holders;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.Account
{
    /// <summary>
    ///     Helper to build any bank account
    /// </summary>
    public class BankAccountBuilder : Builder<BankAccount>
    {
        public BankAccountId BankAccountId { get; private set; }
        public BankAccountLegacyId BankAccountLegacyId { get; private set; }
        public BankAccountHolder BankAccountHolder { get; private set; }
        public BankAccountDetails BankAccountDetails { get; private set; }
        public bool Blocked { get; private set; }
        public bool Canceled { get; private set; }

        public BankAccountBuilder WithBankAccountId(Guid bankAccountId)
        {
            BankAccountId = new BankAccountId(bankAccountId);
            return this;
        }

        public BankAccountBuilder WithBankAccountLegacyId(int bankAccountLegacyId)
        {
            BankAccountLegacyId = new BankAccountLegacyId(bankAccountLegacyId);
            return this;
        }

        public BankAccountBuilder WithBankAccountHolder(string holderName,
            string holderDocument)
        {
            BankAccountHolder = new BankAccountHolder(holderName, holderDocument);
            return this;
        }

        public BankAccountBuilder WithBankAccountDetails(string account, string branch, string number)
        {
            BankAccountDetails = new BankAccountDetails(account, branch, number);
            return this;
        }

        public BankAccountBuilder IsBlocked(bool blocked)
        {
            Blocked = blocked;
            return this;
        }

        public BankAccountBuilder IsCanceled(bool canceled)
        {
            Canceled = canceled;
            return this;
        }

        public override BankAccount Build()
        {
            return new BankAccount(BankAccountId, BankAccountLegacyId, BankAccountHolder,
                BankAccountDetails, Blocked, Canceled);
        }
    }
}