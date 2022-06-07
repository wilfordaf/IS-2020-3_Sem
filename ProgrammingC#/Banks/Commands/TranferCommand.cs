using Banks.Accounts;

namespace Banks.Commands
{
    public class TranferCommand : ICommand
    {
        private readonly IAccount _accountToWithdraw;
        private readonly IAccount _accountToAdd;
        private readonly decimal _fundsAmount;

        public TranferCommand(IAccount accountToWithdraw, IAccount accountToAdd, decimal funds)
        {
            _accountToWithdraw = accountToWithdraw;
            _accountToAdd = accountToAdd;
            _fundsAmount = funds;
        }

        public void Execute()
        {
            _accountToWithdraw.WithdrawFunds(_fundsAmount);
            _accountToAdd.AddFunds(_fundsAmount);
        }

        public void Undo()
        {
            _accountToWithdraw.AddFunds(_fundsAmount);
            _accountToAdd.WithdrawFunds(_fundsAmount);
        }
    }
}