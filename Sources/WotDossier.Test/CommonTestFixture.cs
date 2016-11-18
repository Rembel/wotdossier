using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Formatting = Newtonsoft.Json.Formatting;

namespace WotDossier.Test
{
    [TestFixture]
    public class CommonTestFixture : TestFixtureBase
    {
        private string clientPath = @"I:\World_of_Tanks_CT";

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
                ratingExpectancy.CompDescr = tankDescription.CompDescr;

                xmlTanks.Add(ratingExpectancy);
            }

            foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            {
                if (xmlTanks.FirstOrDefault(x => string.Equals(x.Icon, description.Icon.IconOrig, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    Console.WriteLine(description.Icon.IconOrig);
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(xmlTanks.OrderBy(x => x.TankTitle), Formatting.Indented));
        }

        [Test]
        public void CsvExportProviderTest()
        {
            CsvExportProvider provider = new CsvExportProvider();
            FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            List<RandomBattlesTankStatisticRowViewModel> list = tanks.Select(x => new RandomBattlesTankStatisticRowViewModel(x, new List<StatisticSlice>())).ToList();
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
        public void MedalsResourcesTest()
        {
            var dictionary = GetResourcesDictionary();
            
            foreach (var medal in Dictionaries.Instance.Medals)
            {
                var localizedString = Resources.Resources.ResourceManager.GetString(medal.Value.NameResourceId);
                Assert.IsNotNull(localizedString, "Resource not found: {0}", medal.Value.NameResourceId);

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
                Assert.IsNotNull(localizedString, "Resource not found: {0}", map.Value.MapNameId);

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
                else
                {
                    File.Copy(Environment.CurrentDirectory + "\\..\\..\\..\\WotDossier.Resources\\" + key.Replace("/", "\\"), string.Format(@"d:\1\{0}.png", tank.Value.Icon.IconId));
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
        public void ImportTanksComponentsXmlTest()
        {
            var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Tanks"), "shells.xml",
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

                    var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals\shells");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = Path.Combine(path, info.Directory.Parent.Name + "_" + info.Name.Replace(info.Extension, ".json"));

                    var stream = File.OpenWrite(path.ToLower());
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
            EnshureScriptsCopied();

            CopyGameTextResources();

            Console.WriteLine("Copy tanks definitions");

            var scriptsPath = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Scripts");
            var destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Tanks");
            var source = Path.Combine(scriptsPath, @"item_defs\vehicles");

            Directory.CreateDirectory(destination);

            DirectoryCopy(source, destination, true);

            var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Tanks"), "list.xml", SearchOption.AllDirectories);

            List<JObject> result = new List<JObject>();

            StringBuilder codegen = new StringBuilder();

            Dictionary<string, string> resources = new Dictionary<string, string>();

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
                        Country country = (Country)Enum.Parse(typeof(Country), info.Directory.Name);
                        var countryid = (int)country;
                        tankDescription["countryid"] = countryid;
                        var typeCompDesc = Utils.TypeCompDesc(countryid, tankid);
                        tankDescription["compDescr"] = typeCompDesc;
                        var uniqueId = Utils.ToUniqueId(countryid, tankid);
                        tankDescription["active"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) && !Dictionaries.Instance.Tanks[uniqueId].Active ? 0 : 1;
                        var tankType = GetVehicleTypeByTag(tank.Value["tags"].Value<string>());
                        tankDescription["type"] = (int)tankType;
                        tankDescription["type_name"] = tankType.ToString();
                        tankDescription["tier"] = tank.Value["level"].Value<int>();
                        tankDescription["premium"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) ? Dictionaries.Instance.Tanks[uniqueId].Premium :
                        tank.Value["notInShop"] == null ? 0 : 1;
                        var key = tank.Value["userString"].Value<string>().Split(':')[1];
                        var value = GetString(key);
                        tankDescription["title"] = value;
                        var titleShort = tank.Value["shortUserString"];
                        if (titleShort != null)
                        {
                            tankDescription["title_short"] = GetString(titleShort.Value<string>().Split(':')[1]);
                        }
                        var icon = tank.Key.Replace("-", "_").ToLower();

                        if (countryid == 0)
                        {
                            resources.Add(icon, value);
                        }

                        tankDescription["icon"] = icon;
                        tankDescription["icon_orig"] = tank.Key;

                        if (!Dictionaries.Instance.Tanks.ContainsKey(uniqueId))
                        {
                            Console.WriteLine(tank.Value);
                        }
                        else
                        {
                            var description = Dictionaries.Instance.Tanks[uniqueId];
                            if (description.Icon.Icon != (string)tankDescription["icon"])
                            {
                                string f = @"else if (iconId == ""{0}_{1}"")
                                {{
                                    //{4} replay tank name changed to {2}
                                    tankDescription = tankDescriptions[{3}];
                                }}";
                                codegen.AppendFormat(f, country.ToString().ToLower(), description.Icon.Icon, tankDescription["icon"], uniqueId, Dictionaries.VersionRelease);
                                codegen.AppendLine();
                            }
                        }

                        JObject tankDef = GetTankDefinition(countryid, tank.Key);

                        if (tankDef != null)
                        {
                            //Console.WriteLine(tankDef.ToString(Formatting.Indented));
                            var health = tankDef.SelectToken("$.vehicles.hull.maxHealth").Value<int>() + tankDef.SelectTokens("$.vehicles.turrets0..maxHealth").First().Value<int>();
                            tankDescription["health"] = health;
                        }

                        tanks.Add(tankDescription);
                    }
                    result.AddRange(tanks);
                }
            }

            using (ResXResourceWriter writer = new ResXResourceWriter(Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Resources\ussr_vehicles_out.resx")))
            {
                foreach (var resource in resources)
                {
                    writer.AddResource(resource.Key, resource.Value);
                }
            }

            var serializeObject = JsonConvert.SerializeObject(result
                .OrderBy(x => GetOrder(x["countryid"].Value<int>()))
                .ThenBy(x => x["tankid"].Value<int>()));
            var tanksJson = serializeObject.Replace("{", "\n{").Replace(",\"", ", \"").Replace(":", ": ");

            var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, "tanks.json");

            var stream = File.OpenWrite(path);
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(tanksJson);
            }

            Console.WriteLine(tanksJson);
            Console.WriteLine(codegen);
        }

        private string GetString(string key)
        {
            var value = ResourceManagers.Select(x => x.GetString(key, CultureInfo.InvariantCulture)).FirstOrDefault(x => x != null);
            value = (value ?? key).Trim();
            if (value.Contains(@"PC방"))
            {
                value = value.Replace(@"PC방 ", "") + " IGR";
            }
            return value.Replace("ä", "a").Replace("ö", "o").Replace("ß", "ss").Replace("â", "a").Replace("ä", "a");
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
            var fileName = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Tanks", ((Country)countryid).ToString(), tankName + ".xml");
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
                case Country.Czech:
                    return 7;
                case Country.Sweden:
                    return 8;
            }
            return -1;
        }

        [Test]
        public void ImportMapsTest()
        {
            string configsPath = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Maps");

            if (!Directory.Exists(configsPath))
            {
                Assert.Fail("Folder not exists - [{0}]", configsPath);
            }

            var files = Directory.GetFiles(configsPath, "*.xml", SearchOption.AllDirectories);

            BigWorldXmlReader reader = new BigWorldXmlReader();

            JArray array = new JArray();

            foreach (var configFile in files)
            {
                FileInfo file = new FileInfo(configFile);

                FileStream stream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
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
                if (!File.Exists(Path.Combine(configsPath, key + ".xml")))
                {
                    Console.WriteLine("Missed map: {0}", key);
                }
            }

            var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, "maps_description.json");

            var outputStream = File.OpenWrite(path);
            var mapsJson = array.ToString(Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(outputStream))
            {
                writer.Write(mapsJson);
            }

            Console.WriteLine(mapsJson);
        }

        [Test]
        public void UpdateToPatch()
        {
            string destination;
            string source;

            EnshureScriptsCopied();

            ImportTanksXmlTest();

            Console.WriteLine("Copy maps definitions");

            destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Maps");

            var scriptsPath = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\scripts");
            source = Path.Combine(scriptsPath, @"arena_defs");

            Directory.CreateDirectory(destination);

            DirectoryCopy(source, destination, true);

            ImportMapsTest();

            Console.WriteLine("Copy tanks components");
            
            ImportTanksComponentsXmlTest();

            destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Images");

            string filepath = Path.Combine(clientPath, @"res\packages\gui.pkg");
            using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
            {
                var achievement = @"gui/maps/icons/achievement";
                zip.ExtractSelectedEntries("name = *.*", achievement, destination, ExtractExistingFileAction.OverwriteSilently);

                var achievementsDestinationPath = Path.Combine(destination, "achievement");

                if (Directory.Exists(achievementsDestinationPath))
                {
                    Directory.Delete(achievementsDestinationPath, true);
                }

                Directory.Move(Path.Combine(destination, achievement), achievementsDestinationPath);

                var vehicle = @"gui/maps/icons/vehicle";
                zip.ExtractSelectedEntries("name = *.*", vehicle, destination, ExtractExistingFileAction.OverwriteSilently);
                var vehiclesPath = Path.Combine(destination, @"vehicle");

                if (Directory.Exists(vehiclesPath))
                {
                    Directory.Delete(vehiclesPath, true);
                }

                Directory.Move(Path.Combine(destination, vehicle), vehiclesPath);

                var files = Directory.GetFiles(vehiclesPath);

                foreach (var file in files)
                {
                    FileInfo info = new FileInfo(file);
                    var destFileName = file.Replace("-", "_");
                    if (!File.Exists(destFileName))
                    {
                        info.MoveTo(destFileName);
                    }
                }
            }


        }

        private void EnshureScriptsCopied()
        {
            var destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch");
            var scriptsPath = Path.Combine(destination, @"scripts");

            if (!Directory.Exists(scriptsPath))
            {
                string filepath = Path.Combine(clientPath, @"res\packages\scripts.pkg");
                using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
                {
                    zip.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        private void CopyGameTextResources()
        {
            string destination;
            string source;
            Console.WriteLine("Copy resources");

            destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Resources");
            source = Path.Combine(clientPath, @"res\text\lc_messages");

            Directory.CreateDirectory(destination);

            var strings = Directory.GetFiles(source, "*_vehicles.mo");

            foreach (var resourceFile in strings)
            {
                FileInfo info = new FileInfo(resourceFile);
                info.CopyTo(Path.Combine(destination, info.Name), true);
            }

            string result;
            using (var proc = new Process())
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.FileName = Path.Combine(destination, Path.Combine(Environment.CurrentDirectory, @"Tools\convert.bat"));

                proc.StartInfo.WorkingDirectory = destination;

                proc.Start();

                result = proc.StandardOutput.ReadToEnd();

                Console.WriteLine(result);

                //write log
                proc.WaitForExit();
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subdir.Name));
                    DirectoryCopy(subdir.FullName, temppath, true, overwrite);
                }
            }
        }

        [Test]
        public void GetClanInfoTest()
        {
            var appSettings = SettingsReader.Get();
            WotApiClient.Instance.GetClanMemberInfo(3016489, appSettings);
        }
/*
        [Test]
        public void GenMapsImagesWithBases()
        {
            foreach (var map in Dictionaries.Instance.Maps)
            {
                var mapDescription = map.Value;
                var outFileName = mapDescription.LocalizedMapName + ".png";

                var replayMap = new ReplayMap
                {
                    Gameplay = Gameplay.ctf,
                    MapId = mapDescription.MapId,
                    MapName = mapDescription.MapName,
                    MapNameId = mapDescription.MapNameId,
                    Team = 1
                };

                var mapImage = (BitmapImage)MapToMinimapImageConverter.Default.Convert(replayMap, null, null, CultureInfo.InvariantCulture);

                MapElementContext elementContext = new MapElementContext(mapDescription, replayMap.Gameplay.ToString(), 1, 300, 300);

                var mapImageElements = elementContext.GetMapImageElements();

                Bitmap bitmap = new Bitmap(Convert.ToInt32(300), Convert.ToInt32(300), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics mainImage = Graphics.FromImage(bitmap);

                mainImage.DrawImage(GetImage(mapImage),  new Point(0, 0));

                foreach (var mapImageElement in mapImageElements)
                {
                    var baseImage = (BitmapImage)MapImageElementToIconConverter.Default.Convert(mapImageElement, null, null, CultureInfo.InvariantCulture);

                    mainImage.DrawImage(GetImage(baseImage), new Point((int) mapImageElement.X, (int) mapImageElement.Y));
                }

                bitmap.Save(outFileName, ImageFormat.Png);
            }
        }

        private Graphics ToGraphics(BitmapImage bitmapImage)
        {
            var image = GetImage(bitmapImage);

            return Graphics.FromImage(image);
        }

        private static Image GetImage(BitmapImage bitmapImage)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);
            stream.Position = 0;

            var image = System.Drawing.Image.FromStream(stream);
            return image;
        }

        private class ReplayMap : IReplayMap
        {
            public Gameplay Gameplay { get; set; }
            public string MapName { get; set; }
            public int MapId { get; set; }
            public string MapNameId { get; set; }
            public int Team { get; set; }
        }*/
    }
}