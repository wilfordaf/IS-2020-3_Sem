using Banks.Tools;

namespace Banks.Commands
{
    public class CommandInvoker
    {
        private ICommand _currentCommand;

        internal CommandInvoker() { }

        internal void SetCommand(ICommand command)
        {
            _currentCommand = command;
        }

        internal void RunCommand()
        {
            if (_currentCommand is null)
            {
                throw new BanksException();
            }

            _currentCommand.Execute();
            Refresh();
        }

        internal void UndoCommand()
        {
            if (_currentCommand is null)
            {
                throw new BanksException();
            }

            _currentCommand.Undo();
            Refresh();
        }

        private void Refresh()
        {
            _currentCommand = null;
        }
    }
}