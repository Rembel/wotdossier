using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain.Tank;

namespace WotDossier.Test
{
    /// <summary>
    /// Dossier cache load tests
    /// </summary>
    public class CacheTestFixture : TestFixtureBase
    {
        [Test]
        public void CacheTest_085()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.5\");
            
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_086()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.6\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_087()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.7\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_088()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.8\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_089()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_0810()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.10\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_0811()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.11\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_090()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.9.0\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_091()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.9.1\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_092()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.9.2\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_093()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.9.3\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                    tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));

            foreach (var group in tanks.GroupBy(x => x.Common.type))
            {
                Console.WriteLine();
                Console.WriteLine("Battles - " + @group.Sum(x => x.A15x15.battlesCount));

                foreach (TankJson tankJson in @group.OrderBy(x => x.Common.type).ThenByDescending(x => x.A15x15.battlesCount))
                {
                    Console.WriteLine("15x15-{1}\t\tclan-{2}\tcompany-{3}\t7x7-{4}\t\tTank - {0} - {5}", tankJson.Common.tanktitle, GetCount(tankJson.A15x15), GetCount(tankJson.Clan), GetCount(tankJson.Company), GetCount(tankJson.A7x7), tankJson.Common.basedonversion);
                }
            }

            Console.WriteLine("battles count cache -" + tanks.Sum(x => x.A15x15.battlesCount));
            Console.WriteLine("last battle -" + tanks.Max(x => x.Common.lastBattleTimeR));
            Console.WriteLine("tanks count cache - " + tanks.Count);

            string tanksRequest = new Uri(
                "http://api.worldoftanks.ru/wot/account/tanks/?application_id=19779cdf6e8aab1a8c99e1261274f13b&access_token=ab2994337c9f5b09d0b7f372001f17c45f0b4af8&account_id=1749450")
                .Get();

            var parsedData = JsonConvert.DeserializeObject<JObject>(tanksRequest);

            JArray array = (JArray)parsedData["data"]["1749450"];
            Console.WriteLine("tanks count server - " + array.Count());
            Console.WriteLine("battles count server - " + array.Sum(x => x["statistics"]["all"]["battles"].Value<int>()));

            foreach (TankJson tankJson in tanks)
            {
                TankServerInfo serverInfo = Dictionaries.Instance.ServerTanks.Values.FirstOrDefault(x => x.name.EndsWith(tankJson.Description.Icon.IconOrig));

                if (serverInfo != null)
                {
                    JToken token = array.FirstOrDefault(x => x["tank_id"].Value<int>() == serverInfo.tank_id);
                    if (token == null)
                    {
                        Console.WriteLine(tankJson.Description.Title);
                    }
                }
            }
        }

        private string GetCount(StatisticJson a15X15)
        {
            if (a15X15 != null)
            {
                return a15X15.battlesCount.ToString("D3");
            }
            return "000";
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

            files = Directory.GetFiles(Environment.CurrentDirectory + folder, "*.dat");

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
