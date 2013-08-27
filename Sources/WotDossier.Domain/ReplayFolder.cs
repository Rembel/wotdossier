using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace WotDossier.Domain
{
    [Serializable]
    [XmlRoot("folder")]
    public class ReplayFolder
    {
        private ObservableCollection<ReplayFolder> _folders = new ObservableCollection<ReplayFolder>();

        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlElementAttribute(ElementName = "folder", Type = typeof(ReplayFolder))]
        public ObservableCollection<ReplayFolder> Folders
        {
            get { return _folders; }
            set { _folders = value; }
        }
    }
}
