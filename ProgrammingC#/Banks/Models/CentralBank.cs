using System.Collections.Generic;
using Banks.Accounts;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class CentralBank : ICentralBank
    {
        private readonly List<IBank> _banks = new ();

        public CentralBank() { }

        public void AddBank(IBank bank)
        {
            _banks.Add(bank);
        }

        public void RemoveBank(IBank bank)
        {
            if (!_banks.Remove(bank))
            {
                throw new BanksException("Such bank was not found.");
            }
        }

        public void SetNewTransactionLimitForUnregisteredCustomer(decimal limit, IBank bank)
        {
            bank.ChangeTransactionLimitForUnregisteredCustomer(limit);
            bank.NotifyAllCustomers($"Transaction limit For unregistered customer was changed to {limit}");
        }

        public void SetNewTransactionLimitForRegisteredCustomer(decimal limit, IBank bank)
        {
            bank.ChangeTransactionLimitForRegisteredCustomer(limit);
            bank.NotifyAllCustomers($"Transaction limit For registered customer was changed to {limit}");
        }

        public void SetNewDebitAnnualInterestOnBalance(decimal interest, IBank bank)
        {
            bank.ChangeDebitAnnualInterestOnBalance(interest);
            bank.NotifySpecificCustomers(
                AccountTypes.DebitAccount,
                $"Annual interest on balance was changed to {interest}");
        }

        public void SetNewCreditLimitAmount(decimal limit, IBank bank)
        {
            bank.ChangeCreditLimitAmount(limit);
            bank.NotifySpecificCustomers(
                AccountTypes.CreditAccount,
                $"Credit limit amount was changed to {limit}");
        }

        public void SetNewCreditFee(decimal fee, IBank bank)
        {
            bank.ChangeCreditFee(fee);
            bank.NotifySpecificCustomers(
                AccountTypes.CreditAccount,
                $"Credit fee was changed to {fee}");
        }

        public void NotifyBanksDayPassed()
        {
            _banks.ForEach(b => b.UpdateDay());
        }

        public void NotifyBanksMonthPassed()
        {
            _banks.ForEach(b => b.UpdateMonth());
        }
    }
}