using System.Collections.Generic;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.WireTransfer
{
    /// <summary>
    ///     Bacen information about <see cref="WireTransfer" />
    /// </summary>
    public class ProtocolTrackable : ValueObject
    {
        public ProtocolTrackable(string statusCode, string statusInformation, string protocolNumber)
        {
            StatusCode = statusCode;
            StatusInformation = statusInformation;
            ProtocolNumber = protocolNumber;
        }

        public string StatusCode { get; }
        public string StatusInformation { get; }
        public string ProtocolNumber { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ProtocolNumber;
            yield return StatusCode;
            yield return StatusInformation;
        }
    }
}