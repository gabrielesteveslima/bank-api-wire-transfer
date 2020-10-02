using Autofac;
using Wire.Transfer.In.Application.Configuration.Data;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer;
using Wire.Transfer.In.Infrastructure.Domain.WireTransfers;

namespace Wire.Transfer.In.Infrastructure.Database
{
    public class DatabaseModule : Module
    {
        private readonly string _cqrsConnectionString;
        private readonly string _sqlConnectionString;

        public DatabaseModule(string sqlConnectionString, string cqrsConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
            _cqrsConnectionString = cqrsConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionsFactory>()
                .As<IConnectionFactory>()
                .WithParameter("sqlConnectionString", _sqlConnectionString)
                .WithParameter("cqrsConnectionString", _cqrsConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<WireTransferRepository>()
                .As<IWireTransferRepository>()
                .InstancePerLifetimeScope();
        }
    }
}