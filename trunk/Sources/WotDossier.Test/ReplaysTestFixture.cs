using System;
using System.Collections.ObjectModel;
using System.IO;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain.Replay;

namespace WotDossier.Test
{
    /// <summary>
    /// Replays tests
    /// </summary>
    public class ReplaysTestFixture : TestFixtureBase
    {
        [Test]
        public void ReplaysTest()
        {
            foreach (Version version in Dictionaries.Instance.Versions)
            {
                string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", version.ToString(3));

                if (!Directory.Exists(replayFolder))
                {
                    Assert.Fail("Folder not exists - [{0}]", replayFolder);        
                }

                var replays = Directory.GetFiles(replayFolder, "*.wotreplay");

                foreach (string fileName in replays)
                {
                    FileInfo replayFile = new FileInfo(fileName);

                    Replay replay = ReplayFileHelper.ParseReplay_8_11(replayFile);
                    Assert.IsNotNull(replay);
                    Assert.IsNotNull(replay.datablock_battle_result);

                    PhisicalReplay phisicalReplay = new PhisicalReplay(replayFile, replay, Guid.Empty);

                    var mockView = new Mock<IReplayView>();
                    ReplayViewModel model = new ReplayViewModel(mockView.Object);
                    model.Init(phisicalReplay.ReplayData(true));   
                }
            }
        }

        [Test]
        public void AdvancedReplayTest()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.9.1\14003587093213_ussr_Object_140_el_hallouf.wotreplay"));
            StopWatch watch = new StopWatch();
            watch.Reset();
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile, true);
            Console.WriteLine(watch.PeekMs());

            string serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
            serializeObject.Dump(cacheFile.FullName + "_1");

            watch.Reset();
            replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, true);
            Console.WriteLine(watch.PeekMs());

            serializeObject = JsonConvert.SerializeObject(replay, Formatting.Indented);
            serializeObject.Dump(cacheFile.FullName + "_2");
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
    }
}
