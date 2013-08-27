using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel
{
    [Export]
    public class ReplaysManager
    {
        private const string _dataReplayscatalogXml = @"Data\ReplaysCatalog.xml";

        public void SaveFolder(ReplayFolder replayFolder)
        {
            using (StreamWriter writer = File.CreateText(Path.Combine(Environment.CurrentDirectory, _dataReplayscatalogXml)))
            {
                writer.WriteLine(XmlSerializer.StoreObjectInXml(replayFolder));
                writer.Flush();
            }
        }

        public List<ReplayFolder> GetFolders()
        {
            using (StreamReader streamReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, _dataReplayscatalogXml)))
            {
                string tree = streamReader.ReadToEnd();
                return new List<ReplayFolder> {XmlSerializer.LoadObjectFromXml<ReplayFolder>(tree)};
            }
        }
    }
}