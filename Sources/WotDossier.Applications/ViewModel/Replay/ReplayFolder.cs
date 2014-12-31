using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace WotDossier.Applications.ViewModel.Replay
{
    [Serializable]
    [XmlRoot("folder")]
    public class ReplayFolder : INotifyPropertyChanged
    {
        private ObservableCollection<ReplayFolder> _folders = new ObservableCollection<ReplayFolder>();
        private Guid _id;
        private int _count;
        private string _name;
        private bool _useInCharts = true;
        private List<string> _files;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [XmlAttribute("id")]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets the name with count.
        /// </summary>
        /// <value>
        /// The name with count.
        /// </value>
        [XmlIgnore]
        public string NameWithCount
        {
            get { return string.Format("{0}({1})", _name, Count); }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [XmlAttribute("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        [XmlElement(ElementName = "folder", Type = typeof(ReplayFolder))]
        public ObservableCollection<ReplayFolder> Folders
        {
            get { return _folders; }
            set { _folders = value; }
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [XmlIgnore]
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("NameWithCount");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use in charts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use in charts]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("use-in-charts")]
        public bool UseInCharts
        {
            get { return _useInCharts; }
            set { _useInCharts = value; }
        }

        [XmlIgnore]
        public List<string> Files
        {
            get { return _files ?? new List<string>(0); }
            set { _files = value; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Gets all folders recursively.
        /// </summary>
        /// <param name="list">The plain folders list.</param>
        /// <returns></returns>
        public static List<ReplayFolder> GetAll(this List<ReplayFolder> list)
        {
            List<ReplayFolder> result = new List<ReplayFolder>();

            result.AddRange(list);
            foreach (var folder in list)
            {
                result.AddRange(folder.Folders.ToList().GetAll());
            }

            return result;
        }
    }
}
