using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wire.Transfer.In.Infrastructure.Database;
using Wire.Transfer.In.Infrastructure.Domain;
using Wire.Transfer.In.Infrastructure.InProc;
using Wire.Transfer.In.Infrastructure.Logs;
using Wire.Transfer.In.Infrastructure.Resilience;

namespace Wire.Transfer.In.Infrastructure
{
    public static class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var serviceProvider = CreateAutofacServiceProvider(services, configuration);
            return serviceProvider;
        }

        private static IServiceProvider CreateAutofacServiceProvider(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterModule(new DatabaseModule(
                configuration.GetConnectionString("MSSqlConnection"),
                configuration.GetConnectionString("MongodbConnection")));

            container.RegisterModule(new InProcModule(configuration));
            container.RegisterModule(new DomainModule(configuration));
            container.RegisterModule(new LogModule());
            container.RegisterModule(new ResilienceModule());

            var buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));
            var serviceProvider = new AutofacServiceProvider(buildContainer);
            CompositionRoot.SetContainer(buildContainer);
            return serviceProvider;
        }
    }
}