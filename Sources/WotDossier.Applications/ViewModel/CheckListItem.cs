using System;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    public class CheckListItem<TId> : ListItem<TId>
    {
        private readonly Action<CheckListItem<TId>, bool> _onCheckedChanged;
        public static readonly string PropChecked = TypeHelper<CheckListItem<TId>>.PropertyName(v => v.Checked);
        
        private bool _checked;
        private readonly CheckListItem<TId> _parentListItem;

        /// <summary>
        /// Gets or sets a value indicating whether item is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if checked; otherwise, <c>false</c>.
        /// </value>
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnPropertyChanged(PropChecked);

                if (_onCheckedChanged != null) _onCheckedChanged(this, Checked);
            }
        }

        public bool GroupCheck
        {
            set
            {
                _checked = value;
                OnPropertyChanged(PropChecked);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem{TId}" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="check">if set to <c>true</c> check item.</param>
        /// <param name="onCheckedChanged">The on checked changed.</param>
        public CheckListItem(TId id, string value, bool check, Action<CheckListItem<TId>, bool> onCheckedChanged)
            : this(id, value, check)
        {
            _onCheckedChanged = onCheckedChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem{TId}" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="check">if set to <c>true</c> check item.</param>
        /// <param name="onCheckedChanged">The on checked changed.</param>
        /// <param name="parentListItem">The parent list item.</param>
        public CheckListItem(TId id, string value, bool check, Action<CheckListItem<TId>, bool> onCheckedChanged, CheckListItem<TId> parentListItem)
            : this(id, value, check, onCheckedChanged)
        {
            _parentListItem = parentListItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="check">if set to <c>true</c> check item.</param>
        public CheckListItem(TId id, string value, bool check) : base(id, value)
        {
            _checked = check;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        public CheckListItem(TId id, string value) : base(id, value)
        {
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals(PropChecked))
            {
                if (_parentListItem != null && !Checked)
                {
                    _parentListItem.GroupCheck = false;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Checked: {1}", base.ToString(), Checked);
        }
    }
}