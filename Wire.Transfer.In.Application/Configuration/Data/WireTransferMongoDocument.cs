using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Wire.Transfer.In.Application.Configuration.Data
{
    /// <summary>
    ///     Represent <see cref="MongoDB" /> document
    /// </summary>
    [BsonIgnoreExtraElements]
    public class WireTransferMongoDocument
    {
        [BsonElement("WIRE_TRANSFER_IN_UUID")] public string WireTransferId { get; set; }

        [BsonElement("SENDER_ACCOUNT_NU")] public string SenderAccountNumber { get; set; }

        [BsonElement("SENDER_NM")] public string SenderName { get; set; }

        [BsonElement("SENDER_BRANCH_NU")] public string SenderBranchNumber { get; set; }

        [BsonElement("SENDER_BANK_NU")] public string SenderBankNumber { get; set; }

        [BsonElement("SENDER_CPF_CNPJ_NU")] public string SenderDocument { get; set; }

        [BsonElement("BENEFICIARY_ACCOUNT_UUID")]
        public string BeneficiaryAccountId { get; set; }

        [BsonElement("WIRE_TRANSFER_IN_TP")] public int WireTransferType { get; set; }

        [BsonElement("WIRE_TRANSFER_IN_PROTOCOL_NU")]
        public int ProtocolNumber { get; set; }

        [BsonElement("BACEN_RETURN_CD")] public string ProtocolCode { get; set; }

        [BsonElement("BACEN_ST")] public string ProtocolStatus { get; set; }

        [BsonElement("TRADE_DT")] public DateTime TradeDate { get; set; }

        [BsonElement("TRANSACTION_UUID")] public string TransactionId { get; set; }

        [BsonElement("WIRE_TRANSFER_IN_VL")] public decimal Amount { get; set; }
    }
}