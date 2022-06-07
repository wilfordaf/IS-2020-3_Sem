using Banks.Accounts;

namespace Banks.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly IAccount _account;
        private readonly decimal _fundsAmount;

        public WithdrawCommand(IAccount account, decimal funds)
        {
            _account = account;
            _fundsAmount = funds;
        }

        public void Execute()
        {
            _account.WithdrawFunds(_fundsAmount);
        }

        public void Undo()
        {
            _account.AddFunds(_fundsAmount);
        }
    }
}