namespace Wire.Transfer.In.Domain.SeedWorks
{
    public static class StringExtensions
    {
        public static string Padding(this string value, int totalWidth, char paddingChar)
        {
            return string.IsNullOrEmpty(value) ? value : value.PadLeft(totalWidth, paddingChar);
        }
    }
}