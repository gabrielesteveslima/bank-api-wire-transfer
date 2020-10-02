using System;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.Configuration.Validation
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string message, DuplicateRuleValidationEnumeration duplicateRuleValidationEnumeration)
            : base(message)
        {
            DuplicateRuleValidationEnumeration = duplicateRuleValidationEnumeration;
        }

        public DuplicateRuleValidationEnumeration DuplicateRuleValidationEnumeration { get; }
    }

    public class DuplicateRuleValidationEnumeration : ExceptionRuleValidationEnumeration
    {
        public static readonly DuplicateRuleValidationEnumeration ERROR_WIRE_TRANSFER_TRANSACTION_DUPLICATED =
            new DuplicateRuleValidationEnumeration(40004, nameof(ERROR_WIRE_TRANSFER_TRANSACTION_DUPLICATED));

        public static readonly DuplicateRuleValidationEnumeration ERROR_WIRE_TRANSFER_REJECTED_DUPLICATED =
            new DuplicateRuleValidationEnumeration(40005, nameof(ERROR_WIRE_TRANSFER_REJECTED_DUPLICATED));

        public static readonly DuplicateRuleValidationEnumeration ERROR_WIRE_TRANSFER_DUPLICATED =
            new DuplicateRuleValidationEnumeration(40006, nameof(ERROR_WIRE_TRANSFER_DUPLICATED));

        private DuplicateRuleValidationEnumeration(int id, string name) : base(id, name)
        {
        }
    }
}