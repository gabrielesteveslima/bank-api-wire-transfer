using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Wire.Transfer.In.Application.Configuration.Validation;
using Wire.Transfer.In.Domain.SeedWorks;
using Wire.Transfer.In.Infrastructure.Logs;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers
{
    /// <summary>
    ///     Filters any exception from the application
    /// </summary>
    public class ProblemDetailsFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            switch (exception)
            {
                case NotFoundException notFoundException:
                    Log.Warning(notFoundException);
                    break;
                case BusinessRuleValidationException businessRuleValidationException:
                    Log.Error(businessRuleValidationException);
                    break;
                case InvalidCommandException commandException:
                    Log.Error(commandException.Errors);
                    break;
                case DuplicateException duplicateTransactionException:
                    Log.Error(duplicateTransactionException);
                    break;
                case JsonException jsonException:
                    Log.Error(jsonException);
                    break;
                default:
                    Log.Error(exception);
                    break;
            }
        }
    }
}