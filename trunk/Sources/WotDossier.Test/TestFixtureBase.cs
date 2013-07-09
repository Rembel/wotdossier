﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Applications;
using WotDossier.Applications.ViewModel;
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
            Thread.Sleep(1000);
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
            Thread.Sleep(1000);
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier\Resources\Images\Tanks\{0}.png", tankJson.Icon.iconid);
                Assert.True(File.Exists(iconPath), string.Format("can't find icon {0}", tankJson.Icon.iconid));
            }
        }

        [Test]
        public void CacheTest_087()
        {
            FileInfo cacheFile = GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.7\");
            CacheHelper.BinaryCacheToJson(cacheFile);
            Thread.Sleep(1000);
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

        [Test]
        public void ReplayTest_087()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.7\20130706_1009_ussr-T-54_73_asia_korea.wotreplay"));
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
            //FileInfo info = new FileInfo(@"C:\Documents and Settings\YaroshikPV\AppData\Roaming\Wargaming.net\WorldOfTanks\replays\20121111_1414_ussr-KV-1s_13_erlenberg.wotreplay");

            //ReplayUploader uploader = new ReplayUploader();

            //uploader.Upload(info, "replay1", "replayDescription1", "http://wotreplays.ru/site/upload");
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
        public void ReplayTest()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            Replay replay = WotApiClient.Instance.ReadReplay2Blocks(cacheFile);
        }

        [Test]
        public void MedalsTest()
        {
            MedalHelper.ReadMedals();
        }

        [Test]
        public void CritsTest()
        {
            int res = 67108864 >> 24 & 255;
            int res1 = 65552 >> 12 & 4095;
        }
    }
}
