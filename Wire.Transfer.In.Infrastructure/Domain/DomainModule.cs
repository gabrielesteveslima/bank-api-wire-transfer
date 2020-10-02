using Autofac;
using Microsoft.Extensions.Configuration;
using Wire.Transfer.In.Application.Account;
using Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.MakeTransaction;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.WireTransfer.Transaction;

namespace Wire.Transfer.In.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        private readonly IConfiguration _configuration;

        public DomainModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new CheckingAccountConfiguration
            {
                Url = _configuration[
                    $"{nameof(CheckingAccountConfiguration)}:{nameof(CheckingAccountConfiguration.Url)}"],
                GetDetailsBeneficiaryAccountPathSegment = _configuration[
                    $"{nameof(CheckingAccountConfiguration)}:{nameof(CheckingAccountConfiguration.GetDetailsBeneficiaryAccountPathSegment)}"]
            }).As<CheckingAccountConfiguration>();

            builder.Register(c => new TransactionConfiguration
                {
                    Url = _configuration[$"{nameof(TransactionConfiguration)}:{nameof(TransactionConfiguration.Url)}"],
                    CreateTransactionPathSegment =
                        _configuration[
                            $"{nameof(TransactionConfiguration)}:{nameof(TransactionConfiguration.CreateTransactionPathSegment)}"],
                    GetDetailsTransactionPathSegment = _configuration[
                        $"{nameof(TransactionConfiguration)}:{nameof(TransactionConfiguration.GetDetailsTransactionPathSegment)}"]
                })
                .As<TransactionConfiguration>();

            builder.RegisterType<BeneficiaryUniquenessChecker>().As<IAccountUniquenessChecker>();
            builder.RegisterType<BeneficiaryAccountBalance>().As<IBeneficiaryAccountBalance>();
        }
    }
}