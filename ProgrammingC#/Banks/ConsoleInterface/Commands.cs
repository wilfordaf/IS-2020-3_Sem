namespace Banks.ConsoleInterface
{
    public enum Commands
    {
        /// <summary>
        /// ...
        /// </summary>
        PrintInfoCurrentAccount,

        /// <summary>
        /// ...
        /// </summary>
        AddDebitAccount,

        /// <summary>
        /// ...
        /// </summary>
        AddCreditAccount,

        /// <summary>
        /// ...
        /// </summary>
        AddDepositAccount,

        /// <summary>
        /// ...
        /// </summary>
        ChooseAccount,

        /// <summary>
        /// ...
        /// </summary>
        DeleteAccountById,

        /// <summary>
        /// ...
        /// </summary>
        DissipateDays,

        /// <summary>
        /// ...
        /// </summary>
        DissipateMonths,

        /// <summary>
        /// ...
        /// </summary>
        AddFunds,

        /// <summary>
        /// ...
        /// </summary>
        WithdrawFunds,

        /// <summary>
        /// ...
        /// </summary>
        TransferFunds,

        /// <summary>
        /// ...
        /// </summary>
        UndoLastCommand,

        /// <summary>
        /// ...
        /// </summary>
        ReadNotifications,

        /// <summary>
        /// ...
        /// </summary>
        DiscardNotifications,

        /// <summary>
        /// ...
        /// </summary>
        SetNewTransactionLimitForUnregisteredCustomer,

        /// <summary>
        /// ...
        /// </summary>
        SetNewTransactionLimitForRegisteredCustomer,

        /// <summary>
        /// ...
        /// </summary>
        SetNewDebitAnnualInterestOnBalance,

        /// <summary>
        /// ...
        /// </summary>
        SetNewCreditLimitAmount,

        /// <summary>
        /// ...
        /// </summary>
        SetNewCreditFee,

        /// <summary>
        /// ...
        /// </summary>
        ListAllCustomersAccounts,
    }
}