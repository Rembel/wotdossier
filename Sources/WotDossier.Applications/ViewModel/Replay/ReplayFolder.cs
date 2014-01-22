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

        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlIgnore]
        public string NameWithCount
        {
            get { return string.Format("{0}({1})", _name, Count); }
        }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlElement(ElementName = "folder", Type = typeof(ReplayFolder))]
        public ObservableCollection<ReplayFolder> Folders
        {
            get { return _folders; }
            set { _folders = value; }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("NameWithCount");
            }
        }

        [XmlAttribute("use-in-charts")]
        public bool UseInCharts
        {
            get { return _useInCharts; }
            set { _useInCharts = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Extensions
    {
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
