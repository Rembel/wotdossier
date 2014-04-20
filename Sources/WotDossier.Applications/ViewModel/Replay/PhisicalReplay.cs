using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class PhisicalReplay : ReplayFile
    {
        public override bool Exists
        {
            get { return PhisicalFile.Exists; }
        }

        public override string PhisicalPath
        {
            get { return PhisicalFile != null ? PhisicalFile.FullName : null; }
        }

        public override string Name
        {
            get { return PhisicalFile != null ? PhisicalFile.Name : null; }
        }

        public PhisicalReplay(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            PhisicalFile = replayFileInfo;
        }

        public override void Move(ReplayFolder targetFolder)
        {
            if (PhisicalFile != null)
            {
                string destFileName = Path.Combine(targetFolder.Path, PhisicalFile.Name);
                if (!File.Exists(destFileName))
                {
                    PhisicalFile.MoveTo(destFileName);
                }
                else
                {
                    PhisicalFile.Delete();
                }
            }
        }

        public override void Play()
        {
            if (PhisicalFile != null && File.Exists(PhisicalFile.FullName))
            {
                AppSettings appSettings = SettingsReader.Get();
                string path = appSettings.PathToWotExe;
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    VistaOpenFileDialog dialog = new VistaOpenFileDialog();
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;
                    dialog.DefaultExt = ".exe"; // Default file extension
                    dialog.Filter = "WorldOfTanks (.exe)|*.exe"; // Filter files by extension 
                    dialog.Multiselect = false;
                    dialog.Title = Resources.Resources.WindowCaption_SelectPathToWorldOfTanksExecutable;
                    bool? showDialog = dialog.ShowDialog();
                    if (showDialog == true)
                    {
                        path = appSettings.PathToWotExe = dialog.FileName;
                        SettingsReader.Save(appSettings);
                    }
                }

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    try
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.FileName = path;
                        proc.StartInfo.Arguments = string.Format("\"{0}\"", PhisicalFile.FullName);
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on play replay ({0} {1})", e, path, PhisicalFile.FullName);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnPlayReplay, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public override Domain.Replay.Replay ReplayData()
        {
            if (PhisicalFile != null)
            {
                string jsonFile = PhisicalFile.FullName.Replace(PhisicalFile.Extension, ".json");
                //convert dossier cache file to json
                if (!File.Exists(jsonFile))
                {
                    CacheHelper.ReplayToJson(PhisicalFile);
                }

                if (!File.Exists(jsonFile))
                {
                    MessageBox.Show(Resources.Resources.Msg_Error_on_replay_file_read,
                        Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return WotApiClient.Instance.LoadReplay(jsonFile);
            }
            return null;
        }

        public override void Delete()
        {
            CompositionContainerFactory.Instance.GetExport<DossierRepository>().SaveReplay(PlayerId, ReplayId, Link, ReplayData());
            PhisicalFile.Delete();
        }
    }
}
