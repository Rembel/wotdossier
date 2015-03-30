using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Xml;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Chart;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework.Forms.ProgressDialog;
using Formatting = Newtonsoft.Json.Formatting;

namespace WotDossier.Test
{
    /// <summary>
    ///     Replays tests
    /// </summary>
    public class ReplaysTestFixture : TestFixtureBase
    {
        private List<ResourceManager> _resourceManagers;

        public ReplaysTestFixture()
        {
            _resourceManagers = new List<ResourceManager>();

            Assembly entryAssembly = GetType().Assembly;
            var resources = AssemblyExtensions.GetResourcesByMask(entryAssembly, ".resources");

            foreach (var resource in resources)
            {
                var resourceManager = new ResourceManager(resource.Replace(".resources", string.Empty), GetType().Assembly);
                _resourceManagers.Add(resourceManager);
            }
        }

        [Test]
        public void ReplaysByVersionTest()
        {
            foreach (Version version in Dictionaries.Instance.Versions)
            {
                ReplayTest(version);
            }
        }

        [TestCase("0.9.7.0")]
        public void ReplayTest(string version)
        {
            ReplayTest(new Version(version));
        }

        public void ReplayTest(Version version)
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", version.ToString(3));

            if (!Directory.Exists(replayFolder))
            {
                Assert.Fail("Folder not exists - [{0}]", replayFolder);
            }

            ReplaysFolderTest(replayFolder);
        }

        [Test]
        public void DeadCrewTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, @"Replays\CasesTest");
            ReplaysFolderTest(replayFolder);

            string[] replays = Directory.GetFiles(replayFolder, "*.wotreplay");

            foreach (string path in replays)
            {
                ReplayToJson(path);
            }
        }

        [Test]
        public void NotReadTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, @"Replays\CasesTest");
            ReplaysFolderTest(replayFolder);

            string[] replays = Directory.GetFiles(replayFolder, "*.wotreplay");

            foreach (string path in replays)
            {
                ReplayToJson(path);
            }
        }

        private static void ReplaysFolderTest(string replayFolder)
        {
            var replays = Directory.GetFiles(replayFolder, "*.wotreplay", SearchOption.AllDirectories);

            Console.WriteLine("Found: {0}", replays.Count());

            for (int index = 0; index < replays.Length; index++)
            {
                string fileName = replays[index];
                Console.WriteLine("Process[{0}]: {1}", index, fileName);

                FileInfo replayFile = new FileInfo(fileName);

                Replay replay = ReplayFileHelper.ParseReplay_8_11(replayFile, true);
                Assert.IsNotNull(replay, "Replay not parsed");
                Assert.IsNotNull(replay.datablock_battle_result, "Battle result not parsed");
                Assert.IsNotNull(replay.datablock_advanced, "Advanced data not parsed");

                var phisicalReplay = new PhisicalReplay(replayFile, replay, Guid.Empty);
                var mockView = new Mock<IReplayView>();
                ReplayViewModel model = new ReplayViewModel(mockView.Object);
                model.Init(phisicalReplay.ReplayData(true), phisicalReplay);
            }
        }

        [Test]
        public void ImportTanksComponentsXmlTest()
        {
            var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Tanks"), "shells.xml",
                SearchOption.AllDirectories);

            List<JObject> result = new List<JObject>();

            foreach (var xml in strings)
            {
                BigWorldXmlReader reader = new BigWorldXmlReader();
                FileInfo info = new FileInfo(xml);
                using (BinaryReader br = new BinaryReader(info.OpenRead()))
                {
                    var xmlContent = reader.DecodePackedFile(br, "shell");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonText);
                    dictionary = dictionary["shell"].ToObject<Dictionary<string, JObject>>();
                    dictionary.Remove("icons");

                    jsonText = JsonConvert.SerializeObject(dictionary, Formatting.Indented);

                    var path = Path.Combine(Environment.CurrentDirectory, "Tanks", info.Directory.Parent.Name + "_" + 
                                                                                info.Name.Replace(info.Extension, ".json"));
                    var stream = File.OpenWrite(path);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonText);
                    }

                }
            }
        }

        [Test]
        public void ImportTanksXmlTest()
        {
            var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Tanks"), "list.xml", SearchOption.AllDirectories);

            List<JObject> result = new List<JObject>();

            foreach (var xml in strings)
            {
                BigWorldXmlReader reader = new BigWorldXmlReader();
                FileInfo info = new FileInfo(xml);
                using (BinaryReader br = new BinaryReader(info.OpenRead()))
                {
                    var xmlContent = reader.DecodePackedFile(br, "vehicles");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonText);
                    dictionary = dictionary["vehicles"].ToObject<Dictionary<string, JObject>>();

                    //{"tankid": 0, "countryid": 0, "compDescr": 1, "active": 1, "type": 2, 
                    //"type_name": "MT", "tier": 5, "premium": 0, "title": "T-34", "icon": "r04_t_34", "icon_orig": "R04_T-34"},

                    List<JObject> tanks = new List<JObject>();
                    foreach (var tank in dictionary)
                    {
                        JObject tankDescription = new JObject();
                        var tankid = tank.Value["id"].Value<int>();
                        tankDescription["tankid"] = tankid;
                        Country country = (Country) Enum.Parse(typeof(Country), info.Directory.Name);
                        var countryid = (int)country;
                        tankDescription["countryid"] = countryid;
                        var typeCompDesc = Utils.TypeCompDesc(countryid, tankid);
                        tankDescription["compDescr"] = typeCompDesc;
                        var uniqueId = Utils.ToUniqueId(countryid, tankid);
                        tankDescription["active"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) & !Dictionaries.Instance.Tanks[uniqueId].Active ? 0 : 1;
                        var tankType = GetVehicleTypeByTag(tank.Value["tags"].Value<string>());
                        tankDescription["type"] = (int)tankType;
                        tankDescription["type_name"] = tankType.ToString();
                        tankDescription["tier"] = tank.Value["level"].Value<int>();
                        tankDescription["premium"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) ? Dictionaries.Instance.Tanks[uniqueId].Premium  : 
                        tank.Value["notInShop"] == null ? 0 : 1;
                        tankDescription["title"] = GetString(tank.Value["userString"].Value<string>().Split(':')[1]);
                        var titleShort = tank.Value["shortUserString"];
                        if (titleShort != null)
                        {
                            tankDescription["title_short"] = GetString(titleShort.Value<string>().Split(':')[1]);
                        }
                        var icon = tank.Key.Replace("-", "_").ToLower();
                        tankDescription["icon"] = icon;
                        tankDescription["icon_orig"] = tank.Key;

                        if (!Dictionaries.Instance.Tanks.ContainsKey(Utils.ToUniqueId(typeCompDesc)))
                        {
                            Console.WriteLine(tank.Value);
                        }

                        JObject tankDef = GetTankDefinition(countryid, tank.Key);

                        if (tankDef != null)
                        {
                            //Console.WriteLine(tankDef.ToString(Formatting.Indented));
                            tankDescription["health"] = tankDef.SelectToken("$.vehicles.hull.maxHealth").Value<int>() + tankDef.SelectTokens("$.vehicles.turrets0..maxHealth").Max(x => x.Value<int>());
                        }

                        tanks.Add(tankDescription);
                    }
                    result.AddRange(tanks);
                }
            }

            var serializeObject = JsonConvert.SerializeObject(result
                .OrderBy(x => GetOrder(x["countryid"].Value<int>()))
                .ThenBy(x => x["tankid"].Value<int>()));
            var tanksJson = serializeObject.Replace("{", "\n{").Replace(",\"", ", \"").Replace(":", ": ");

            var path = Path.Combine(Environment.CurrentDirectory, "Tanks", "tanks.json");
            var stream = File.OpenWrite(path);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(tanksJson);
            }

            Console.WriteLine(tanksJson);
        }

        private string GetString(string key)
        {
            var value = _resourceManagers.Select(x => x.GetString(key, CultureInfo.InvariantCulture)).FirstOrDefault(x => x != null);
            value = (value ?? key).Trim();
            if (value.Contains("PC방"))
            {
                value = value.Replace("PC방 ", "") + " IGR";
            }
            return value.Replace("ä", "a").Replace("ö", "o").Replace("ß", "ss").Replace("â", "a").Replace("ä","a");
        }

        private TankType GetVehicleTypeByTag(string tags)
        {
            if (tags.StartsWith("lightTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.LT;
            }
            if (tags.StartsWith("mediumTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.MT;
            }
            if (tags.StartsWith("heavyTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.HT;
            }
            if (tags.StartsWith("SPG", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.SPG;
            }
            if (tags.StartsWith("AT-SPG", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.TD;
            }
            return TankType.Unknown;
        }

        private JObject GetTankDefinition(int countryid, string tankName)
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, @"Tanks", ((Country)countryid).ToString(), tankName + ".xml");
            if (File.Exists(fileName))
            {
                var file = new FileInfo(fileName);
                BigWorldXmlReader reader = new BigWorldXmlReader();
                using (BinaryReader br = new BinaryReader(file.OpenRead()))
                {
                    var xmlContent = reader.DecodePackedFile(br, "vehicles");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    var value = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
                    return JsonConvert.DeserializeObject<JObject>(value);
                }
            }
            return null;
        }

        private int GetOrder(int value)
        {
            switch ((Country)value)
            {
                case Country.China:
                    return 0;
                case Country.Germany:
                    return 1;
                case Country.France:
                    return 2;
                case Country.Ussr:
                    return 3;
                case Country.Usa:
                    return 4;
                case Country.Uk:
                    return 5;
                case Country.Japan:
                    return 6;
            }
            return -1;
        }

        [Test]
        public void MapXmlTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, "Maps");

            if (!Directory.Exists(replayFolder))
            {
                Assert.Fail("Folder not exists - [{0}]", replayFolder);
            }

            var replays = Directory.GetFiles(replayFolder, "*.xml", SearchOption.AllDirectories);

            BigWorldXmlReader reader = new BigWorldXmlReader();

            JArray array = new JArray();

            foreach (var replay in replays)
            {
                FileInfo file = new FileInfo(replay);

                FileStream stream = new FileStream(replay, FileMode.Open, FileAccess.Read);
                using (BinaryReader br = new BinaryReader(stream))
                {
                    var xml = reader.DecodePackedFile(br, "map");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

                    var deserializeObject = JsonConvert.DeserializeObject<JObject>(jsonText);

                    var jToken = deserializeObject["map"];

                    var mapKey = file.Name.Replace(file.Extension, string.Empty);

                    if (Dictionaries.Instance.Maps.ContainsKey(mapKey))
                    {
                        var target = Dictionaries.Instance.Maps[mapKey];

                        JsonConvert.PopulateObject(jToken["boundingBox"].ToString(), target);
                    }

                    array.Add(jToken);
                }


            }

            foreach (var key in Dictionaries.Instance.Maps.Keys)
            {
                if (!File.Exists(Path.Combine(replayFolder, key + ".xml")))
                {
                    Console.WriteLine("Missed map: {0}", key);
                }
            }

            Console.WriteLine(array.ToString(Formatting.Indented));
        }

        //[Test]
        //public void AdvancedReplayTest()
        //{
        //    FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.9.1\14003587093213_ussr_Object_140_el_hallouf.wotreplay"));
        //    StopWatch watch = new StopWatch();
        //    watch.Reset();
        //    Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile, true);
        //    Console.WriteLine(watch.PeekMs());

        //    string serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
        //    serializeObject.Dump(cacheFile.FullName + "_1");

        //    watch.Reset();
        //    replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, true);
        //    Console.WriteLine(watch.PeekMs());

        //    serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
        //    serializeObject.Dump(cacheFile.FullName + "_2");
        //}

        [Test]
        public void ReplaysFoldersSaveLoadTest()
        {
            ReplayFolder folder = new ReplayFolder { Name = "Parent", Path = "c:\\Parent", Folders = new ObservableCollection<ReplayFolder> { new ReplayFolder { Name = "Child", Path = "c:\\Child" } } };
            string xml = XmlSerializer.StoreObjectInXml(folder);
            Console.WriteLine(xml);

            ReplayFolder replayFolder = XmlSerializer.LoadObjectFromXml<ReplayFolder>(xml);

            Console.WriteLine(replayFolder.Folders.Count);
        }

        [Test]
        public void ReplayToJsonTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", new Version("0.8.1").ToString(3));

            if (!Directory.Exists(replayFolder))
            {
                Assert.Fail("Folder not exists - [{0}]", replayFolder);
            }

            string[] replays = Directory.GetFiles(replayFolder, "*.wotreplay");

            foreach (string path in replays)
            {
                ReplayToJson(path);
            }
        }

        [Test]
        public void ParserMigrationTest()
        {
            string externalparser = @"ExternalParser\";
            string internalparser = @"InternalParser\";

            if (!Directory.Exists(externalparser))
            {
                Directory.CreateDirectory(externalparser);
            }
            if (!Directory.Exists(internalparser))
            {
                Directory.CreateDirectory(internalparser);
            }

            foreach (Version version in Dictionaries.Instance.Versions)
            {
                string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", version.ToString(3));

                if (Directory.Exists(replayFolder))
                {
                    var replays = Directory.GetFiles(replayFolder, "*.wotreplay", SearchOption.AllDirectories);

                    Console.WriteLine("Found: {0}", replays.Count());

                    for (int index = 0; index < replays.Length; index++)
                    {
                        string path = replays[index];

                        FileInfo replayFile = new FileInfo(path);
                        var replay = ReplayFileHelper.ParseReplay_8_0(replayFile);
                        OrderList(replay);
                        var serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
                        serializeObject.Dump(externalparser + version.ToString(3) + ".json");
                        Console.WriteLine(serializeObject);
                        replay = ReplayFileHelper.ParseReplay_8_11(replayFile);
                        OrderList(replay);
                        serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
                        serializeObject.Dump(internalparser + version.ToString(3) + ".json");
                        Console.WriteLine(serializeObject);
                    }
                }
                else
                {
                    Console.WriteLine("Folder not exists - [{0}]", replayFolder);
                }
            }
        }

        private void OrderList(Replay replay)
        {
            replay.datablock_1.vehicles = replay.datablock_1.vehicles.OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Value);

            if (replay.datablock_battle_result != null)
            {
                replay.datablock_battle_result.personal.details = replay.datablock_battle_result.personal.details.OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Value);

                replay.datablock_battle_result.players = replay.datablock_battle_result.players.OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Value);

                replay.datablock_battle_result.vehicles = replay.datablock_battle_result.vehicles.OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Value);
            }
        }

        private static void ReplayToJson(string path)
        {
            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var replay = new Replay();

                    try
                    {
                        int blocksCount = 0;
                        var buffer = new byte[4];
                        stream.Read(buffer, 0, 4);

                        if (buffer[0] != 0x21)
                        {
                            blocksCount = (int)stream.Read(4).ConvertLittleEndian();
                            Console.WriteLine("Found Replay Blocks: " + blocksCount);
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            var blockLength = (int)stream.Read(4).ConvertLittleEndian();
                            Console.WriteLine("{0} block length: {1}", i + 1, blockLength);
                            byte[] blockData = stream.Read(blockLength);

                            //read first block
                            if (i == 0)
                            {
                                string firstBlock = ReplayFileHelper.ReadFirstBlock(blockData);
                                replay.datablock_1 = JsonConvert.DeserializeObject<FirstBlock>(firstBlock);
                                Console.WriteLine(
                                    JsonConvert.DeserializeObject<JObject>(firstBlock).ToString(Formatting.Indented));
                            }

                            //read second block
                            if (i == 1)
                            {
                                DateTime playTime = DateTime.Parse(replay.datablock_1.dateTime,
                                    CultureInfo.GetCultureInfo("ru-RU"));
                                Version version = ReplayFileHelper.ResolveVersion(replay.datablock_1.Version, playTime);

                                if (version < new Version("0.8.11.0") && blocksCount < 3)
                                {
                                    object thirdBlock = ReplayFileHelper.ReadThirdBlock(blockData);
                                    Console.WriteLine(JsonConvert.SerializeObject(thirdBlock, Formatting.Indented));
                                }
                                else
                                {
                                    JArray secondBlock = ReplayFileHelper.ReadSecondBlock(blockData);
                                    Console.WriteLine(secondBlock.ToString(Formatting.Indented));
                                }
                            }

                            //read third block for replays 0.8.1-0.8.10
                            if (i == 2)
                            {
                                object thirdBlock = ReplayFileHelper.ReadThirdBlock(blockData);
                                Console.WriteLine(JsonConvert.SerializeObject(thirdBlock, Formatting.Indented));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error on replay file read. Incorrect file format({0})", e, path);
                    }
                }
            }
        }

        /// <summary>
        /// process only changed replays files, full processing only on app start
        /// </summary>
        [Test]
        public void FolderProcessingTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", new Version("0.8.5").ToString(3));
            ReplaysViewModel replaysViewModel = new ReplaysViewModel(DossierRepository, new ProgressControlViewModel(), new PlayerChartsViewModel());
            var mockView = new Mock<IReporter>();
            mockView.Setup(x => x.Report(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<int, string, object[]>((percentProgress, format, arg) => Console.WriteLine(format, arg));
            List<ReplayFolder> replayFolders = new List<ReplayFolder> { new ReplayFolder { Path = replayFolder, Folders = new ObservableCollection<ReplayFolder>() } };
            replaysViewModel.ReplaysFolders = replayFolders;
            StopWatch stopWatch = new StopWatch();
            replaysViewModel.ProcessReplaysFolders(replayFolders, mockView.Object);
            Console.WriteLine(stopWatch.Peek());
            stopWatch.Reset();
            replaysViewModel.ProcessReplaysFolders(replayFolders, mockView.Object);
            Console.WriteLine(stopWatch.Peek());
        }
    }
}