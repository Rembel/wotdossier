using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Formatting = Newtonsoft.Json.Formatting;

namespace WotDossier.Test
{
    [TestFixture]
    public class CommonTestFixture : TestFixtureBase
    {
        [Test]
        //TODO: move to api 2.0
        public void LoginTest()
        {
            string url = "https://api.worldoftanks.ru/auth/create/api/1.0/?source_token=WG-WoT_Assistant-1.3.2";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.Method = WebRequestMethods.Http.Post;
            request.CookieContainer = new CookieContainer();
            request.ContentType = "application/x-www-form-urlencoded";

            // Перед тем как заполнять поля формы, текст запроса конвертируем в байты            
            byte[] ByteQuery = Encoding.ASCII.GetBytes("login=<>&password=<>");
            // Длинна запроса (обязательный параметр)
            request.ContentLength = ByteQuery.Length;
            // Открываем поток для записи
            Stream QueryStream = request.GetRequestStream();
            // Записываем в поток (это и есть POST запрос(заполнение форм))
            QueryStream.Write(ByteQuery, 0, ByteQuery.Length);
            // Закрываем поток
            QueryStream.Close();

            WebResponse webResponse = request.GetResponse();
            using (Stream stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string readToEnd = streamReader.ReadToEnd();
                }
            }
        }

        [Test]
        public void MultipleUploadTest()
        {
            FileInfo info = new FileInfo(@"Replays\20140325_2258_ussr-Object_140_84_winter.wotreplay");
            ReplayUploader uploader = new ReplayUploader();
            WotReplaysSiteResponse response = uploader.Upload(info, 10800699, "_rembel_");
            Console.WriteLine(response.Error);
        }

        [Test]
        public void NominalDamageTest()
        {
            XmlDocument document = new XmlDocument();
            document.Load(@"Data\TankNominalDamage.xml");
            XmlNodeList nodes = document.SelectNodes("/damage/tr");

            List<RatingExpectancy> xmlTanks = new List<RatingExpectancy>();

            foreach (XmlNode node in nodes)
            {
                XmlNodeList values = node.SelectNodes("td");
                XmlNode tankLevel = values[3];
                XmlNode tankType = values[4];
                XmlNode nominalDamage = values[5];
                XmlNode wn8NominalDamage = values[6];
                XmlNode wn8NominalWinRate = values[7];
                XmlNode wn8NominalSpotted = values[8];
                XmlNode wn8NominalFrags = values[9];
                XmlNode wn8NominalDefence = values[10];
                XmlNode href = values[1].SelectSingleNode("a/@href");
                XmlNode title = values[1].SelectSingleNode("a");

                RatingExpectancy ratingExpectancy = new RatingExpectancy();
                ratingExpectancy.PRNominalDamage = double.Parse(nominalDamage.InnerText.Replace(",", ""));
                if (!string.IsNullOrEmpty(wn8NominalDamage.InnerText))
                {
                    ratingExpectancy.TankLevel = int.Parse(tankLevel.InnerText);
                    ratingExpectancy.TankType = (TankType)Enum.Parse(typeof(TankType), tankType.InnerText);
                    ratingExpectancy.Wn8NominalDamage = double.Parse(wn8NominalDamage.InnerText.Replace(",", ""));
                    ratingExpectancy.Wn8NominalWinRate = double.Parse(wn8NominalWinRate.InnerText.Replace("%", ""));
                    ratingExpectancy.Wn8NominalSpotted = double.Parse(wn8NominalSpotted.InnerText.Replace(".", ","));
                    ratingExpectancy.Wn8NominalFrags = double.Parse(wn8NominalFrags.InnerText.Replace(".", ","));
                    ratingExpectancy.Wn8NominalDefence = double.Parse(wn8NominalDefence.InnerText.Replace(".", ","));
                }
                ratingExpectancy.Icon = href.InnerText.Replace("/tank/eu/", "");
                ratingExpectancy.TankTitle = title.InnerText.Trim();
                
                xmlTanks.Add(ratingExpectancy);
            }

            foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            {
                if (xmlTanks.FirstOrDefault(x => string.Equals(x.Icon, description.Icon.IconOrig, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    Console.WriteLine(description.Icon.IconOrig);
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(xmlTanks.OrderBy(x => x.Icon), Formatting.Indented));
        }

        [Test]
        public void NominalDamageTest2()
        {
            JObject expectedValues;
            using (StreamReader re = new StreamReader(@"Data\expected_tank_values.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                expectedValues = se.Deserialize<JObject>(reader);
            }

            List<RatingExpectancy> xmlTanks = new List<RatingExpectancy>();
            foreach (var expectedValue in expectedValues["data"])
            {
                int idNum = (int) expectedValue["IDNum"];

                if (Dictionaries.Instance.Tanks.Values.FirstOrDefault(x => x.CompDescr == idNum) == null)
                {
                    expectedValue["tankId"] = idNum >> 8 & 65535;
                    expectedValue["countryId"] = idNum >> 4 & 15;
                    Console.WriteLine(expectedValue);
                    continue;
                }

                var tankDescription = Dictionaries.Instance.Tanks.Values.First(x => x.CompDescr == idNum);

                RatingExpectancy ratingExpectancy = new RatingExpectancy();
                double prNominalDamage = (double) expectedValue["expDamage"];
                ratingExpectancy.PRNominalDamage = tankDescription.Expectancy.PRNominalDamage;
                ratingExpectancy.TankLevel = tankDescription.Tier;
                ratingExpectancy.TankType = (TankType) tankDescription.Type;
                ratingExpectancy.Wn8NominalDamage = prNominalDamage;
                ratingExpectancy.Wn8NominalWinRate = (double)expectedValue["expWinRate"];
                ratingExpectancy.Wn8NominalSpotted = (double)expectedValue["expSpot"];
                ratingExpectancy.Wn8NominalFrags = (double)expectedValue["expFrag"];
                ratingExpectancy.Wn8NominalDefence = (double)expectedValue["expDef"];
                ratingExpectancy.Icon = tankDescription.Icon.IconOrig;
                ratingExpectancy.TankTitle = tankDescription.Title;

                xmlTanks.Add(ratingExpectancy);
            }

            foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            {
                if (xmlTanks.FirstOrDefault(x => string.Equals(x.Icon, description.Icon.IconOrig, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    Console.WriteLine(description.Icon.IconOrig);
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(xmlTanks.OrderBy(x => x.Icon), Formatting.Indented));
        }

        [Test]
        public void NominalDamageTest3()
        {
            JArray expectedValues;
            using (StreamReader re = new StreamReader(@"External\tanks_expectations.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                expectedValues = se.Deserialize<JArray>(reader);
            }

            Console.WriteLine(JsonConvert.SerializeObject(expectedValues.OrderBy(x => x["icon"].Value<string>()).ToArray(), Formatting.Indented));
        }

        [Test]
        public void CsvExportProviderTest()
        {
            CsvExportProvider provider = new CsvExportProvider();
            FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            List<RandomBattlesTankStatisticRowViewModel> list = tanks.Select(x => new RandomBattlesTankStatisticRowViewModel(x)).ToList();
            Console.WriteLine(provider.Export(list, new List<Type>{typeof(IStatisticBattles), typeof(IStatisticFrags)}));
        }

        [Test]
        public void appSpotTest()
        {
            AppSpotUploader uploader = new AppSpotUploader();
            FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            long id = uploader.Upload(cacheFile);
            uploader.Update(cacheFile, id);
        }

        [Test]
        public void DynamicTest()
        {
            var dictionary = new Dictionary<string, int>();
            dictionary["Field"] = 23;
            dictionary["Field1"] = 23;
            var serializeObject = JsonConvert.SerializeObject(dictionary, Formatting.Indented);

            dynamic o = new {Field = 23, Field1 = 23};

            Console.WriteLine(serializeObject);

            serializeObject = JsonConvert.SerializeObject(o, Formatting.Indented);
            Console.WriteLine("-------------------------");
            Console.WriteLine(serializeObject);
        }

        [Test]
        public void MedalsResourcesTest()
        {
            var dictionary = GetResourcesDictionary();
            
            foreach (var medal in Dictionaries.Instance.Medals)
            {
                var localizedString = Resources.Resources.ResourceManager.GetString(medal.Value.NameResourceId);
                Assert.IsNotNullOrEmpty(localizedString, "Resource not found: {0}", medal.Value.NameResourceId);

                string key = string.Format("images/medals/{0}.png", medal.Value.Icon).ToLowerInvariant();
                Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", medal.Value.Icon);
            }
        }

        [Test]
        public void MapsResourcesTest()
        {
            var dictionary = GetResourcesDictionary();

            foreach (var map in Dictionaries.Instance.Maps)
            {
                var localizedString = Resources.Resources.ResourceManager.GetString("Map_" + map.Value.MapNameId);
                Assert.IsNotNullOrEmpty(localizedString, "Resource not found: {0}", map.Value.MapNameId);

                var key = string.Format("images/maps/{0}.jpg", map.Value.MapNameId).ToLowerInvariant();
                //Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", map.Value.mapidname);
                if (!dictionary.ContainsKey(key))
                {
                    Console.WriteLine("Image resource not found: {0}", map.Value.MapNameId);
                }
            }
        }

        private static Dictionary<string, object> GetResourcesDictionary()
        {
            var assembly = Assembly.Load("WotDossier.Resources");
            Stream fs = assembly.GetManifestResourceStream("WotDossier.Resources.g.resources");
            var rr = new System.Resources.ResourceReader(fs);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (DictionaryEntry entry in rr)
            {
                dictionary.Add(entry.Key.ToString().ToLowerInvariant(), entry.Value);
            }
            return dictionary;
        }

        [Test]
        public void TanksResourcesTest()
        {
            var dictionary = GetResourcesDictionary();

            foreach (var tank in Dictionaries.Instance.Tanks)
            {
                var key = string.Format("images/tanks/{0}.png", tank.Value.Icon.IconId).ToLowerInvariant();
                //Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", tank.Value.Icon.IconId);
                if (!dictionary.ContainsKey(key))
                {
                    Console.WriteLine("Image resource not found: {0}", tank.Value.Icon.IconId);
                }
            }
        }

        [Test]
        public void EncodeCacheFileName()
        {
            string server = "login.p7.worldoftanks.net";
            string playerName = "LayneksII";
            var encodFileName = CacheFileHelper.EncodFileName(server, playerName);
            Console.WriteLine(encodFileName);
            Console.WriteLine(CacheFileHelper.DecodFileName(encodFileName));
            Console.WriteLine(CacheFileHelper.DecodFileName("NRXWO2LOFZYDOLTXN5ZGYZDPMZ2GC3TLOMXG4ZLUHIZDAMB.dat"));
            
        }

        [Test]
        public void GeneratePostgreInsertStatements()
        {
            SQLiteConnection connection = null;

            using (connection = GetConnection())
            {
                SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", connection);

                command.CommandType = CommandType.Text;

                Dictionary<string, List<string>> tables = new Dictionary<string, List<string>>();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add((string)reader["name"], new List<string>());
                    }
                }

                foreach (var table in tables.Keys)
                {
                    command = new SQLiteCommand(string.Format("PRAGMA table_info('{0}');",table), connection);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables[table].Add((string)reader["name"]);
                        }
                    }
                }

                /*'INSERT INTO PlayerStatistic (Id, PlayerId, Updated, Wins, Losses, SurvivedBattles, Xp, BattleAvgXp, MaxXp, Frags, Spotted, HitsPercents, DamageDealt, CapturePoints, DroppedCapturePoints, BattlesCount, Rating_IntegratedValue'
 +', Rating_IntegratedPlace, Rating_BattleAvgPerformanceValue, Rating_BattleAvgPerformancePlace, Rating_BattleAvgXpValue, Rating_BattleAvgXpPlace, Rating_BattleWinsValue, Rating_BattleWinsPlace, Rating_BattlesValue'
+', Rating_BattlesPlace, Rating_CapturedPointsValue, Rating_CapturedPointsPlace, Rating_DamageDealtValue, Rating_DamageDealtPlace, Rating_DroppedPointsValue, Rating_DroppedPointsPlace, Rating_FragsValue, Rating_FragsPlace'
+', Rating_SpottedValue, Rating_SpottedPlace, Rating_XpValue, Rating_XpPlace, AvgLevel, AchievementsId, Rating_HitsPercentsValue, Rating_HitsPercentsPlace, Rating_MaxXpValue, Rating_MaxXpPlace'
+', RBR, WN8Rating, PerformanceRating, DamageTaken, MaxFrags, MaxDamage, MarkOfMastery, UId, PlayerUId, AchievementsUId, Rev) values ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12)', 
				[statistic.Id, statistic.PlayerId, statistic.Updated, statistic.Wins, statistic.Losses, statistic.SurvivedBattles, statistic.Xp, statistic.BattleAvgXp, statistic.MaxXp, statistic.Frags, statistic.Spotted, 
	statistic.HitsPercents, statistic.DamageDealt, statistic.CapturePoints, statistic.DroppedCapturePoints, statistic.BattlesCount, statistic.Rating_IntegratedValue, statistic.Rating_IntegratedPlace, statistic.Rating_BattleAvgPerformanceValue, 
	statistic.Rating_BattleAvgPerformancePlace, statistic.Rating_BattleAvgXpValue, statistic.Rating_BattleAvgXpPlace, statistic.Rating_BattleWinsValue, statistic.Rating_BattleWinsPlace, statistic.Rating_BattlesValue,
	statistic.Rating_BattlesPlace, statistic.Rating_CapturedPointsValue, statistic.Rating_CapturedPointsPlace, statistic.Rating_DamageDealtValue, statistic.Rating_DamageDealtPlace, statistic.Rating_DroppedPointsValue, 
	statistic.Rating_DroppedPointsPlace, statistic.Rating_FragsValue, statistic.Rating_FragsPlace, statistic.Rating_SpottedValue, statistic.Rating_SpottedPlace, statistic.Rating_XpValue, statistic.Rating_XpPlace, statistic.AvgLevel, 
	statistic.AchievementsId, statistic.Rating_HitsPercentsValue, statistic.Rating_HitsPercentsPlace, statistic.Rating_MaxXpValue, statistic.Rating_MaxXpPlace, statistic.RBR, statistic.WN8Rating, statistic.PerformanceRating, statistic.DamageTaken, 
	statistic.MaxFrags, statistic.MaxDamage, statistic.MarkOfMastery, statistic.UId, statistic.PlayerUId, statistic.AchievementsUId, statistic.Rev]*/

                foreach (var table in tables)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("'INSERT INTO {0} ", table.Key);
                    builder.AppendFormat("({0}) ", string.Join(",", table.Value));
                    builder.AppendFormat("VALUES ({0})'", string.Join(",", Enumerable.Range(0, table.Value.Count).Select(x => "$" + (x+1))));
                    builder.AppendFormat(",[{0}]", string.Join(",", table.Value.Select(x => table.Key.ToLower() + "." + x).ToList()));
                    Console.WriteLine(builder);
                    Console.WriteLine();
                }
            }
        }

        private SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Data/dossier.s3db;Version=3;");
            connection.Open();
            return connection;
        }
    }
}