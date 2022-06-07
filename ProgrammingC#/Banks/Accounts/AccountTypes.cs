namespace Banks.Accounts
{
    public enum AccountTypes
    {
        /// <summary>
        /// An ordinary account with a fixed interest on the balance.
        /// </summary>
        DebitAccount,

        /// <summary>
        /// An account with credit limit within which it can have negative balance.
        /// </summary>
        CreditAccount,

        /// <summary>
        /// An account from which money cannot be withdrawn or transferred until its term expires.
        /// </summary>
        DepositAccount,
    }
}