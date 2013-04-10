using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Tank;

namespace WotDossier.Test
{
    [TestFixture]
    public class TestFixtureBase
    {
        private DataProvider _dataProvider;

        [Import(typeof(DataProvider))]
        public DataProvider DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; }
        }

        [TestFixtureSetUp]
        public void Init()
        {
            CompositionContainerFactory.Instance.Container.SatisfyImportsOnce(this);
        }

        [SetUp]
        public virtual void SetUp()
        {

            DataProvider.OpenSession();
            DataProvider.BeginTransaction();
        }

        [TearDown]
        public virtual void TearDown()
        {
            // DataProvider.CommitTransaction();
            DataProvider.RollbackTransaction();
            DataProvider.CloseSession();
        }

        /// <summary>
        /// Binary dossier cache to plain json.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public static void BinaryCacheToJson(FileInfo cacheFile)
        {
            string temp = Environment.CurrentDirectory;

            string directoryName = Environment.CurrentDirectory;
            Environment.CurrentDirectory = directoryName + @"\External";
            Process proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = directoryName + @"\External\wotdc2j.exe";
            proc.StartInfo.Arguments = string.Format("{0} -f -r", cacheFile.FullName);
            proc.Start();

            Environment.CurrentDirectory = temp;
        }

        [Test]
        public void CacheTest_085()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.5\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            //WotApiClient.Instance.
        }

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <returns>null if there is no any dossier cache file for specified player</returns>
        public static FileInfo GetCacheFile(string playerId, string folder)
        {
            FileInfo cacheFile = null;

            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(Environment.CurrentDirectory + folder, "*.dat");
            }
            catch (DirectoryNotFoundException ex)
            {
                //_log.Error("Cann't find dossier cache files", ex);
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
