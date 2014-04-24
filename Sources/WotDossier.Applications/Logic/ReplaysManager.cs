using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;

namespace WotDossier.Applications.Logic
{
    [Export]
    public class ReplaysManager
    {
        private const string REPLAYS_CATALOG_FILE_PATH = @"Data\ReplaysCatalog.xml";

        /// <summary>
        /// Saves the folder.
        /// </summary>
        /// <param name="replayFolder">The replay folder.</param>
        public void SaveFolder(ReplayFolder replayFolder)
        {
            using (StreamWriter writer = File.CreateText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                writer.WriteLine(XmlSerializer.StoreObjectInXml(replayFolder));
                writer.Flush();
            }
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <returns></returns>
        public List<ReplayFolder> GetFolders()
        {
            using (StreamReader streamReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                string tree = streamReader.ReadToEnd();
                return new List<ReplayFolder> {InitFolder(tree)};
            }
        }

        private static ReplayFolder InitFolder(string tree)
        {
            ReplayFolder folder = XmlSerializer.LoadObjectFromXml<ReplayFolder>(tree);
            return folder;
        }
    }
}