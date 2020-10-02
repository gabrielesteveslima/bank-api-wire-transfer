using System.ComponentModel;
using System.Data.SqlClient;
using Flurl.Http;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Wire.Transfer.In.API.Configuration.ProblemDetails.Helpers;
using Wire.Transfer.In.Application.Configuration.Validation;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.API.Configuration.ProblemDetails
{
    public static class ProblemDetailsExtension
    {
        /// <summary>
        ///     Set handlers errors to problem details classes <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails" />
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddProblemDetailsMiddleware(this IServiceCollection services)
        {
            services.AddScoped<ProblemDetailsFilter>();
            services.AddProblemDetails(setup =>
            {
                setup.Map<SqlException>(exception => new InfrastructureExceptionProblemDetails(exception));
                setup.Map<Win32Exception>(exception => new InfrastructureExceptionProblemDetails(exception));
                setup.Map<FlurlHttpException>(exception => new InfrastructureExceptionProblemDetails(exception));
                setup.Map<InvalidCommandException>(exception =>
                    new InvalidCommandRuleValidationExceptionProblemDetails(exception));
                setup.Map<JsonException>(exception => new JsonApiSerializerExceptionProblemDetails(exception));
                setup.Map<DuplicateException>(exception =>
                    new DuplicateExceptionProblemDetails(exception, exception.DuplicateRuleValidationEnumeration));
                setup.Map<BusinessRuleValidationException>(exception =>
                    new BusinessRuleValidationExceptionProblemDetails(exception,
                        exception.BusinessRuleValidationEnumeration));
                setup.Map<NotFoundException>(exception => new NotFoundExceptionProblemDetails(exception));
            });

            return services;
        }
    }
}