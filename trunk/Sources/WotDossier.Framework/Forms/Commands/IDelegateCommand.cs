namespace WotDossier.Framework.Forms.Commands
{
    public interface IDelegateCommand
    {
        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}