using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Infrastructure.Logs
{
    public class LogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => new ConsoleLifetimeOptions
            {
                SuppressStatusMessages = true
            }).As<ConsoleLifetimeOptions>();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            builder.RegisterType<Logging>().As<ILogging>();

            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}