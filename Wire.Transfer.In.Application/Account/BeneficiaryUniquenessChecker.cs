using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using JsonApiSerializer;
using Newtonsoft.Json;
using Wire.Transfer.In.Domain.AggregatesModels.Account;
using Wire.Transfer.In.Domain.AggregatesModels.Account.Holders;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.Account
{
    public class BeneficiaryUniquenessChecker : IAccountUniquenessChecker
    {
        private readonly CheckingAccountConfiguration _checkingAccountApiConfiguration;
        private readonly ILogging _logging;

        public BeneficiaryUniquenessChecker(
            CheckingAccountConfiguration checkingAccountApiConfiguration, ILogging logging)
        {
            _checkingAccountApiConfiguration = checkingAccountApiConfiguration ??
                                               throw new ArgumentException(nameof(checkingAccountApiConfiguration));
            _logging = logging ?? throw new ArgumentException(nameof(logging));
        }

        /// <summary>
        ///     Get details beneficiary from Checking Account
        /// </summary>
        /// <param name="beneficiaryAccountHolder"></param>
        /// <param name="beneficiaryBankAccountDetails"></param>
        /// <returns></returns>
        public async Task<BankAccount> AccountUniquenessChecker(
            BankAccountHolder beneficiaryAccountHolder, BankAccountDetails beneficiaryBankAccountDetails)
        {
            var json = await
                _checkingAccountApiConfiguration.Url
                    .AppendPathSegment(_checkingAccountApiConfiguration.GetDetailsBeneficiaryAccountPathSegment)
                    .ConfigureRequest(settings =>
                    {
                        settings.BeforeCall = call => { _logging.Information(call.Request.RequestUri); };
                    })
                    .SetQueryParam("filter[person.document]",
                        beneficiaryAccountHolder.HolderDocument.Value)
                    .SetQueryParam("filter[number]", beneficiaryBankAccountDetails.GetAccountWithoutDigit())
                    .SetQueryParam("filter[digit]", beneficiaryBankAccountDetails.Digit)
                    .SetQueryParam("filter[branch]", beneficiaryBankAccountDetails.Branch)
                    .GetStringAsync();

            var response =
                (JsonConvert.DeserializeObject<List<BeneficiaryBankAccountDetails>>(json,
                     new JsonApiSerializerSettings()) ??
                 throw new JsonException("Error to build https://jsonapi.org/ specification")).FirstOrDefault();

            if (response != null)
            {
                var account =
                    new BankAccountBuilder()
                        .WithBankAccountId(response.Id)
                        .WithBankAccountLegacyId(response.LegacyId)
                        .WithBankAccountHolder(beneficiaryAccountHolder.HolderName,
                            beneficiaryAccountHolder.HolderDocument.Value)
                        .WithBankAccountDetails(beneficiaryBankAccountDetails.Account,
                            beneficiaryBankAccountDetails.Branch, beneficiaryBankAccountDetails.Number)
                        .IsBlocked(response.Blocked)
                        .IsCanceled(response.Canceled)
                        .Build();

                return account;
            }

            return new BankAccountBuilder().Build();
        }
    }
}