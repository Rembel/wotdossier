using System.ComponentModel;
using System.Windows.Threading;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// Represents a view
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Occurs directly after IView.Close() is called, and can be handled to cancel view closure.
        /// </summary>
        event CancelEventHandler Closing;

        /// <summary>
        /// Occurs when a view becomes the foreground window.
        /// </summary>
        event System.EventHandler Activated;

        /// <summary>
        /// Occurs when the element is laid out, rendered, and ready for interaction.
        /// </summary>
        event System.Windows.RoutedEventHandler Loaded;

        /// <summary>
        /// Gets or sets the data context of the view.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Gets the System.Windows.Threading.Dispatcher this view is associated with.
        /// </summary>
        /// <value>
        /// The dispatcher.
        /// </value>
        Dispatcher Dispatcher { get; }

        /// <summary>
        /// Opens a view and returns without waiting for the newly opened view to close.
        /// </summary>
        void Show();

        /// <summary>
        /// Opens a View and returns only when the newly opened view is closed.
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// Manually closes a view
        /// </summary>
        void Close();
    }
}
