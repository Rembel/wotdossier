using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
using WotDossier.Domain.Replay;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Test
{
    /// <summary>
    ///     Replays tests
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

                Console.WriteLine("Found: {0}", replays.Count());

                for (int index = 0; index < replays.Length; index++)
                {
                    string fileName = replays[index];
                    Console.WriteLine("Process[{0}]: {1}", index, fileName);

                    FileInfo replayFile = new FileInfo(fileName);

                    Replay replay = ReplayFileHelper.ParseReplay_8_11(replayFile);
                    Assert.IsNotNull(replay);
                    Assert.IsNotNull(replay.datablock_battle_result);

                    var phisicalReplay = new PhisicalReplay(replayFile, replay, Guid.Empty);
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
        public void ReplayToJsonTest1()
        {
            ReplayToJson(@"");
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
                            blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                            Console.WriteLine("Found Replay Blocks: " + blocksCount);
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            var blockLength = (int) stream.Read(4).ConvertLittleEndian();
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
        public void ReplaysViewModelTest()
        {
            string replayFolder = Path.Combine(Environment.CurrentDirectory, "Replays", new Version("0.8.1").ToString(3));
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