using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    public class ListItem<TId> : INotifyPropertyChanged
    {
        public static readonly string PropId = TypeHelper<ListItem<TId>>.PropertyName(v => v.Id);
        public static readonly string PropValue = TypeHelper<ListItem<TId>>.PropertyName(v => v.Value);

        private TId _id;
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public TId Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(PropId);
            }
        }

        private string _value;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(PropValue);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ListItem(TId id, string value)
        {
            _id = id;
            _value = value;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(ListItem<TId> other)
        {
            return EqualityComparer<TId>.Default.Equals(_id, other._id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" }, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ListItem<TId>) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(_id);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}