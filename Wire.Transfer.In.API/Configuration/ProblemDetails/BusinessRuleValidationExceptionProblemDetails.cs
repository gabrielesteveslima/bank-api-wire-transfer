using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    /// <summary>
    ///     Represent business exceptions
    /// </summary>
    public class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception,
            BusinessRuleValidationEnumeration ruleValidationEnumeration)
        {
            Status = StatusCodes.Status400BadRequest;
            Type = nameof(BusinessRuleValidationExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception, ruleValidationEnumeration);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }
        public new string Extensions { get; set; }
    }
}