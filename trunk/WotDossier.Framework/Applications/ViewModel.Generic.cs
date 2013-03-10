using System;
using System.Threading;
using System.Windows.Threading;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class ViewModel<TView> : ViewModel where TView : IView
    {
        private readonly TView _view;

        protected ViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(TView view) : this(view, false) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> then this object is a child of another ViewModel.</param>
        protected ViewModel(TView view, bool isChild)
        {
            if (view == null) { throw new ArgumentNullException("view"); }
            _view = view;
            if (!isChild)
            {
                // Check if the code is running within the WPF application model
                if (SynchronizationContext.Current is DispatcherSynchronizationContext)
                {
                    // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                    // before the view starts with DataBinding.
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                    {
                        view.DataContext = this;
                    });
                }
                else
                {
                    // When the code runs outside of the WPF application model then we expect that this constructor is called
                    // within a unit test. Therefore, we don't need the Dispatcher here.
                    view.DataContext = this;
                }
            }
        }


        /// <summary>
        /// Gets the associated view as specified view type.
        /// </summary>
        /// <remarks>
        /// Use this property in a ViewModel subclass to avoid casting.
        /// </remarks>
        public TView ViewTyped { get { return _view; } }
    }
}
