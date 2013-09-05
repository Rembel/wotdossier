using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Logging;
using WotDossier.Common;

namespace WotDossier.Applications
{
    public static class CacheHelper
    {
        private static readonly ILog _log = LogManager.GetLogger("DossierRepository");

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <returns>null if there is no any dossier cache file for specified player</returns>
        public static FileInfo GetCacheFile(string playerId)
        {
            FileInfo cacheFile = null;

            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(Folder.GetDossierCacheFolder(), "*.dat");
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.Error("Cann't find dossier cache files", ex);
            }

            if (!files.Any())
            {
                return null;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (GetPlayerName(info).Equals(playerId, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (cacheFile == null)
                    {
                        cacheFile = info;
                    }
                    else if (cacheFile.LastWriteTime < info.LastWriteTime)
                    {
                        cacheFile = info;
                    }
                }
            }
            return cacheFile;
        }

        /// <summary>
        /// Binary dossier cache to plain json.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public static void BinaryCacheToJson(FileInfo cacheFile)
        {
            string temp = Environment.CurrentDirectory;

            string directoryName = temp;
            Environment.CurrentDirectory = directoryName + @"\External";
            Process proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = directoryName + @"\External\wotdc2j.exe";
            proc.StartInfo.Arguments = string.Format("\"{0}\" -f", cacheFile.FullName);
            proc.Start();

            Environment.CurrentDirectory = temp;

            while (!proc.HasExited)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Binary dossier cache to plain json.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public static void ReplayToJson(FileInfo cacheFile)
        {
            string temp = Environment.CurrentDirectory;

            string directoryName = temp;
            Environment.CurrentDirectory = directoryName + @"\External";
            Process proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = directoryName + @"\External\wotrpbr2j.exe";
            proc.StartInfo.Arguments = string.Format("\"{0}\" -f -r", cacheFile.FullName);
            proc.Start();

            Environment.CurrentDirectory = temp;

            while (!proc.HasExited)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Gets the name of the player from name of dossier cache file.
        /// </summary>
        /// <param name="cacheFile">The cache file in base32 format. Example of decoded filename - login-ct-p1.worldoftanks.com:20015;_Rembel__RU</param>
        /// <returns></returns>
        public static string GetPlayerName(FileInfo cacheFile)
        {
            Base32Encoder encoder = new Base32Encoder();
            string str = cacheFile.Name.Replace(cacheFile.Extension, string.Empty);
            byte[] decodedFileNameBytes = encoder.Decode(str.ToLowerInvariant());
            string decodedFileName = Encoding.UTF8.GetString(decodedFileNameBytes);
            string playerName = decodedFileName.Split(';')[1];
            return playerName;
        }
    }
}
