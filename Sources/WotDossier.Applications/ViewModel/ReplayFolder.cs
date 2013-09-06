﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    [Serializable]
    [XmlRoot("folder")]
    public class ReplayFolder
    {
        private ObservableCollection<ReplayFolder> _folders = new ObservableCollection<ReplayFolder>();
        private ObservableCollection<ReplayFile> _files = new ObservableCollection<ReplayFile>();

        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlElement(ElementName = "folder", Type = typeof(ReplayFolder))]
        public ObservableCollection<ReplayFolder> Folders
        {
            get { return _folders; }
            set { _folders = value; }
        }

        [XmlIgnore]
        public ObservableCollection<ReplayFile> Files
        {
            get { return _files; }
            set { _files = value; }
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