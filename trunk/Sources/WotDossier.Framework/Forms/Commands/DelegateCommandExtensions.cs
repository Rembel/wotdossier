using System.Windows.Input;

namespace WotDossier.Framework.Forms.Commands
{
    public static class DelegateCommandExtensions
    {
        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        /// <param name="command">The command.</param>
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