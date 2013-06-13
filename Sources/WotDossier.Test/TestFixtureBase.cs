using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
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


        /*Добавлены новые немецкие танки:
            VK 20.01 (D) (средний 4-го уровня);
            VK 30.01 (D) (средний 6-го уровня);
            Aufklarerpanzer Panther (лёгкий 7-го уровня);
            Indien Panzer (средний 8-го уровня);
            Leopard Prototype der Arbeitsgruppe A (средний 9-го уровня);
            Leopard 1 (средний 10-го уровня);

        Добавлены новые советские танки:
            Т-60 (лёгкий 2-го уровня);
            Т-70 (лёгкий 3-го уровня);
            Т-80 (лёгкий 4-го уровня);
         
        Американская премиум САУ Sexton 3-го уровня переведена в британское дерево и добавлена в магазин для продажи.

        Для тестирования супертестерами добавлены танки:
            британский тяжёлый премиум танк 5-го уровня A33 Excelsior;
            советский специальный танк 10-го уровня «Объект 907»;
            американский специальный средний танк 10-го уровня М60;
            немецкий специальный тяжёлый танк 10-го уровня VK7201.*/
        [Test]
        public void CacheTest_085()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.5\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png", tankJson.Icon.iconid);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Icon.iconid));
            }
        }

        [Test]
        public void CacheTest_086()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.6\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png", tankJson.Icon.iconid);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Icon.iconid));
            }
        }

        [Test]
        public void ReplayTest_084()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_086()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.6\20130612_0912_germany-E-100_28_desert.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
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

        [Test]
        public void ReplayFileTest()
        {
            ReplayFile replayFile = new ReplayFile(new FileInfo(@"C:\20130329_2326_ussr-IS-3_19_monastery.wotreplay"));
            replayFile = new ReplayFile(new FileInfo(@"C:\20130202_1447_usa-T1_hvy_29_el_hallouf.wotreplay"));
        }

        [Test]
        public void LoadMapsImages()
        {
            List<Map> maps = WotApiClient.ReadMaps();

            foreach (var map in maps)
            {
                string url = "http://wotreplays.ru/img/results/Maps/" + map.mapidname + ".png";

                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception e)
                {
                    Console.WriteLine(map.mapidname);
                    continue;
                }
                Stream responseStream = response.GetResponseStream();

                if (responseStream != null)
                {
                    using (var streamReader = new BinaryReader(responseStream))
                    {
                        Byte[] lnByte = streamReader.ReadBytes(1 * 1024 * 1024 * 10);
                        using (FileStream destinationFile = File.Create(Path.Combine(Environment.CurrentDirectory, map.mapidname + ".png")))
                        {
                            destinationFile.Write(lnByte, 0, lnByte.Length);
                        }
                    }
                }
            }
        }

        [Test]
        public void LoginTest()
        {
            string url = "https://api.worldoftanks.ru/auth/create/api/1.0/?source_token=WG-WoT_Assistant-1.3.2";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
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
        public void UploadTest()
        {
            FileInfo info = new FileInfo(@"C:\Documents and Settings\YaroshikPV\AppData\Roaming\Wargaming.net\WorldOfTanks\replays\20121111_1414_ussr-KV-1s_13_erlenberg.wotreplay");

            ReplayUploader uploader = new ReplayUploader();

            uploader.Upload(info, "replay1", "replayDescription1", "http://wotreplays.ru/site/upload");
        }
    }
}
