using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;
using Wire.Transfer.In.Application.Configuration.Validation;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    public class DuplicateExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public DuplicateExceptionProblemDetails(DuplicateException exception,
            DuplicateRuleValidationEnumeration exceptionDuplicateRuleValidationEnumeration)
        {
            Status = StatusCodes.Status400BadRequest;
            Type = nameof(DuplicateExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception, exceptionDuplicateRuleValidationEnumeration);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }
        public new string Extensions { get; set; }
    }
}