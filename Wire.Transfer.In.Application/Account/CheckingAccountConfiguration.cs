namespace Wire.Transfer.In.Application.Account
{
    /// <summary>
    ///     Represent class configuration for checking account api
    /// </summary>
    public class CheckingAccountConfiguration
    {
        public string Url { get; set; }
        public string GetDetailsBeneficiaryAccountPathSegment { get; set; }
    }
}