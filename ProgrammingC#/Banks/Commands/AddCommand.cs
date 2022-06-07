using Banks.Accounts;

namespace Banks.Commands
{
    public class AddCommand : ICommand
    {
        private readonly IAccount _account;
        private readonly decimal _fundsAmount;

        public AddCommand(IAccount account, decimal funds)
        {
            _account = account;
            _fundsAmount = funds;
        }

        public void Execute()
        {
            _account.AddFunds(_fundsAmount);
        }

        public void Undo()
        {
            _account.WithdrawFunds(_fundsAmount);
        }
    }
}