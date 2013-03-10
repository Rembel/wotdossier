using System.Windows.Input;

namespace WotDossier.Framework.Forms.Commands
{
    public static class DelegateCommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand command)
        {
            var delegateCommand = command as IDelegateCommand;
            if(delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
            }
        }
    }
}