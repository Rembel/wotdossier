using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public class PhisicalReplay : ReplayFile
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ReplayFile" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public override bool Exists
        {
            get { return PhisicalFile.Exists; }
        }

        /// <summary>
        /// Gets the phisical path.
        /// </summary>
        /// <value>
        /// The phisical path.
        /// </value>
        public override string PhisicalPath
        {
            get { return PhisicalFile != null ? PhisicalFile.FullName : null; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get { return PhisicalFile != null ? PhisicalFile.Name : null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhisicalReplay" /> class.
        /// </summary>
        /// <param name="replayFileInfo">The replay file info.</param>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public PhisicalReplay(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            PhisicalFile = replayFileInfo;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
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

        /// <summary>
        /// Plays replay.
        /// </summary>
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

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            if (PhisicalFile != null)
            {
                return ReplayFileHelper.ParseReplay(PhisicalFile, ClientVersion, readAdvancedData);
            }
            return null;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            PhisicalFile.Delete();
        }
    }
}
