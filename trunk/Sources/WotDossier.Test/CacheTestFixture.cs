using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.BattleModeStrategies;
using WotDossier.Applications.Update;
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
            Player player1 = new Player();
            player1.dataField = new PlayerData { account_id = 10800699, nickname = "_rembel__ru", created_at = 1349068892 };
            ServerStatWrapper serverStatistic = new ServerStatWrapper(player1);

            foreach (Version version in Dictionaries.Instance.Versions)
            {
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

                StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(BattleMode.RandomCompany, DossierRepository);

                PlayerEntity player = strategy.UpdatePlayerStatistic(serverStatistic.Player.dataField.account_id, tanks, serverStatistic);

                var playerStatisticViewModel = strategy.GetPlayerStatistic(player, tanks, serverStatistic);
            }
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

            string[] files = Directory.GetFiles(Environment.CurrentDirectory + folder, "*.dat");

            if (!files.Any())
            {
                return null;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (CacheFileHelper.GetPlayerName(info).Equals(playerId, StringComparison.InvariantCultureIgnoreCase))
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
    }
}
