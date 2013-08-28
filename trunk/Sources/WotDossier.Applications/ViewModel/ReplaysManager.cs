using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    [Export]
    public class ReplaysManager
    {
        private const string REPLAYS_CATALOG_FILE_PATH = @"Data\ReplaysCatalog.xml";

        public void SaveFolder(ReplayFolder replayFolder)
        {
            using (StreamWriter writer = File.CreateText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                writer.WriteLine(XmlSerializer.StoreObjectInXml(replayFolder));
                writer.Flush();
            }
        }

        public List<ReplayFolder> GetFolders()
        {
            using (StreamReader streamReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                string tree = streamReader.ReadToEnd();
                return new List<ReplayFolder> {XmlSerializer.LoadObjectFromXml<ReplayFolder>(tree)};
            }
        }

        public void Move(ReplayFile replayFile, ReplayFolder targetFolder)
        {
            string destFileName = Path.Combine(targetFolder.Path, replayFile.FileInfo.Name);
            if (!File.Exists(destFileName))
            {
                replayFile.FileInfo.MoveTo(destFileName);
            }
        }
    }
}