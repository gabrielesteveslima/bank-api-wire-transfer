using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    /// <summary>
    ///     Represent infrastructure exceptions
    /// </summary>
    /// <example>
    ///     <see cref="FlurlHttpException" /> <see cref="Win32Exception" /> <see cref="SqlException" />
    /// </example>
    public class InfrastructureExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public InfrastructureExceptionProblemDetails(Exception exception)
        {
            Status = StatusCodes.Status500InternalServerError;
            Type = nameof(InfrastructureExceptionProblemDetails);
            Errors = ProblemDetailsWrapErrors.GetErrors(exception, null);
        }

        public IEnumerable<ProblemDetailsWrapErrors> Errors { get; }

        public new string Extensions { get; set; }
    }
}