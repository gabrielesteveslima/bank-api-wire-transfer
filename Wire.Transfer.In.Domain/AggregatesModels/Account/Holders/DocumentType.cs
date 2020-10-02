using Wire.Transfer.In.Domain.SeedWorks;
using static System.Int32;
using static System.String;

namespace Wire.Transfer.In.Domain.AggregatesModels.Account.Holders
{
    /// <summary>
    ///     Represent accepts document types
    /// </summary>
    public class DocumentType : Enumeration
    {
        private static readonly DocumentType Cpf = new DocumentType(1, "CPF", 11);
        private static readonly DocumentType Cnpj = new DocumentType(2, "CNPJ", 14);

        private DocumentType(int id, string name, int documentLength) : base(id, name)
        {
            DocumentLength = documentLength;
        }

        private DocumentType(int id, string name) : base(id, name)
        {
        }

        public int DocumentLength { get; }

        public static DocumentType GetDocumentType(string documentValue)
        {
            return documentValue switch
            {
                var value when IsMatchCpf(value) => Cpf,
                var value when IsMatchCnpj(value) => Cnpj,
                _ => throw new BusinessRuleValidationException(
                    "Document is not valid", BusinessRuleValidationEnumeration.ERROR_BENEFICIARY_DOCUMENT_NOT_IS_VALID)
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        private static bool IsMatchCpf(string cpf)
        {
            const int totalLengthValid = 11;

            var mult1 = new[] {10, 9, 8, 7, 6, 5, 4, 3, 2};
            var mult2 = new[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};

            var cpfCompleted = Empty;
            var digit = Empty;

            //there is a rule that only the first 3 digits of the cpf can be sequentially 0
            if (cpf.Length < 8)
                return false;

            cpfCompleted = cpf.PadLeft(totalLengthValid, '0').Substring(0, 9);
            var sum = 0;

            for (var i = 0; i < 9; i++)
                sum += Parse(cpfCompleted[i].ToString()) * mult1[i];

            var restDiv = sum % 11;

            if (restDiv < 2)
                restDiv = 0;
            else
                restDiv = 11 - restDiv;

            digit = restDiv.ToString();
            cpfCompleted += digit;
            sum = 0;

            for (var i = 0; i < 10; i++)
                sum += Parse(cpfCompleted[i].ToString()) * mult2[i];

            restDiv = sum % 11;

            if (restDiv < 2)
                restDiv = 0;
            else
                restDiv = 11 - restDiv;

            digit += restDiv;
            return cpf.EndsWith(digit);
        }

        /// <summary>
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        private static bool IsMatchCnpj(string cnpj)
        {
            const int totalLengthValid = 14;

            var mult1 = new[] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var mult2 = new[] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            var cnpjCompleted = cnpj.PadLeft(totalLengthValid, '0').Substring(0, 12);

            var sum = 0;
            for (var i = 0; i < 12; i++)
                sum += Parse(cnpjCompleted[i].ToString()) * mult1[i];

            var restDiv = sum % 11;
            if (restDiv < 2)
                restDiv = 0;
            else
                restDiv = 11 - restDiv;

            var digit = restDiv.ToString();
            cnpjCompleted += digit;

            sum = 0;
            for (var i = 0; i < 13; i++)
                sum += Parse(cnpjCompleted[i].ToString()) * mult2[i];

            restDiv = sum % 11;
            if (restDiv < 2)
                restDiv = 0;
            else
                restDiv = 11 - restDiv;

            digit += restDiv;
            return cnpj.EndsWith(digit);
        }
    }
}