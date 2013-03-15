using System.Windows.Threading;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// Represents a view
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets or sets the data context of the view.
        /// </summary>
        object DataContext { get; set; }
        Dispatcher Dispatcher { get; }
    }
}
