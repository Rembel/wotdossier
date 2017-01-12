using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Resources;
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

        private List<ResourceManager> _resourceManagers;

        public List<ResourceManager> ResourceManagers
        {
            get { return _resourceManagers; }
        }

        [TestFixtureSetUp]
        public void Init()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            AssemblyExtensions.SetEntryAssembly(Assembly.LoadFrom(Path.Combine(TestContext.CurrentContext.TestDirectory, "WotDossier.Test.dll")));
            CompositionContainerFactory.Instance.Container.SatisfyImportsOnce(this);
            _databaseManager = new DatabaseManager();
            _databaseManager.InitDatabase();

            _resourceManagers = new List<ResourceManager>();

            Assembly entryAssembly = GetType().Assembly;
            var resources = AssemblyExtensions.GetResourcesByMask(entryAssembly, ".resources");

            foreach (var resource in resources)
            {
                var resourceManager = new ResourceManager(resource.Replace(".resources", string.Empty), GetType().Assembly);
                _resourceManagers.Add(resourceManager);
            }

            CultureHelper.SetUiCulture(SettingsReader.Get().Language);

            if (!UriParser.IsKnownScheme("pack"))
            {
                // the pack:// scheme is not yet registered. This scheme is registered when you create the Application object.
                new System.Windows.Application();
            }
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
        
    }
}
