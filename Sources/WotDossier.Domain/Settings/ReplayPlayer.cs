using System;
using System.ComponentModel;

namespace WotDossier.Domain.Settings
{
    public class ReplayPlayer : INotifyPropertyChanged
    {
        private const string DefaultVersion = "0.0";
        private string _path;
        private Version _version;
        private string _stringVersion;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        //[XmlIgnore]
        public Version Version
        {
            get { return _version ?? new Version(StringVersion); }
            set
            {
                _version = value;
                StringVersion = _version.ToString();
                OnPropertyChanged("Version");
            }
        }

        //[XmlElement("Version")]
        public string StringVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_stringVersion))
                {
                    _stringVersion = DefaultVersion;
                }
                return _stringVersion;
            }
            set { _stringVersion = value; }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
