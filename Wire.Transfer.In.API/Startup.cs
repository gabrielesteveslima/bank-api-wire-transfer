using System;
using Hellang.Middleware.ProblemDetails;
using JsonApiSerializer.ContractResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Wire.Transfer.In.API.Configuration;
using Wire.Transfer.In.API.Configuration.Docs;
using Wire.Transfer.In.API.Configuration.ProblemDetails;
using Wire.Transfer.In.Infrastructure;

namespace Wire.Transfer.In.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new JsonApiContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.Converters = new JsonConverter[] {new TrimmingJsonConverter()};
                });

            services
                .AddVersioningSystem()
                .AddSwaggerDocumentation()
                .AddProblemDetailsMiddleware()
                .AddHealthChecks();

            return ApplicationStartup.Initialize(
                services,
                Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
            });

            app.UseSwaggerDocumentation();
        }
    }
}