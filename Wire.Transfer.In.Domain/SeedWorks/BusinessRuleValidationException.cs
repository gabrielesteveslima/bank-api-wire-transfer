using System;
using Wire.Transfer.In.Domain.AggregatesModels.Account;

namespace Wire.Transfer.In.Domain.SeedWorks
{
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(string details,
            BusinessRuleValidationEnumeration businessRuleValidationEnumeration) : base(details)
        {
            Details = details;
            BusinessRuleValidationEnumeration = businessRuleValidationEnumeration;
        }

        public BusinessRuleValidationException(string details, BankAccountStatusType bankAccountStatusType,
            BusinessRuleValidationEnumeration businessRuleValidationEnumeration) :
            this(details, businessRuleValidationEnumeration)
        {
            Details = details;
            BankAccountStatusType = bankAccountStatusType;
        }

        public string Details { get; }
        public BusinessRuleValidationEnumeration BusinessRuleValidationEnumeration { get; }

        public BankAccountStatusType BankAccountStatusType { get; }
    }

    public class BusinessRuleValidationEnumeration : ExceptionRuleValidationEnumeration
    {
        public static readonly BusinessRuleValidationEnumeration ERROR_BENEFICIARY_ACCOUNT_NOT_FOUND =
            new BusinessRuleValidationEnumeration(40001, nameof(ERROR_BENEFICIARY_ACCOUNT_NOT_FOUND));

        public static readonly BusinessRuleValidationEnumeration ERROR_BENEFICIARY_DOCUMENT_NOT_IS_VALID =
            new BusinessRuleValidationEnumeration(40002, nameof(ERROR_BENEFICIARY_DOCUMENT_NOT_IS_VALID));

        public static readonly BusinessRuleValidationEnumeration ERROR_BENEFICIARY_ACCOUNT_IS_CANCELED =
            new BusinessRuleValidationEnumeration(40003, nameof(ERROR_BENEFICIARY_ACCOUNT_IS_CANCELED));

        private BusinessRuleValidationEnumeration(int id, string name) : base(id, name)
        {
        }
    }
}