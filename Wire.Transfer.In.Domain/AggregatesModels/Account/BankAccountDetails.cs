using System;
using System.Collections.Generic;
using System.Linq;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    /// <summary>
    ///     A details from <see cref="BankAccount" />
    /// </summary>
    public class BankAccountDetails : ValueObject
    {
        public BankAccountDetails(string account, string branch, string number)
        {
            Account = account;
            Branch = branch;
            Number = number;
        }

        public string Number { get; }
        public string Branch { get; }
        public string Account { get; }

        public string Digit => GetAccountDigits().Reverse().Last().ToString();

        public string GetAccountWithoutDigit()
        {
            return string.Join(string.Empty, GetAccountDigits().Reverse().SkipLast(1).Select(x => x));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return Branch;
            yield return Account;
        }

        private IEnumerable<long> GetAccountDigits()
        {
            var accountDigits = Convert.ToInt64(Account);

            while (accountDigits > 0)
            {
                var digit = accountDigits % 10;
                accountDigits /= 10;
                yield return digit;
            }
        }
    }
}