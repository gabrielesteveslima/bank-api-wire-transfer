using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    public class JsonApiSerializerExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public JsonApiSerializerExceptionProblemDetails(JsonException exception)
        {
            Status = StatusCodes.Status500InternalServerError;
            Type = nameof(InvalidCommandRuleValidationExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception, null);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }
        public new string Extensions { get; set; }
    }
}