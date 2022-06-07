using System;
using Banks.Interfaces;

namespace Banks.Accounts
{
    public interface IAccount
    {
        ICustomer Owner { get; init; }

        Guid Id { get; }

        decimal Funds { get; }

        AccountTypes Type { get; }

        decimal TransactionLimitForRegisteredCustomer { get; set; }

        decimal TransactionLimitForUnregisteredCustomer { get; set; }

        void AddFunds(decimal amount);

        void WithdrawFunds(decimal amount);

        void NotifyOwner(string message);

        void UpdateDay();

        void UpdateMonth();

        void UpdateTransactionLimit();
    }
}