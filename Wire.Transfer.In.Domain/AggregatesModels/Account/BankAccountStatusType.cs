namespace Wire.Transfer.In.Domain.AggregatesModels.Account
{
    /// <summary>
    ///     Bank account status <see cref="BankAccount" />
    /// </summary>
    public enum BankAccountStatusType
    {
        Canceled = 3,
        HolderDocumentNotFound = 4,
        BankAccountNotFound = 5
    }
}