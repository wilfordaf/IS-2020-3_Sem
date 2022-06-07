using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts
{
    public class DebitAccount : IAccount
    {
        private decimal _interestFunds;
        private decimal _transactionLimit;

        public DebitAccount(
            decimal transactionLimitForRegisteredCustomer,
            decimal transactionLimitForUnregisteredCustomer,
            decimal annualInterestOnBalance,
            ICustomer owner)
        {
            TransactionLimitForRegisteredCustomer = transactionLimitForRegisteredCustomer;
            TransactionLimitForUnregisteredCustomer = transactionLimitForUnregisteredCustomer;
            AnnualInterestOnBalance = annualInterestOnBalance;
            Owner = owner;
            Owner.AccountIds.Add(Id);
            SetTransactionLimit();
        }

        public ICustomer Owner { get; init; }

        public decimal AnnualInterestOnBalance { get; internal set; }

        public decimal TransactionLimitForRegisteredCustomer { get; set; }

        public decimal TransactionLimitForUnregisteredCustomer { get; set; }

        public decimal Funds { get; internal set; }

        public Guid Id { get; } = Guid.NewGuid();

        public AccountTypes Type => AccountTypes.DebitAccount;

        public void AddFunds(decimal amount)
        {
            Funds += amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (Funds <= 0)
            {
                throw new BanksException("Insufficient Funds.");
            }

            if (amount >= _transactionLimit)
            {
                throw new BanksException("While calling an operation, you have exceeded your transaction limit.");
            }

            if (Funds < amount)
            {
                throw new BanksException("Insufficient Funds.");
            }

            Funds -= amount;
        }

        public void CalculateInterest()
        {
            _interestFunds += AnnualInterestOnBalance /
                              Config.PercentDivider /
                              Config.DaysInYear * Funds;
        }

        public void UpdateDay()
        {
            CalculateInterest();
        }

        public void UpdateMonth()
        {
            Funds += _interestFunds;
            _interestFunds = 0;
        }

        public void NotifyOwner(string message)
        {
            Owner.Update(message);
        }

        public void UpdateTransactionLimit()
        {
            SetTransactionLimit();
        }

        public void ChangeAnnualInterest(decimal interest)
        {
            if (interest <= 0)
            {
                throw new BanksException("Interest cannot be negative.");
            }

            AnnualInterestOnBalance = interest;
        }

        private void SetTransactionLimit()
        {
            _transactionLimit = Owner.IsVerified ?
                TransactionLimitForRegisteredCustomer :
                TransactionLimitForUnregisteredCustomer;
        }
    }
}