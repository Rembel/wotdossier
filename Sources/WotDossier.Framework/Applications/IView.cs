using System.ComponentModel;
using System.Windows.Threading;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// Represents a view
    /// </summary>
    public interface IView
    {
        event CancelEventHandler Closing;
        event System.EventHandler Activated;
        event System.Windows.RoutedEventHandler Loaded;

        /// <summary>
        /// Gets or sets the data context of the view.
        /// </summary>
        object DataContext { get; set; }
        Dispatcher Dispatcher { get; }

        void Show();
        bool? ShowDialog();
        void Close();
    }
}
