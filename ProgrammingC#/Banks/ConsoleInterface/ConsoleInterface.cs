using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Commands;
using Banks.Interfaces;
using Banks.Models;
using Banks.Tools;

namespace Banks.ConsoleInterface
{
    public class ConsoleInterface
    {
        private readonly Dictionary<Commands, Action<string[]>> _commands;
        private readonly IBank _bank;
        private readonly ICustomer _customer;
        private readonly ICentralBank _centralBank;
        private readonly TimeManager _timeManager;

        private IAccount _account;
        private ICommand _lastExecutedCommand;

        public ConsoleInterface(ICentralBank centralBank, IBank bank, ICustomer customer)
        {
            _commands = new Dictionary<Commands, Action<string[]>>()
            {
                [Commands.PrintInfoCurrentAccount] = PrintInfoCurrentAccount,
                [Commands.AddDebitAccount] = AddDebitAccount,
                [Commands.AddCreditAccount] = AddCreditAccount,
                [Commands.AddDepositAccount] = AddDepositAccount,
                [Commands.ChooseAccount] = ChooseAccount,
                [Commands.DeleteAccountById] = DeleteAccountById,
                [Commands.DissipateDays] = DissipateDays,
                [Commands.DissipateMonths] = DissipateMonths,
                [Commands.AddFunds] = AddFunds,
                [Commands.WithdrawFunds] = WithdrawFunds,
                [Commands.TransferFunds] = TransferFunds,
                [Commands.UndoLastCommand] = UndoLastCommand,
                [Commands.ReadNotifications] = ReadNotifications,
                [Commands.DiscardNotifications] = DiscardNotifications,
                [Commands.SetNewTransactionLimitForRegisteredCustomer] = SetNewTransactionLimitForRegisteredCustomer,
                [Commands.SetNewTransactionLimitForUnregisteredCustomer] = SetNewTransactionLimitForUnregisteredCustomer,
                [Commands.SetNewDebitAnnualInterestOnBalance] = SetNewDebitAnnualInterestOnBalance,
                [Commands.SetNewCreditLimitAmount] = SetNewCreditLimitAmount,
                [Commands.SetNewCreditFee] = SetNewCreditFee,
                [Commands.ListAllCustomersAccounts] = ListAllCustomersAccounts,
            };

            _centralBank = centralBank;
            _bank = bank;
            _customer = customer;
            _timeManager = TimeManager.GetTimeManager(_centralBank);
        }

        public void Start()
        {
            string line;
            while ((line = Console.ReadLine()) != string.Empty)
            {
                if (line is null || line[0] != '/' || line.Length == 1)
                {
                    Console.WriteLine("Incorrect input - not a command.");
                    continue;
                }

                if (line.Split(' ')[0] == "/Exit")
                {
                    break;
                }

                Commands command;

                try
                {
                    command = Parse(line[1..].Split(' ')[0]);
                }
                catch (BanksException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] args = line.Split(' ')[1..];

                try
                {
                    ExecuteCommand(command, args);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid parameters.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch (BanksException e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Command execution failed: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid type of input parameters.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        private static Commands Parse(string name)
        {
            if (Enum.TryParse(name, out Commands command))
            {
                return command;
            }

            throw new BanksException("Invalid command.");
        }

        private void ExecuteCommand(Commands command, string[] args)
        {
            _commands[command].Invoke(args);
        }

        private void PrintInfoCurrentAccount(string[] args)
        {
            if (_account is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Account to act was not chosen.");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            Console.WriteLine($"{_account.Id} {_account.Type} has {_account.Funds}");
        }

        private void ChooseAccount(string[] args)
        {
            _account = _bank.FindAccountById(args[0]);
            Console.WriteLine($"Chosen account to act with: {_account.Id}.");
        }

        private void AddDebitAccount(string[] args)
        {
            var account = _bank.CreateDebitAccount(_customer);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Debit account successfully created with id: {account.Id}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void AddCreditAccount(string[] args)
        {
            var account = _bank.CreateCreditAccount(_customer);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Credit account successfully created with id: {account.Id}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void AddDepositAccount(string[] args)
        {
            var account = _bank.CreateDepositAccount(_customer, Convert.ToInt32(args[0]), Convert.ToDecimal(args[1]));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Deposit account successfully created with id: {account.Id}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void DeleteAccountById(string[] args)
        {
            _bank.RemoveAccount(_bank.FindAccountById(args[0]));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Account with id: {args[0]} successfully removed.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void TransferFunds(string[] args)
        {
            _lastExecutedCommand = _bank.TranferFundsBetweenAccounts(
                _bank.FindAccountById(args[0]),
                _bank.FindAccountById(args[1]),
                Convert.ToDecimal(args[2]));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Funds were transfered from account with id: {args[0]} to account with id: {args[1]}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void WithdrawFunds(string[] args)
        {
            _lastExecutedCommand = _bank.WithdrawFundsFromAccount(
                _bank.FindAccountById(args[0]),
                Convert.ToDecimal(args[1]));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Funds were withdrawn from account with id: {args[0]}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void AddFunds(string[] args)
        {
            _lastExecutedCommand = _bank.AddFundsToAccount(
                _bank.FindAccountById(args[0]),
                Convert.ToDecimal(args[1]));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Funds were added to account with id: {args[0]}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void UndoLastCommand(string[] args)
        {
            if (_lastExecutedCommand is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No command was executed.");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            _bank.UndoCommand(_lastExecutedCommand);
            _lastExecutedCommand = null;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Last command was successfully undone");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void DissipateDays(string[] args)
        {
            _timeManager.DissipateDays(Convert.ToInt32(args[0]));
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Successfully rewinded {args[0]} days");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void DissipateMonths(string[] args)
        {
            _timeManager.DissipateMonths(Convert.ToInt32(args[0]));
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Successfully rewinded {args[0]} months");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ReadNotifications(string[] args)
        {
            _customer.Notifications.ForEach(n => Console.WriteLine(n));
        }

        private void DiscardNotifications(string[] args)
        {
            _customer.ClearNotifications();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Cleared all notifications");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void SetNewCreditFee(string[] args)
        {
            _centralBank.SetNewCreditFee(Convert.ToDecimal(args[0]), _bank);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"New credit fee {args[0]} was set");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void SetNewCreditLimitAmount(string[] args)
        {
            _centralBank.SetNewCreditLimitAmount(Convert.ToDecimal(args[0]), _bank);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"New credit limit {args[0]} was set");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void SetNewDebitAnnualInterestOnBalance(string[] args)
        {
            _centralBank.SetNewDebitAnnualInterestOnBalance(Convert.ToDecimal(args[0]), _bank);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"New debit annual interest {args[0]} was set");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void SetNewTransactionLimitForUnregisteredCustomer(string[] args)
        {
            _centralBank.SetNewTransactionLimitForUnregisteredCustomer(Convert.ToDecimal(args[0]), _bank);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"New transaction limit for unregistered customers was set {args[0]} was set");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void SetNewTransactionLimitForRegisteredCustomer(string[] args)
        {
            _centralBank.SetNewTransactionLimitForRegisteredCustomer(Convert.ToDecimal(args[0]), _bank);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"New transaction limit for registered customers was set {args[0]} was set");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ListAllCustomersAccounts(string[] args)
        {
            _customer.AccountIds.ForEach(a =>
            {
                var account = _bank.FindAccountById(a.ToString());
                Console.WriteLine($"{account.Id} {account.Type} has {account.Funds}");
            });
        }
    }
}