using System.Collections.Generic;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account.Holders
{
    /// <summary>
    ///     Represent holder documents
    /// </summary>
    public class Document : ValueObject
    {
        public Document(string value)
        {
            DocumentType = DocumentType.GetDocumentType(value);
            Value = value.PadLeft(DocumentType.DocumentLength, '0');
        }

        public string Value { get; }

        public DocumentType DocumentType { get; }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return DocumentType;
        }
    }
}