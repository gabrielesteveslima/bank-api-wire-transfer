using System;

namespace Wire.Transfer.In.Application.Account
{
    /// <summary>
    ///     Return from <see cref="BeneficiaryUniquenessChecker" />
    /// </summary>
    /// <remarks>
    ///     api-checking-account-query
    /// </remarks>
    public class BeneficiaryBankAccountDetails
    {
        public Guid Id { get; set; }
        public int LegacyId { get; set; }
        public bool Blocked { get; set; }
        public bool Canceled { get; set; }
    }
}