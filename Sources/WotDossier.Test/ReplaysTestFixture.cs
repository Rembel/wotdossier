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
using WotDossier.Domain.Replay;

namespace WotDossier.Test
{
    /// <summary>
    /// Replays tests
    /// </summary>
    public class ReplaysTestFixture : TestFixtureBase
    {
        [Test]
        public void ReplayTest_084()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_086()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.6\20130612_0912_germany-E-100_28_desert.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_087()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.7\20130706_1009_ussr-T-54_73_asia_korea.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_088()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.8\20130908_2025_usa-M103_14_siegfried_line.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_089()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.9\20131016_0035_ussr-Object263_37_caucasus.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_0810()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.10\20131208_0156_ussr-Object_140_53_japan.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_0(cacheFile);
        }

        [Test]
        public void ReplayTest_0811()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.8.11\20140126_2109_ussr-T-54_14_siegfried_line.wotreplay"));

            Replay replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, true);
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
        }

        [Test]
        public void ReplayTest_090()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.9.0\13954715200495_germany_PzVI_prohorovka.wotreplay"));

            Replay replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, true);
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
        }

        [Test]
        public void ReplayTest_092()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.9.2\20140713_0042_usa-T57_58_73_asia_korea.wotreplay"));

            Replay replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, true);
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
        }

        [Test]
        public void ReplayTest_093()
        {
            FileInfo cacheFile =
                new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    @"Replays\0.9.3\20140907_1157_usa-T57_58_73_asia_korea.wotreplay"));

            Replay replay = ReplayFileHelper.ParseReplay_8_11(cacheFile, false);
            Assert.IsNotNull(replay);
            Assert.IsNotNull(replay.datablock_battle_result);
            var mockView = new Mock<IReplayView>();
            ReplayViewModel model = new ReplayViewModel(mockView.Object);
            model.Init(replay, new PhisicalReplay(cacheFile, replay, Guid.NewGuid()));
        }

        [Test]
        public void ReplayTest()
        {
            FileInfo cacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Replays\0.8.5\20121107_1810_ussr-KV-1s_10_hills.wotreplay"));
            Replay replay = ReplayFileHelper.ParseReplay_8_11(cacheFile);
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
            replay = ReplayFileHelper.ParseReplay_8_11(cacheFile);
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
