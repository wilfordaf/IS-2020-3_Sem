using Banks.Builders;
using Banks.Interfaces;
using Banks.Models;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            ICentralBank centralBank = new CentralBank();
            IBank bank = new Bank(
                100000,
                10000,
                5,
                50000,
                500);
            centralBank.AddBank(bank);
            var customer = new CustomerBuilder().
                SetName("Sergey", "Yurpalov").
                SetAddress("123").
                SetPassport("123").
                Build();

            new ConsoleInterface.ConsoleInterface(centralBank, bank, customer).Start();
        }
    }
}
