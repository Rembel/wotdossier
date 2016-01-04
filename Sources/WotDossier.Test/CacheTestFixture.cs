using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.BattleModeStrategies;
using WotDossier.Applications.Logic;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Test
{
    /// <summary>
    /// Dossier cache load tests
    /// </summary>
    public class CacheTestFixture : TestFixtureBase
    {
        [Test]
        public void CacheFilesTest()
        {
            //trick
            DataProvider.RollbackTransaction();
            DataProvider.CloseSession();

            //reset DB
            DatabaseManager.DeleteDatabase();
            DatabaseManager.InitDatabase();

            Player serverStatistic = new Player();
            serverStatistic.dataField = new PlayerData { account_id = 10800699, nickname = "_rembel__ru", created_at = 1349068892 };

            foreach (Version version in Dictionaries.Instance.Versions)
            {
                string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

                FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

                if (cacheFile != null)
                {
                    List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
                    foreach (TankJson tankJson in tanks)
                    {
                        string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                            tankJson.Description.Icon.IconId);
                        Assert.True(File.Exists(iconPath),
                            string.Format("Version: {1}. Can't find icon {0}", tankJson.Description.Icon.IconId, version));
                    }

                    foreach (BattleMode battleMode in Enum.GetValues(typeof (BattleMode)))
                    {
                        StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(battleMode,
                            DossierRepository);

                        PlayerEntity player = strategy.UpdatePlayerStatistic(
                            serverStatistic.dataField.account_id, tanks, serverStatistic);

                        var playerStatisticViewModel = strategy.GetPlayerStatistic(player, tanks, serverStatistic);
                        Assert.IsNotNull(playerStatisticViewModel);
                    }
                }
                else
                {
                    Console.WriteLine("Cache file not found: {0}", version);
                }
            }
        }

        [Test]
        public void NoobMeterPerformanceRatingAlgorithmTest()
        {
            Version version = Dictionaries.VersionRelease;

            string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

            FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

            if (cacheFile != null)
            {
                List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
                var performanceRating = RatingHelper.PerformanceRating(tanks, json => json.A15x15);
                Console.WriteLine(performanceRating);

            }
        }

        [Test]
        public void InternalCacheLoaderTest()
        {
            Version version = Dictionaries.VersionRelease;

            string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

            FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

            CacheFileHelper.InternalBinaryCacheToJson(cacheFile);
        }

        [Test]
        public void CacheFileTest()
        {
            Version version = new Version("0.9.13.0");
            const BattleMode battleMode = BattleMode.RandomCompany;

            //trick
            DataProvider.RollbackTransaction();
            DataProvider.CloseSession();

            Player serverStatistic = new Player();
            serverStatistic.dataField = new PlayerData
            {
                account_id = 10800699,
                nickname = "_rembel__ru",
                created_at = 1349068892
            };
            //reset DB
            DatabaseManager.DeleteDatabase();
            DatabaseManager.InitDatabase();

            string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

            FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png", tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("Version: {1}. Can't find icon {0}", tankJson.Description.Icon.IconId, version));
            }
            StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(battleMode, DossierRepository);

            PlayerEntity player = strategy.UpdatePlayerStatistic(serverStatistic.dataField.account_id, tanks,
                serverStatistic);

            var playerStatisticViewModel = strategy.GetPlayerStatistic(player, tanks, serverStatistic);
            Assert.IsNotNull(playerStatisticViewModel);
        }

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// null if there is no any dossier cache file for specified player
        /// </returns>
        public static FileInfo GetCacheFile(string playerId, string folder)
        {
            FileInfo cacheFile = null;

            string path = Environment.CurrentDirectory + folder;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.dat");

                if (!files.Any())
                {
                    return null;
                }

                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);

                    if (CacheFileHelper.GetPlayerName(info)
                        .Equals(playerId, StringComparison.InvariantCultureIgnoreCase))
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
            }
            return cacheFile;
        }
    }
}
