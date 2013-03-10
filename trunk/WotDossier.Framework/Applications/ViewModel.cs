using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WotDossier.Framework.Foundation;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    public abstract class ViewModel : Model
    {
        private readonly List<PropertyChangedEventListener> _propertyChangedListeners = new List<PropertyChangedEventListener>();
        private readonly List<CollectionChangedEventListener> _collectionChangedListeners = new List<CollectionChangedEventListener>();

        protected ViewModel()
        {
        }

        /// <summary>
        /// Adds a weak event listener for a PropertyChanged event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="handler">The event handler.</param>
        /// <exception cref="ArgumentNullException">source must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">handler must not be <c>null</c>.</exception>
        protected void AddWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }
            
            PropertyChangedEventListener listener = new PropertyChangedEventListener(source, handler);

            _propertyChangedListeners.Add(listener);

            PropertyChangedEventManager.AddListener(source, listener, "");
        }

        /// <summary>
        /// Removes the weak event listener for a PropertyChanged event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="handler">The event handler.</param>
        /// <exception cref="ArgumentNullException">source must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">handler must not be <c>null</c>.</exception>
        protected void RemoveWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }
            
            PropertyChangedEventListener listener = _propertyChangedListeners.LastOrDefault(l => 
                l.Source == source && l.Handler == handler);

            if (listener != null)
            {
                _propertyChangedListeners.Remove(listener);
                PropertyChangedEventManager.RemoveListener(source, listener, "");
            }
        }

        /// <summary>
        /// Adds a weak event listener for a CollectionChanged event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="handler">The event handler.</param>
        /// <exception cref="ArgumentNullException">source must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">handler must not be <c>null</c>.</exception>
        protected void AddWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }
            
            CollectionChangedEventListener listener = new CollectionChangedEventListener(source, handler);

            _collectionChangedListeners.Add(listener);

            CollectionChangedEventManager.AddListener(source, listener);
        }

        /// <summary>
        /// Removes the weak event listener for a CollectionChanged event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="handler">The event handler.</param>
        /// <exception cref="ArgumentNullException">source must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">handler must not be <c>null</c>.</exception>
        protected void RemoveWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }

            CollectionChangedEventListener listener = _collectionChangedListeners.LastOrDefault(l =>
                l.Source == source && l.Handler == handler);

            if (listener != null)
            {
                _collectionChangedListeners.Remove(listener);
                CollectionChangedEventManager.RemoveListener(source, listener);
            }
        }
    }
}
