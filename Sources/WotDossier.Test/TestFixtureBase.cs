using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Common.Reflection;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Formatting = Newtonsoft.Json.Formatting;

namespace WotDossier.Test
{
    [TestFixture]
    public class TestFixtureBase
    {
        private DataProvider _dataProvider;
        private DossierRepository _dossierRepository;
        private DatabaseManager _databaseManager;

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

        public DatabaseManager DatabaseManager
        {
            get { return _databaseManager; }
        }

        [TestFixtureSetUp]
        public void Init()
        {
            AssemblyExtensions.SetEntryAssembly(Assembly.LoadFrom("WotDossier.Test.dll"));
            CompositionContainerFactory.Instance.Container.SatisfyImportsOnce(this);
            _databaseManager = new DatabaseManager();
            _databaseManager.InitDatabase();
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
            FileInfo info = new FileInfo(@"Replays\20140325_2258_ussr-Object_140_84_winter.wotreplay");

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

        [Test]
        public void MultipleUploadTest()
        {
            FileInfo info = new FileInfo(@"Replays\20140325_2258_ussr-Object_140_84_winter.wotreplay");
            ReplayUploader uploader = new ReplayUploader();
            WotReplaysSiteResponse response = uploader.Upload(info, 10800699, "_rembel_");
            Console.WriteLine(response.Error);
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

        //[Test]
        //public void TEffTest()
        //{
        //    Dictionary<string, VStat> vstat = WotApiClient.Instance.ReadVstat();

        //    int playerId = 10800699;

        //    IEnumerable<PlayerStatisticEntity> statisticEntities = DossierRepository.GetPlayerStatistic(playerId);
        //    PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();

        //    IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(currentStatistic.PlayerId);
        //    List<TankJson> tankJsons = entities.GroupBy(x => x.TankId).Select(x => x.Select(tank => CompressHelper.DecompressObject<TankJson>(tank.Raw)).OrderByDescending(y => y.A15x15.battlesCount).FirstOrDefault()).ToList();

        //    TankJson is3 = tankJsons.First(x => x.UniqueId() == 29);
        //    TankDescription tankDescription = Dictionaries.Instance.Tanks[29];
        //    VStat stat = vstat[tankDescription.Icon.Icon];

        //    double damageDealt = is3.A15x15.damageDealt;
        //    double battlesCount = is3.A15x15.battlesCount;
        //    double spoted = is3.A15x15.spotted;
        //    double frags = is3.A15x15.frags;

        //    //корректирующие коэффициенты, которые задаются для каждого типа и уровня танка согласно матрице 
        //    //(на время тестов можно изменять эти коэффициенты в конфиге в секции "consts")
        //    double Kf = 1;
        //    double Kd = 3;
        //    double Ks = 1;
        //    double Kmin = 0.4;

        //    double Dmax = stat.topD;
        //    double Smax = stat.topS;
        //    double Fmax = stat.topF;

        //    double Davg = stat.avgD;
        //    double Savg = stat.avgS;
        //    double Favg = stat.avgF;

        //    double Dmin = Davg * Kmin;
        //    double Smin = Savg * Kmin;
        //    double Fmin = Favg * Kmin;

        //    //параметры текущего игрока для текущего танка (дамаг)
        //    double Dt = damageDealt / battlesCount;
        //    double D = Dt > Davg ? 1 + (Dt - Davg) / (Dmax - Davg) :
        //                   1 + (Dt - Davg) / (Davg - Dmin);

        //    //параметры текущего игрока для текущего танка (фраги)
        //    double Ft = frags / battlesCount;
        //    double F = Ft > Favg ? 1 + (Ft - Favg) / (Fmax - Favg) :
        //                   1 + (Ft - Favg) / (Favg - Fmin);

        //    //параметры текущего игрока для текущего танка (засвет)
        //    double St = spoted / battlesCount;
        //    double S = St > Savg ? 1 + (St - Savg) / (Smax - Savg) :
        //                   1 + (St - Savg) / (Savg - Smin);

        //    double TEFF = (D * Kd + F * Kf + S * Ks) / (Kd + Kf + Ks) * 1000;

        //    Console.WriteLine(TEFF);

        //    double D2 = Dt > Davg ? 1 + (Dt - Davg) / (Dmax - Davg) : Dt / Davg;

        //    double F2 = Ft > Favg ? 1 + (Ft - Favg) / (Fmax - Favg) : Ft / Favg;

        //    double S2 = St > Savg ? 1 + (St - Savg) / (Smax - Savg) : St / Savg;

        //    double TEFF2 = (D2 * Kd + F2 * Kf + S2 * Ks) / (Kd + Kf + Ks) * 1000;

        //    Console.WriteLine(TEFF2);
        //}

        [Test]
        public void NoobMeterPerformanceRatingAlgorithmTest()
        {
            int playerId = 10800699;

            IEnumerable<PlayerStatisticEntity> statisticEntities = DossierRepository.GetPlayerStatistic<PlayerStatisticEntity>(playerId);
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();

            IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic<TankStatisticEntity>(currentStatistic.PlayerId);
            List<TankJson> tankJsons = entities.GroupBy(x => x.TankId).Select(x => x.Select(tank => CompressHelper.DecompressObject<TankJson>(tank.Raw)).OrderByDescending(y => y.A15x15.battlesCount).FirstOrDefault()).ToList();

            var performanceRating = RatingHelper.PerformanceRating(tankJsons, json => json.A15x15);

            Console.WriteLine(performanceRating);
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
            provider.Export(list, new List<Type>{typeof(IStatisticBattles), typeof(IStatisticFrags)});
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
        public void ComparerTest()
        {
            List<SortDescription> sortDescriptions = new List<SortDescription>();
            sortDescriptions.Add(new SortDescription("PiercedReceived", ListSortDirection.Ascending));
            sortDescriptions.Add(new SortDescription("BattlesCount", ListSortDirection.Ascending));

            MultiPropertyComparer<ITankStatisticRow> comparer = new MultiPropertyComparer<ITankStatisticRow>(sortDescriptions);

            List<ITankStatisticRow> list = new List<ITankStatisticRow>();
            list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 10});
            list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 12});
            list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 11});

            foreach (var tankStatisticRow in list)
            {
                Console.WriteLine("PiercedReceived [{0}] - BattlesCount [{1}]", tankStatisticRow.PiercedReceived, tankStatisticRow.BattlesCount);
            }

            list.Sort(comparer);

            foreach (var tankStatisticRow in list)
            {
                Console.WriteLine("PiercedReceived [{0}] - BattlesCount [{1}]", tankStatisticRow.PiercedReceived, tankStatisticRow.BattlesCount);
            }
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
                var localizedString = Resources.Resources.ResourceManager.GetString(medal.Value.Name);
                Assert.IsNotNullOrEmpty(localizedString, "Resource not found: {0}", medal.Value.Name);

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
                var localizedString = Resources.Resources.ResourceManager.GetString("Map_" + map.Value.mapidname);
                Assert.IsNotNullOrEmpty(localizedString, "Resource not found: {0}", map.Value.mapidname);

                var key = string.Format("images/maps/{0}.jpg", map.Value.mapidname).ToLowerInvariant();
                //Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", map.Value.mapidname);
                if (!dictionary.ContainsKey(key))
                {
                    Console.WriteLine("Image resource not found: {0}", map.Value.mapidname);
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
    }
}
