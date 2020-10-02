using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;
using Wire.Transfer.In.Application.Configuration.Validation;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    /// <summary>
    ///     Represents errors on command model state <see cref="FluentValidation" />
    /// </summary>
    public class InvalidCommandRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public InvalidCommandRuleValidationExceptionProblemDetails(InvalidCommandException exception)
        {
            Status = StatusCodes.Status400BadRequest;
            Type = nameof(InvalidCommandRuleValidationExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception.Errors);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }
        public new string Extensions { get; set; }
    }
}