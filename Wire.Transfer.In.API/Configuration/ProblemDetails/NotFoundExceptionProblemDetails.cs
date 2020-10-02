using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    /// <summary>
    ///     When the Mongo Db service not found results
    /// </summary>
    public class NotFoundExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public NotFoundExceptionProblemDetails(Exception exception)
        {
            Status = StatusCodes.Status404NotFound;
            Type = nameof(NotFoundExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception, null);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }

        public new string Extensions { get; set; }
    }
}