using System;
using Banks.Accounts;
using Banks.Builders;
using Banks.Commands;
using Banks.Interfaces;
using Banks.Models;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class Tests
    {
        private static readonly ICentralBank MyCentralBank = new CentralBank();
        private static readonly IBank MyBank = new Bank(
            100000,
            10000,
            5,
            50000,
            500);

        private readonly TimeManager _timeManager = TimeManager.GetTimeManager(MyCentralBank);

        [Test]
        public void WithdrawFromDepositBeforeExpiration_RewindTime_WithdrawAfterExpiration_ThrowException()
        {
            MyCentralBank.AddBank(MyBank);
            Customer customer = new CustomerBuilder().
                SetName("Sergey", "Yurpalov").
                SetPassport("123").
                SetAddress("123").
                Build();

            DepositAccount depositAccount = MyBank.CreateDepositAccount(customer, 10, 75000);

            Assert.Catch<BanksException>(() =>
            {
                MyBank.WithdrawFundsFromAccount(depositAccount, 10000);
            });

            _timeManager.DissipateDays(11);
            MyBank.WithdrawFundsFromAccount(depositAccount, 10000);
            Assert.AreEqual(65000, depositAccount.Funds);
        }

        [Test]
        public void CreateAccounts_UseTransactionCommands_UndoTransactionCommands_RewindTimeForDividents()
        {
            MyCentralBank.AddBank(MyBank);
            Customer customer1 = new CustomerBuilder().SetName("Sergey", "Yurpalov").Build();

            Customer customer2 = new CustomerBuilder().SetName("Vlad", "Povish").SetAddress("1234").Build();

            Customer customer3 = new CustomerBuilder().SetName("123", "123").SetAddress("1234").SetPassport("1234")
                .Build();

            DebitAccount debitAccount = MyBank.CreateDebitAccount(customer1);
            CreditAccount creditAccount = MyBank.CreateCreditAccount(customer2);
            DepositAccount depositAccount = MyBank.CreateDepositAccount(customer3, 10, 75000);

            AddCommand addCommand1 = MyBank.AddFundsToAccount(debitAccount, 5000);
            Assert.AreEqual(5000, debitAccount.Funds);
            MyBank.UndoCommand(addCommand1);
            Assert.AreEqual(0, debitAccount.Funds);

            MyBank.AddFundsToAccount(debitAccount, 5000);
            MyBank.TranferFundsBetweenAccounts(debitAccount, creditAccount, 2500);
            Assert.AreEqual(2500, creditAccount.Funds);
            MyBank.WithdrawFundsFromAccount(creditAccount, 9000);

            _timeManager.DissipateMonths(1);
            Assert.AreEqual(-21500, creditAccount.Funds);
            Assert.AreEqual(75216, Math.Round(depositAccount.Funds));
            Assert.AreEqual(2510, Math.Round(debitAccount.Funds));
        }

        [Test]
        public void SurpassTransactionLimit_RegisterUser_SatisfyTransactionLimit_ThrowException()
        {
            MyCentralBank.AddBank(MyBank);
            Customer customer1 = new CustomerBuilder().
                SetName("Sergey", "Yurpalov").
                Build();

            DebitAccount debitAccount = MyBank.CreateDebitAccount(customer1);
            MyBank.AddFundsToAccount(debitAccount, 20000);
            Assert.Catch<BanksException>(() =>
            {
                MyBank.WithdrawFundsFromAccount(debitAccount, 10000);
            });

            MyBank.VerifyCustomer(customer1, "123", "123");
            MyBank.WithdrawFundsFromAccount(debitAccount, 10000);
            Assert.AreEqual(10000, debitAccount.Funds);
        }

        [Test]
        public void SurpassCreditLimit_ChangeCreditLimitForBank_SatisfyCreditLimit_ThrowException()
        {
            MyCentralBank.AddBank(MyBank);
            Customer customer1 = new CustomerBuilder().
                SetName("Sergey", "Yurpalov").
                SetPassport("123").
                SetAddress("123").
                Build();

            CreditAccount creditAccount = MyBank.CreateCreditAccount(customer1);

            Assert.Catch<BanksException>(() =>
            {
                MyBank.WithdrawFundsFromAccount(creditAccount, 55000);
            });

            MyCentralBank.SetNewCreditLimitAmount(60000, MyBank);
            MyBank.WithdrawFundsFromAccount(creditAccount, 55000);
            Assert.AreEqual(-55000, creditAccount.Funds);
        }
    }
}