using System.ComponentModel.Composition;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Applications.Services;
using WotDossier.Framework.Forms;

namespace WotDossier.Framework.Presentation.Services
{
    /// <summary>
    /// This is the default implementation of <see cref="IMessageService"/>. It shows messages via the MessageBox to the user.
    /// </summary>
    /// <remarks>
    /// If the default implementation of this service doesn't serve your need then you can provide your own implementation.
    /// </remarks>
    [Export(typeof(IMessageService))]
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            WpfMessageBox.Show(message, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Default);
        }

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowWarning(string message)
        {
            WpfMessageBox.Show(message, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Warning);
        }

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowError(string message)
        {
            WpfMessageBox.Show(message, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Error);
        }
    }
}
