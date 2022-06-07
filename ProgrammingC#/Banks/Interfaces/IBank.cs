using Banks.Accounts;
using Banks.Commands;

namespace Banks.Interfaces
{
    public interface IBank
    {
        decimal TransactionLimitForRegisteredCustomer { get; }

        decimal TransactionLimitForUnregisteredCustomer { get; }

        decimal DebitAnnualInterestOnBalance { get; }

        decimal CreditLimitAmount { get; }

        decimal CreditFee { get; }

        DebitAccount CreateDebitAccount(ICustomer customer);

        CreditAccount CreateCreditAccount(ICustomer customer);

        DepositAccount CreateDepositAccount(
            ICustomer customer,
            int daysToExpire,
            decimal startFunds = 0);

        void RemoveAccount(IAccount account);

        void NotifyAllCustomers(string message);

        void NotifySpecificCustomers(AccountTypes accountType, string message);

        AddCommand AddFundsToAccount(IAccount account, decimal funds);

        WithdrawCommand WithdrawFundsFromAccount(IAccount account, decimal funds);

        TranferCommand TranferFundsBetweenAccounts(IAccount accountToWithdraw, IAccount accountToAdd, decimal funds);

        void UndoCommand(ICommand command);

        void UpdateDay();

        void UpdateMonth();

        IAccount FindAccountById(string id);

        void VerifyCustomer(ICustomer customer, string newPassport, string newAddress);

        void ChangeTransactionLimitForRegisteredCustomer(decimal limit);

        void ChangeTransactionLimitForUnregisteredCustomer(decimal limit);

        void ChangeDebitAnnualInterestOnBalance(decimal interest);

        void ChangeCreditLimitAmount(decimal limit);

        void ChangeCreditFee(decimal fee);
    }
}