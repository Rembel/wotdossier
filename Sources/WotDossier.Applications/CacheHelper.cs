using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace WotDossier.Applications
{
    public static class CacheHelper
    {
        private static readonly ILog _log = LogManager.GetLogger("DossierRepository");

        public static FileInfo GetCacheFile()
        {
            FileInfo cacheFile = null;

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache", "*.dat");
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.Error("Путь к файлам кэша не найден", ex);
            }

            if (!files.Any())
            {
                return null;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (cacheFile == null)
                {
                    cacheFile = info;
                }
                else if (cacheFile.LastWriteTime < info.LastWriteTime)
                {
                    cacheFile = info;
                }
            }
            return cacheFile;
        }

        public static void BinaryCacheToJson(FileInfo cacheFile)
        {
            string temp = Environment.CurrentDirectory;

            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = directoryName + @"\External";
            Process proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = directoryName + @"\External\wotdc2j.exe";
            proc.StartInfo.Arguments = string.Format("{0} -f -r", cacheFile.FullName);
            proc.Start();

            Environment.CurrentDirectory = temp;
        }
    }
}
