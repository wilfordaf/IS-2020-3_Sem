using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Commands;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class Bank : IBank
    {
        private readonly List<IAccount> _accounts = new ();
        private readonly CommandInvoker _invoker = new ();

        public Bank(
            decimal transactionLimitForRegisteredCustomer,
            decimal transactionLimitForUnregisteredCustomer,
            decimal debitAnnualInterestOnBalance,
            decimal creditLimitAmount,
            decimal creditFee)
        {
            TransactionLimitForRegisteredCustomer = transactionLimitForRegisteredCustomer;
            TransactionLimitForUnregisteredCustomer = transactionLimitForUnregisteredCustomer;
            DebitAnnualInterestOnBalance = debitAnnualInterestOnBalance;
            CreditLimitAmount = creditLimitAmount;
            CreditFee = creditFee;
        }

        public decimal TransactionLimitForRegisteredCustomer { get; internal set; }
        public decimal TransactionLimitForUnregisteredCustomer { get; internal set; }
        public decimal DebitAnnualInterestOnBalance { get; internal set; }
        public decimal CreditLimitAmount { get; internal set; }
        public decimal CreditFee { get; internal set; }

        public DebitAccount CreateDebitAccount(ICustomer customer)
        {
            var newAccount = new DebitAccount(
                TransactionLimitForRegisteredCustomer,
                TransactionLimitForUnregisteredCustomer,
                DebitAnnualInterestOnBalance,
                customer);

            _accounts.Add(newAccount);
            return newAccount;
        }

        public CreditAccount CreateCreditAccount(ICustomer customer)
        {
            var newAccount = new CreditAccount(
                TransactionLimitForRegisteredCustomer,
                TransactionLimitForUnregisteredCustomer,
                CreditFee,
                CreditLimitAmount,
                customer);

            _accounts.Add(newAccount);
            return newAccount;
        }

        public DepositAccount CreateDepositAccount(
            ICustomer customer,
            int daysToExpire,
            decimal startFunds = 0)
        {
            var newAccount = new DepositAccount(
                TransactionLimitForRegisteredCustomer,
                TransactionLimitForUnregisteredCustomer,
                startFunds,
                daysToExpire,
                customer);

            _accounts.Add(newAccount);
            return newAccount;
        }

        public void RemoveAccount(IAccount account)
        {
            if (!_accounts.Remove(account))
            {
                throw new BanksException("Such account was not found.");
            }
        }

        public void NotifyAllCustomers(string message)
        {
            _accounts.ForEach(a => a.NotifyOwner(message));
        }

        public void NotifySpecificCustomers(AccountTypes accountType, string message)
        {
            _accounts.ForEach(a =>
            {
                if (a.Type == accountType)
                {
                    a.NotifyOwner(message);
                }
            });
        }

        public void UpdateDay()
        {
            _accounts.ForEach(a => a.UpdateDay());
        }

        public void UpdateMonth()
        {
            _accounts.ForEach(a => a.UpdateMonth());
        }

        public AddCommand AddFundsToAccount(IAccount account, decimal funds)
        {
            var addCommand = new AddCommand(account, funds);
            _invoker.SetCommand(addCommand);
            _invoker.RunCommand();
            return addCommand;
        }

        public WithdrawCommand WithdrawFundsFromAccount(IAccount account, decimal funds)
        {
            var withdrawCommand = new WithdrawCommand(account, funds);
            _invoker.SetCommand(withdrawCommand);
            _invoker.RunCommand();
            return withdrawCommand;
        }

        public TranferCommand TranferFundsBetweenAccounts(
            IAccount accountToWithdraw, IAccount accountToAdd, decimal funds)
        {
            var transferCommand = new TranferCommand(accountToWithdraw, accountToAdd, funds);
            _invoker.SetCommand(transferCommand);
            _invoker.RunCommand();
            return transferCommand;
        }

        public void UndoCommand(ICommand command)
        {
            _invoker.SetCommand(command);
            _invoker.UndoCommand();
        }

        public IAccount FindAccountById(string id)
        {
            return _accounts.FirstOrDefault(a => a.Id.ToString() == id);
        }

        public List<IAccount> FindsAccountsByCustomer(ICustomer customer)
        {
            return _accounts.Where(a => a.Owner == customer).ToList();
        }

        public void VerifyCustomer(ICustomer customer, string newPassport, string newAddress)
        {
            customer.Address = newAddress;
            customer.Passport = newPassport;
            foreach (var id in customer.AccountIds)
            {
                FindAccountById(id.ToString()).UpdateTransactionLimit();
            }
        }

        public void ChangeTransactionLimitForRegisteredCustomer(decimal limit)
        {
            TransactionLimitForRegisteredCustomer = limit;
            _accounts.ForEach(a => a.TransactionLimitForRegisteredCustomer = limit);
        }

        public void ChangeTransactionLimitForUnregisteredCustomer(decimal limit)
        {
            TransactionLimitForUnregisteredCustomer = limit;
            _accounts.ForEach(a => a.TransactionLimitForUnregisteredCustomer = limit);
        }

        public void ChangeDebitAnnualInterestOnBalance(decimal interest)
        {
            DebitAnnualInterestOnBalance = interest;
            _accounts.ForEach(a =>
            {
                if (a is DebitAccount account)
                {
                    account.ChangeAnnualInterest(interest);
                }
            });
        }

        public void ChangeCreditLimitAmount(decimal limit)
        {
            CreditLimitAmount = limit;
            _accounts.ForEach(a =>
            {
                if (a is CreditAccount account)
                {
                    account.ChangeCreditLimit(limit);
                }
            });
        }

        public void ChangeCreditFee(decimal fee)
        {
            CreditFee = fee;
            _accounts.ForEach(a =>
            {
                if (a is CreditAccount account)
                {
                    account.ChangeCreditFee(fee);
                }
            });
        }
    }
}