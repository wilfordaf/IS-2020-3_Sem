using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts
{
    public class CreditAccount : IAccount
    {
        private decimal _transactionLimit;
        private decimal _creditCommissionSum;

        public CreditAccount(
            decimal transactionLimitForRegisteredCustomer,
            decimal transactionLimitForUnregisteredCustomer,
            decimal fee,
            decimal creditAmount,
            ICustomer owner)
        {
            TransactionLimitForRegisteredCustomer = transactionLimitForRegisteredCustomer;
            TransactionLimitForUnregisteredCustomer = transactionLimitForUnregisteredCustomer;
            Fee = fee;
            CreditAmount = creditAmount;
            Owner = owner;
            Owner.AccountIds.Add(Id);
            SetTransactionLimit();
        }

        public ICustomer Owner { get; init; }

        public decimal TransactionLimitForRegisteredCustomer { get; set; }

        public decimal TransactionLimitForUnregisteredCustomer { get; set; }

        public decimal Fee { get; internal set; }

        public decimal CreditAmount { get; internal set; }

        public decimal Funds { get; internal set; }

        public Guid Id { get; } = Guid.NewGuid();

        public AccountTypes Type => AccountTypes.CreditAccount;

        public void AddFunds(decimal amount)
        {
            Funds += amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (amount >= _transactionLimit)
            {
                throw new BanksException("While calling an operation, you have exceeded your transaction limit.");
            }

            if (Funds + CreditAmount < amount)
            {
                throw new BanksException("Insufficient funds on account.");
            }

            Funds -= amount;
        }

        public void NotifyOwner(string message)
        {
            Owner.Update(message);
        }

        public void UpdateDay()
        {
            if (Funds < 0)
            {
                _creditCommissionSum += Fee;
            }
        }

        public void UpdateMonth()
        {
            Funds -= _creditCommissionSum;
            _creditCommissionSum = 0;
        }

        public void UpdateTransactionLimit()
        {
            SetTransactionLimit();
        }

        public void ChangeCreditLimit(decimal creditLimit)
        {
            if (creditLimit <= 0)
            {
                throw new BanksException("Interest cannot be negative.");
            }

            CreditAmount = creditLimit;
        }

        public void ChangeCreditFee(decimal creditFee)
        {
            if (creditFee <= 0)
            {
                throw new BanksException("Interest cannot be negative.");
            }

            Fee = creditFee;
        }

        private void SetTransactionLimit()
        {
            _transactionLimit = Owner.IsVerified ?
                TransactionLimitForRegisteredCustomer :
                TransactionLimitForUnregisteredCustomer;
        }
    }
}