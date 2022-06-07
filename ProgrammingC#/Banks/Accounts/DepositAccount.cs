using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts
{
    public class DepositAccount : IAccount
    {
        private int _daysToExpire;
        private decimal _interestFunds;
        private decimal _transactionLimit;
        private decimal _interestOnBalance;

        public DepositAccount(
            decimal transactionLimitForRegisteredCustomer,
            decimal transactionLimitForUnregisteredCustomer,
            decimal funds,
            int daysToExpire,
            ICustomer owner)
        {
            TransactionLimitForRegisteredCustomer = transactionLimitForRegisteredCustomer;
            TransactionLimitForUnregisteredCustomer = transactionLimitForUnregisteredCustomer;
            Funds = funds;
            _daysToExpire = daysToExpire;
            Owner = owner;
            Owner.AccountIds.Add(Id);
            SetTransactionLimit();
            SetInterestByStartFunds(funds);
        }

        public ICustomer Owner { get; init; }

        public decimal TransactionLimitForRegisteredCustomer { get; set; }

        public decimal TransactionLimitForUnregisteredCustomer { get; set; }

        public decimal Funds { get; internal set; }

        public Guid Id { get; } = Guid.NewGuid();

        public AccountTypes Type => AccountTypes.DepositAccount;

        public void AddFunds(decimal amount)
        {
            Funds += amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (Funds <= 0)
            {
                throw new BanksException("Insufficient funds.");
            }

            if (_daysToExpire > 0)
            {
                throw new BanksException($"Account is temporary blocked for withdrawal. {_daysToExpire} days left.");
            }

            if (amount >= _transactionLimit)
            {
                throw new BanksException("While calling an operation, you have exceeded your transaction limit.");
            }

            if (Funds < amount)
            {
                throw new BanksException("Insufficient funds.");
            }

            Funds -= amount;
        }

        public void CalculateInterest()
        {
            _interestFunds += _interestOnBalance /
                Config.PercentDivider /
                Config.DaysInYear * Funds;
        }

        public void UpdateDay()
        {
            CalculateInterest();
            if (_daysToExpire > 0)
            {
                _daysToExpire--;
            }
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

        private void SetTransactionLimit()
        {
            _transactionLimit = Owner.IsVerified ?
                TransactionLimitForRegisteredCustomer :
                TransactionLimitForUnregisteredCustomer;
        }

        private void SetInterestByStartFunds(decimal funds)
        {
            _interestOnBalance = funds switch
            {
                <= 50000 => 3,
                <= 100000 => 3.5M,
                _ => 4,
            };
        }
    }
}