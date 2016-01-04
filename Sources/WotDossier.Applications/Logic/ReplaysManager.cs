using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Settings;
using WotDossier.Framework;

namespace WotDossier.Applications.Logic
{
    [Export]
    public class ReplaysManager
    {
        private static readonly ILog _log = LogManager.GetLogger<ReplaysManager>();

        private const string REPLAYS_CATALOG_FILE_PATH = @"Data\ReplaysCatalog.xml";

        private static readonly Guid FOLDER_DELETED = new Guid("09C12D79-65B0-49DE-A257-D7B9B411F0C3");

        public static ReplayFolder DeletedFolder = new ReplayFolder
        {
            Id = FOLDER_DELETED,
            Name = Resources.Resources.ReplaysFolders_Deleted
        };

        /// <summary>
        /// Saves the folder.
        /// </summary>
        /// <param name="replayFolder">The replay folder.</param>
        public void SaveFolder(ReplayFolder replayFolder)
        {
            using (StreamWriter writer = File.CreateText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                writer.WriteLine(XmlSerializer.StoreObjectInXml(replayFolder));
                writer.Flush();
            }
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <returns></returns>
        public List<ReplayFolder> GetFolders()
        {
            using (StreamReader streamReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, REPLAYS_CATALOG_FILE_PATH)))
            {
                string tree = streamReader.ReadToEnd();
                return new List<ReplayFolder> {InitFolder(tree)};
            }
        }

        private static ReplayFolder InitFolder(string tree)
        {
            ReplayFolder folder = XmlSerializer.LoadObjectFromXml<ReplayFolder>(tree);
            return folder;
        }

        public void PlayWith(ReplayFile replayFile)
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<ReplayViewerSettingsViewModel>();
            if (viewModel.Show() == false)
            {
                return;
            }

            var replayPlayer = viewModel.Player;
            if (replayPlayer == null)
            {
                replayPlayer = AutoSelectPlayer(replayFile.ClientVersion);
            }
            ExecutePlayer(replayFile, replayPlayer);
        }

        public void Play(ReplayFile replayFile)
        {
            ReplayPlayer replayPlayer = AutoSelectPlayer(replayFile.ClientVersion);
            if (replayPlayer != null)
            {
                ExecutePlayer(replayFile, replayPlayer);
            }
            else
            {
                PlayWith(replayFile);    
            }
        }

        private static void ExecutePlayer(ReplayFile replayFile, ReplayPlayer player)
        {
            if (player != null && replayFile != null && replayFile is PhisicalReplay)
            {
                if (!string.IsNullOrEmpty(player.Path) && File.Exists(player.Path))
                {
                    try
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.FileName = player.Path;
                        proc.StartInfo.Arguments = string.Format("\"{0}\"", replayFile.PhisicalPath);
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on play replay ({0} {1})", e, player.Path, replayFile.PhisicalPath);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnPlayReplay, Resources.Resources.WindowCaption_Error,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private ReplayPlayer AutoSelectPlayer(Version replayFileVersion)
        {
            AppSettings settings = SettingsReader.Get();
            return settings.ReplayPlayers.OrderBy(x => x.Version).FirstOrDefault(x => x.Version >= replayFileVersion);
        }
    }
}