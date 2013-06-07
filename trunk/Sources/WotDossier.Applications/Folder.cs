using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Applications
{
    public static class Folder
    {
        public static string GetDossierCacheFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dossierCacheFolder = appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache";
            return dossierCacheFolder;
        }

        public static string GetReplaysFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dossierCacheFolder = appDataPath + @"\Wargaming.net\WorldOfTanks\replays";
            return dossierCacheFolder;
        }
    }
}
