using System.ComponentModel.Composition;
using System.Reflection;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Update.Update;

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

            CultureHelper.SetUiCulture();
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

        //[Test]
        //public void UploadTest()
        //{
        //    FileInfo info = new FileInfo(@"Replays\20140325_2258_ussr-Object_140_84_winter.wotreplay");

        //    ReplayUploader uploader = new ReplayUploader();

        //    uploader.Upload(info, "replay1", "replayDescription1", "http://wotreplays.ru/site/upload");
        //    string url = "http://wotreplays.ru/site/upload";
        //    Uri uri = new Uri(url);
        //    CookieContainer cookieContainer = ReplayUploader.LoadCookies(url);
        //    foreach (Cookie coockie in cookieContainer.GetCookies(uri))
        //    {
        //        string s = HttpUtility.UrlDecode(coockie.Value);
        //    }
        //}

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

        //[Test]
        //public void JsonTest()
        //{
        //    int playerId = 10800699;

        //    IEnumerable<PlayerStatisticEntity> statisticEntities = DossierRepository.GetPlayerStatistic<PlayerStatisticEntity>(playerId);
        //    PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();

        //    IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic<TankStatisticEntity>(currentStatistic.PlayerId);

        //    var list = entities.Select(
        //        x => new {x.Updated, x.Raw, x.TankId, x.BattlesCount}).ToList();

        //    Console.WriteLine(JsonConvert.SerializeObject(list));
        //}

        //[Test]
        //public void ComparerTest()
        //{
        //    List<SortDescription> sortDescriptions = new List<SortDescription>();
        //    sortDescriptions.Add(new SortDescription("PiercedReceived", ListSortDirection.Ascending));
        //    sortDescriptions.Add(new SortDescription("BattlesCount", ListSortDirection.Ascending));

        //    MultiPropertyComparer<ITankStatisticRow> comparer = new MultiPropertyComparer<ITankStatisticRow>(sortDescriptions);

        //    List<ITankStatisticRow> list = new List<ITankStatisticRow>();
        //    list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 10});
        //    list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 12});
        //    list.Add(new RandomBattlesTankStatisticRowViewModel(TankJson.Initial){PiercedReceived = 1, BattlesCount = 11});

        //    foreach (var tankStatisticRow in list)
        //    {
        //        Console.WriteLine("PiercedReceived [{0}] - BattlesCount [{1}]", tankStatisticRow.PiercedReceived, tankStatisticRow.BattlesCount);
        //    }

        //    list.Sort(comparer);

        //    foreach (var tankStatisticRow in list)
        //    {
        //        Console.WriteLine("PiercedReceived [{0}] - BattlesCount [{1}]", tankStatisticRow.PiercedReceived, tankStatisticRow.BattlesCount);
        //    }
        //}

        //[Test]
        //public void RenameTankResources()
        //{
        //    string[] files = Directory.GetFiles(@"I:\1");

        //    foreach (var filePath in files)
        //    {
        //        var file = new FileInfo(filePath);
        //        file.MoveTo(filePath.Replace("-", "_"));
        //    }
        //}
    }
}
