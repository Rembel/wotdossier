using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.Update;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Test
{
    [TestFixture]
    public class TestFixtureBase
    {
        private DataProvider _dataProvider;
        private DossierRepository _dossierRepository;

        [Import(typeof(DataProvider))]
        public DataProvider DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; }
        }

        [Import(typeof(DossierRepository))]
        public DossierRepository DossierRepository
        {
            get { return _dossierRepository; }
            set { _dossierRepository = value; }
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

        #region Dossier cache load tests

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
            List<TankJson> tanks = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_086()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.6\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_087()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.7\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_088()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.8\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_089()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanksV2)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_0810()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.10\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanksV2)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        [Test]
        public void CacheTest_0811()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.11\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanksV2)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png",
                                                tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Description.Icon.IconId));
            }
        }

        #endregion
        
        #region Replays tests

        [Test]
        public void ReplayTest_084()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_086()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.6\20130612_0912_germany-E-100_28_desert.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_087()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.7\20130706_1009_ussr-T-54_73_asia_korea.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_088()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.8\20130908_2025_usa-M103_14_siegfried_line.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_089()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.9\20131016_0035_ussr-Object263_37_caucasus.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_0810()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.10\20131208_0156_ussr-Object_140_53_japan.wotreplay"));
            CacheHelper.ReplayToJson(cacheFile);
            Replay replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
        }

        [Test]
        public void ReplayTest_0811()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                                          @"Replays\0.8.11\20140126_2109_ussr-T-54_14_siegfried_line.wotreplay"));

            Replay replay = WotApiClient.Instance.ReadReplay2Blocks(cacheFile);
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
            CacheHelper.ReplayToJson(cacheFile);
            replay = WotApiClient.Instance.ReadReplay(cacheFile.FullName.Replace(cacheFile.Extension, ".json"));
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
        }

        [Test]
        public void ReplayTest()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            Replay replay = WotApiClient.Instance.ReadReplay2Blocks(cacheFile);
        }

        #endregion

        #region Help methods

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

        #endregion
        
        [Test]
        public void LoadMapsImages()
        {
            List<Map> maps = Dictionaries.ReadMaps().Values.ToList();

            foreach (var map in maps)
            {
                string url = "http://wotreplays.ru/img/results/Maps/" + map.mapidname + ".png";

                WebRequest request = HttpWebRequest.Create(url);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception)
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
        public void LoadTanksImages()
        {
            List<TankDescription> tanks = Dictionaries.Instance.Tanks.Values.ToList();

            foreach (var tank in tanks)
            {
                string url = string.Format("http://www.vbaddict.net/wot/tanks/{0}_{1}.png", tank.Icon.CountryId, tank.Icon.Icon);

                WebRequest request = HttpWebRequest.Create(url);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception)
                {
                    Console.WriteLine(tank.Title);
                    continue;
                }
                Stream responseStream = response.GetResponseStream();

                if (responseStream != null)
                {
                    using (var streamReader = new BinaryReader(responseStream))
                    {
                        Byte[] lnByte = streamReader.ReadBytes(1 * 1024 * 1024 * 10);
                        using (FileStream destinationFile = File.Create(Path.Combine(Environment.CurrentDirectory, string.Format("{0}_{1}.png", tank.CountryCode, tank.Icon.Icon))))
                        {
                            destinationFile.Write(lnByte, 0, lnByte.Length);
                        }
                    }
                }
            }
        }

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
        public void UploadTest()
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));

            ReplayUploader uploader = new ReplayUploader();

            uploader.Upload(info, "replay1", "replayDescription1", "http://wotreplays.ru/site/upload");
            string url = "http://wotreplays.ru/site/upload";
            Uri uri = new Uri(url);
            CookieContainer cookieContainer = ReplayUploader.LoadCookies(url);
            foreach (Cookie coockie in cookieContainer.GetCookies(uri))
            {
                string s = HttpUtility.UrlDecode(coockie.Value);
            }
        }

        //[Test]
        //public void Base64ToFile()
        //{
        //    string credits =
        //        "iVBORw0KGgoAAAANSUhEUgAAABMAAAAXCAYAAADpwXTaAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyRpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo5QUU0N0I4RjUzRkUxMUUyODREOUZENzBDNkEyNjEzNiIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDo5QUU0N0I5MDUzRkUxMUUyODREOUZENzBDNkEyNjEzNiI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjlBRTQ3QjhENTNGRTExRTI4NEQ5RkQ3MEM2QTI2MTM2IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjlBRTQ3QjhFNTNGRTExRTI4NEQ5RkQ3MEM2QTI2MTM2Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+8bcsCgAAAwFJREFUeNpi/P//PwO1ABMxiuaFhZUSZRrIZSA8i5OfF8aG4emcPByzvX0TTm5c9x9dDhsmqGD39Om/Hz9+8L9KRo6DkFqC3lR0cWJhYGJmkPHxnExRmM0MCMxkYWdj+PrhI4OOp2cKNjWzeAR4sRo2l0dQEJmv4OE66c2NGwyHFsxnkNHTZqhVVhUMY2YRnMErwAFTk/blw2cYmxHk12ZdXTUpVjaOf/8Z/jz88+uPGgcXz5M/f374L5x79eL8hVu3b9xUV7dr29njCxYy3N20hUGBncP43o/vX0Dqnv768YMJ6Cae7z/eM66Kjq7Vrq1oQnP9XxhjipuP/rSHD64eXr/qv7CWFtbg+PTwEcPmnEIVsMumhwTdsaiuUGZmY2O4uWM3w81jJ0xr1q49U6etw3nhxo0fskyMDHyiogzfBQVj5ORlNRkVFCptk+IZ2Dg5Ge4dO84QkJrJCA6zZBZWzsw161Q25BbXfPn4iUHB3oaBVVRIPoeVXe/xzZv/gKGrP/X37//vXr9hnHD16uJfr95eMg7yZ2BiZWXYN2/RDZhBKIk2nplZqEVTy3TP9s3/T5459n/dxN7/McwsBjD5dGZWpuXREa3Hjh/6f/jw3v+tfr7pBBNtspiY2+4dm/5vWjr/vx8TczCy3MLcrP9HTh78X2Ns5EhUop3z8uUuZg5OhuuHjzGwAcN2qq1d1JTkhGsguUeHj1v8+f2XgYWPXxlnok1kZhGCCfS5uXn8Z/zP8ODseQbvkvxdmq21S7XiozU3zJ72//G7dzLf3r5lkLCznIjNMEb0ImhlXdUHIUdb/s+vXjHwiokxPDp1huHLtVvLVCKCo4CRyvDr82cGRjZWBj//CEYMlyUguQoEuAx0+f/+/cfAwsHOsL+zLyC5vJYxf+HiaG/PAMaX124ysAsKMbBx8zJUWVqo4CyCQHiynqHQlh0b/09qqPkF4tszMTGjB3KVj3f8unXL//fmZp7FG5uzQ4OaSuztrIkpu3rLCh+iizHSvdge+oYBBBgA3u0GkFI0rc4AAAAASUVORK5CYII=";
        //    string xp =
        //        "iVBORw0KGgoAAAANSUhEUgAAABAAAAATCAYAAACZZ43PAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyRpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo5QUU0N0I4QjUzRkUxMUUyODREOUZENzBDNkEyNjEzNiIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDo5QUU0N0I4QzUzRkUxMUUyODREOUZENzBDNkEyNjEzNiI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjlBRTQ3Qjg5NTNGRTExRTI4NEQ5RkQ3MEM2QTI2MTM2IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjlBRTQ3QjhBNTNGRTExRTI4NEQ5RkQ3MEM2QTI2MTM2Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+AKia1gAAAxVJREFUeNqcU11IU2EYfs/OOXPbcW2psIltLi00KkdqhtrPhVFeZBiEEsUswboIg8ggL7rpoh9Ssyi7yCiCWBcygn6M/sjoyouJsoU/bdV0U7c55zb3534670c7WFBQL7zn/c73ve/zPe/zfR+k02lY7VcZlsJ4+WfsYrLobiaLy6zfYaXs6nySdIUVc4V56iS7vbxNV1Otk+nW76EVigWWoQtCoZCfdrtrF0bHrJNv31e2TX6OwiojAIOGlvayjnM9nFbL4H8ikQCKooBhGEilUhAIBCCZTEIqEoG5Z88nd3VeKBEA7m+rKDgw+GI4Wy7P5wuplZUViMViBCAej0M0GiVUw+EwyOVyBEoG7vU/quruakUAZq1WI0sD5PDJFE7g7piMhcvLy4QFzkkkEqBpGlPohERa9EsLHo8njVSxMMMAI8uyxBEEGSGow+EAS8+N4vNvXtsFgD6DQZXf2GjKkkhqtFotAUJHQ+DFxUVwOp3g9/thamoKxq3Wl80yeUvHpyEvAUB72nvz4eiSvyU3NxdmZ2dJAe6M4s3MzIDb7SYtcRwHpaWl0NTQAMPHW2km04sqN0/dtH8f2Gw2iPBqLy0tYWtESIVCATqdDtRqNej1ejJ22exwLRFPEYCByh1bVwxHPbU8OiYVFhaSnbE4Y6gFskV2ZrMZLK9eGQ82NwEzsLlMXP7kcWeMYY4YjUYiUnV1NSiVSrBYLEQ4LEZGmVbE8/PGTd+nLwkiWq1Wk0qlOoRimUwmGBkZgWAwCF6vV2CBbSEYHmm9hDvd/2Wij9yDLl2x9PCHdwpUGJPr6upIj3a7nag+NzdHbiKehkgkwpiUff0WfJCdQ50I+dKEwe2TpwbX7Kytx/PXaDQEKHMb0Xw+Hzk+l8sFAafTVTVm3XjR5wkLLaAd02iGUsUbSir21uVNjI+npx0OKsWviyhK5Odbi4VCUBSOfFT6fOb+eOysoG7mWbbTTFOfmKvE8V0ppxeer1i25Qy/9vuzzzj5dNDsbozX2SzqT4l/BeB3X9fLSth/LRYA0G+JZdT/APwQYABW1V8RGJTU1AAAAABJRU5ErkJggg==";
        //    byte[] xp_bytes = Convert.FromBase64String(xp);
        //    byte[] credits_bytes = Convert.FromBase64String(credits);
        //    using (FileStream fileStream = File.Create(@"c:\CreditsIcon_Large.png"))
        //    {
        //        fileStream.Write(credits_bytes, 0, credits_bytes.Length);
        //        fileStream.Flush();
        //        fileStream.Close();
        //    }

        //    using (FileStream fileStream = File.Create(@"c:\XpIcon_Large.png"))
        //    {
        //        fileStream.Write(xp_bytes, 0, xp_bytes.Length);
        //        fileStream.Flush();
        //        fileStream.Close();
        //    }
        //}

        [Test]
        public void MedalsTest()
        {
            MedalHelper.ReadMedals();
        }
        
        [Test]
        public void TEffTest()
        {
            Dictionary<string, VStat> vstat = WotApiClient.Instance.ReadVstat();

            int playerId = 10800699;

            IEnumerable<PlayerStatisticEntity> statisticEntities = DossierRepository.GetPlayerStatistic(playerId);
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();

            IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(currentStatistic.PlayerId);
            List<TankJson> tankJsons = entities.GroupBy(x => x.TankId).Select(x => x.Select(tank => WotApiHelper.UnZipObject<TankJson>(tank.Raw)).OrderByDescending(y => y.A15x15.battlesCount).FirstOrDefault()).ToList();

            TankJson is3 = tankJsons.First(x => x.UniqueId() == 29);
            TankDescription tankDescription = Dictionaries.Instance.Tanks[29];
            VStat stat = vstat[tankDescription.Icon.Icon];

            double damageDealt = is3.A15x15.damageDealt;
            double battlesCount = is3.A15x15.battlesCount;
            double spoted = is3.A15x15.spotted;
            double frags = is3.A15x15.frags;

            //корректирующие коэффициенты, которые задаются для каждого типа и уровня танка согласно матрице 
            //(на время тестов можно изменять эти коэффициенты в конфиге в секции "consts")
            double Kf = 1;
            double Kd = 3;
            double Ks = 1;
            double Kmin = 0.4;

            double Dmax = stat.topD;
            double Smax = stat.topS;
            double Fmax = stat.topF;

            double Davg = stat.avgD;
            double Savg = stat.avgS;
            double Favg = stat.avgF;

            double Dmin = Davg * Kmin;
            double Smin = Savg * Kmin;
            double Fmin = Favg * Kmin;

            //параметры текущего игрока для текущего танка (дамаг)
            double Dt = damageDealt / battlesCount;
            double D = Dt > Davg ? 1 + (Dt - Davg) / (Dmax - Davg) :
                           1 + (Dt - Davg) / (Davg - Dmin);

            //параметры текущего игрока для текущего танка (фраги)
            double Ft = frags / battlesCount;
            double F = Ft > Favg ? 1 + (Ft - Favg) / (Fmax - Favg) :
                           1 + (Ft - Favg) / (Favg - Fmin);

            //параметры текущего игрока для текущего танка (засвет)
            double St = spoted / battlesCount;
            double S = St > Savg ? 1 + (St - Savg) / (Smax - Savg) :
                           1 + (St - Savg) / (Savg - Smin);

            double TEFF = (D * Kd + F * Kf + S * Ks) / (Kd + Kf + Ks) * 1000;

            Console.WriteLine(TEFF);

            double D2 = Dt > Davg ? 1 + (Dt - Davg) / (Dmax - Davg) : Dt / Davg;

            double F2 = Ft > Favg ? 1 + (Ft - Favg) / (Fmax - Favg) : Ft / Favg;

            double S2 = St > Savg ? 1 + (St - Savg) / (Smax - Savg) : St / Savg;

            double TEFF2 = (D2 * Kd + F2 * Kf + S2 * Ks) / (Kd + Kf + Ks) * 1000;

            Console.WriteLine(TEFF2);
        }

        [Test]
        public void XVMTest()
        {
            double xeff = RatingHelper.XEFF(1257);
            double xwn = RatingHelper.XWN6(1318);
        }

        [Test]
        public void NoobMeterPerformanceRatingAlgorithmTest()
        {
            int playerId = 10800699;

            IEnumerable<PlayerStatisticEntity> statisticEntities = DossierRepository.GetPlayerStatistic(playerId);
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();

            IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(currentStatistic.PlayerId);
            List<TankJson> tankJsons = entities.GroupBy(x => x.TankId).Select(x => x.Select(tank => WotApiHelper.UnZipObject<TankJson>(tank.Raw)).OrderByDescending(y => y.A15x15.battlesCount).FirstOrDefault()).ToList();

            double damage = tankJsons.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();

            var performanceRating = RatingHelper.PerformanceRating(currentStatistic.BattlesCount, currentStatistic.Wins, damage, currentStatistic.DamageDealt, currentStatistic.AvgLevel);

            Console.WriteLine(performanceRating);
        }

        [Test]
        public void UpdateTest()
        {
            DatabaseManager manager = new DatabaseManager();
            manager.Update();
        }

        [Test]
        public void ReplaysFoldersSaveLoadTest()
        {
            ReplayFolder folder = new ReplayFolder{Name = "Parent", Path = "c:\\Parent", Folders = new ObservableCollection<ReplayFolder> {new ReplayFolder{Name = "Child", Path = "c:\\Child"}}};
            string xml = XmlSerializer.StoreObjectInXml(folder);
            Console.WriteLine(xml);

            ReplayFolder replayFolder = XmlSerializer.LoadObjectFromXml<ReplayFolder>(xml);

            Console.WriteLine(replayFolder.Folders.Count);
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
                XmlNode nominal_damage = values[5];
                XmlNode wn8_nominal_damage = values[6];
                XmlNode wn8_nominal_win_rate = values[7];
                XmlNode wn8_nominal_spotted = values[8];
                XmlNode wn8_nominal_frags = values[9];
                XmlNode wn8_nominal_defence = values[10];
                XmlNode href = values[1].SelectSingleNode("a/@href");
                XmlNode title = values[1].SelectSingleNode("a");

                RatingExpectancy ratingExpectancy = new RatingExpectancy();
                ratingExpectancy.PRNominalDamage = double.Parse(nominal_damage.InnerText.Replace(",", ""));
                if (!string.IsNullOrEmpty(wn8_nominal_damage.InnerText))
                {
                    ratingExpectancy.Wn8NominalDamage = double.Parse(wn8_nominal_damage.InnerText.Replace(",", ""));
                    ratingExpectancy.Wn8NominalWinRate = double.Parse(wn8_nominal_win_rate.InnerText.Replace("%", ""));
                    ratingExpectancy.Wn8NominalSpotted = double.Parse(wn8_nominal_spotted.InnerText.Replace(".", ","));
                    ratingExpectancy.Wn8NominalFrags = double.Parse(wn8_nominal_frags.InnerText.Replace(".", ","));
                    ratingExpectancy.Wn8NominalDefence = double.Parse(wn8_nominal_defence.InnerText.Replace(".", ","));
                }
                ratingExpectancy.Icon = href.InnerText.Replace("/tank/eu/", "");
                ratingExpectancy.TankTitle = title.InnerText.Trim();
                
                xmlTanks.Add(ratingExpectancy);
            }

            foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            {
                if (xmlTanks.FirstOrDefault(x => x.Icon.ToLower() == description.Icon.IconOrig.ToLower()) == null)
                {
                    Console.WriteLine(description.Icon.IconOrig);
                }
            }

            JsonSerializer se = new JsonSerializer();
            StringBuilder builder = new StringBuilder();
            se.Serialize(new StringWriter(builder), xmlTanks);

            Console.WriteLine(builder);

            //IEnumerable<string> enumerable = xmlTanks.Join(WotApiClient.Instance.TanksDictionary.Values, x => x.Tank, y => y.Icon.Icon.ToLower(),
            //    (x, y) => string.Format("{0} \t\t {1} - {2} \t\t\t\t {3}", x.Tank, x.PRNominalDamage, y.Expectancy.PRNominalDamage, x.PRNominalDamage == y.Expectancy.PRNominalDamage));

            //foreach (var value in enumerable)
            //{
            //    Console.WriteLine(value);
            //}
        }

        [Test]
        public void CacheTest()
        {
            List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksCache(@"D:\NRXWO2LOFZYDMLTXN5ZGYZDPMZ2GC3TLOMXG4ZLUHIZDAMBRGQ5XO2LMMRTW6YTMNFXA====.json");

            foreach (var group in tanksV2.GroupBy(x => x.Common.type))
            {
                Console.WriteLine();
                Console.WriteLine("Battles - " + group.Sum(x => x.A15x15.battlesCount));

                foreach (TankJson tankJson in group.OrderBy(x => x.Common.type).ThenByDescending(x => x.A15x15.battlesCount))
                {
                    Console.WriteLine("15x15-{1}\t\tclan-{2}\tcompany-{3}\t7x7-{4}\t\tTank - {0} - {5}", tankJson.Common.tanktitle, GetCount(tankJson.A15x15), GetCount(tankJson.Clan), GetCount(tankJson.Company), GetCount(tankJson.A7x7), tankJson.Common.basedonversion);
                }
            }

            Console.WriteLine("battles count cache -" + tanksV2.Sum(x => x.A15x15.battlesCount));
            Console.WriteLine("last battle -" + tanksV2.Max(x => x.Common.lastBattleTimeR));
            Console.WriteLine("tanks count cache - " + tanksV2.Count);

            string tanks = new Uri(
                "http://api.worldoftanks.ru/wot/account/tanks/?application_id=19779cdf6e8aab1a8c99e1261274f13b&access_token=ab2994337c9f5b09d0b7f372001f17c45f0b4af8&account_id=1749450")
                .Get();

            var parsedData = JsonConvert.DeserializeObject<JObject>(tanks);

            JArray array = (JArray)parsedData["data"]["1749450"];
            Console.WriteLine("tanks count server - " + array.Count());
            Console.WriteLine("battles count server - " + array.Sum(x => x["statistics"]["all"]["battles"].Value<int>()));

            foreach (TankJson tankJson in tanksV2)
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

        [Test]
        public void CsvExportProviderTest()
        {
            CsvExportProvider provider = new CsvExportProvider();
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksCache(cacheFile.FullName.Replace(".dat", ".json"));
            List<TankStatisticRowViewModel> list = tanksV2.Select(x => new TankStatisticRowViewModel(x)).ToList();
            provider.Export(list, new List<Type>{typeof(ITankRowBattles), typeof(ITankRowFrags)});
        }

        [Test]
        public void appSpotTest()
        {
            AppSpotUploader uploader = new AppSpotUploader();

            uploader.Upload(
                new FileInfo(
                    @"C:\Users\Pasha\AppData\Roaming\wargaming.net\WorldOfTanks\dossier_cache\NRXWO2LOFZYDCLTXN5ZGYZDPMZ2GC3TLOMXG4ZLUHIZDAMBRGQ5V6UTFNVRGK3C7.dat"), 19376001);
        }
    }
}
