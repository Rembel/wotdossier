using System;
using System.IO;
using System.Reflection;

namespace WotDossier.Dal
{
    public static class Folder
    {
        public static string GetDossierCacheFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dossierCacheFolder = appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache";
            return dossierCacheFolder;
        }

        public static string GetDossierAppDataFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dossierCacheFolder = appDataPath + @"\WotDossier";
            if (!Directory.Exists(dossierCacheFolder))
            {
                try
                {
                    Directory.CreateDirectory(dossierCacheFolder);
                }
                catch (Exception e)
                {
                    
                }
            }
            return dossierCacheFolder;
        }

        public static string AssemblyDirectory()
        {
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }
    }
}
