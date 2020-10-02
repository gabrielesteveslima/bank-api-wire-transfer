using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers
{
    /// <summary>
    ///     Helper for get errors from problem details
    /// </summary>
    public class ProblemDetailsWrapErrors
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Code { get; set; }
        public string Type { get; set; }

        public static IEnumerable<ProblemDetailsWrapErrors> GetErrors(IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures.Select(error => new ProblemDetailsWrapErrors
            {
                Title = $"{error.PropertyName} failed validation",
                Description = error.ErrorMessage
            }).ToList();
        }

        public static IEnumerable<ProblemDetailsWrapErrors> GetErrors(Exception exception,
            Enumeration ruleValidationEnumeration)
        {
            return new[]
            {
                new ProblemDetailsWrapErrors
                {
                    Description = exception.Message,
                    Code = ruleValidationEnumeration?.Id,
                    Type = ruleValidationEnumeration?.Name
                }
            };
        }
    }
}