namespace Banks.Interfaces
{
    public interface ICentralBank
    {
        void AddBank(IBank bank);

        void RemoveBank(IBank bank);

        void SetNewTransactionLimitForUnregisteredCustomer(decimal limit, IBank bank);

        void SetNewTransactionLimitForRegisteredCustomer(decimal limit, IBank bank);

        void SetNewDebitAnnualInterestOnBalance(decimal interest, IBank bank);

        void SetNewCreditLimitAmount(decimal limit, IBank bank);

        void SetNewCreditFee(decimal fee, IBank bank);

        void NotifyBanksDayPassed();

        void NotifyBanksMonthPassed();
    }
}