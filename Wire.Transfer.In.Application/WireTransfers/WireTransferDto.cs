using System;
using JsonApiSerializer.JsonApi;
using Microsoft.Toolkit.Extensions;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers
{
    public class WireTransferDto
    {
        public Guid Id { get; set; }
        public SenderDto Sender { get; set; }
        public BeneficiaryDto Beneficiary { get; set; }
        public ProtocolDto Protocol { get; set; }
        public string WireTransferType { get; set; }
        public string Type => "wire-transfers";
        public decimal Amount { get; set; }
        public DateTime TradeDate { get; set; }
        public TransactionDto Transaction { get; set; }
        public AccountDto Account { get; set; }
        public Links Links { get; set; }

        public void AddSelfLik(string host)
        {
            Links = new Links
            {
                {
                    "self", new Link
                    {
                        Href = $"{host}/wire-transfers/v1/checking-accounts/{Account.Id}/wire-transfers-in/{Id}"
                    }
                }
            };
        }
    }

    public enum WireTransferDtoTypeEnum
    {
        CIP = 31,
        STR = 32
    }

    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Type => "transactions";
    }

    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Type => "accounts";
    }

    public class ProtocolDto
    {
        public string Number { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }

    public class SenderDto
    {
        private string _numberSenderBank;
        private string _senderName;

        public string Name
        {
            get => _senderName.Truncate(100, false);
            set => _senderName = value;
        }

        public DocumentDto Document { get; set; }

        public string Number
        {
            get => _numberSenderBank.Padding(3, '0');
            set => _numberSenderBank = value;
        }

        public string Branch { get; set; }
        public string Account { get; set; }
    }

    public class BeneficiaryDto
    {
        private string _beneficiaryName;
        private string _beneficiaryNumberBank;

        public string Name
        {
            get => _beneficiaryName.Truncate(100, false);
            set => _beneficiaryName = value;
        }

        public DocumentDto Document { get; set; }

        public string Number
        {
            get => _beneficiaryNumberBank.Padding(3, '0');
            set => _beneficiaryNumberBank = value;
        }

        public string Branch { get; set; }
        public string Account { get; set; }
    }

    public class DocumentDto
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }

    public enum DocumentDtoType
    {
        CPF,
        CNPJ
    }
}