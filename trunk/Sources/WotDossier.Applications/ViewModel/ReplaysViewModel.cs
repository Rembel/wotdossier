using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Logging;
using Ionic.Zip;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.Events;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Framework;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    public class ReplaysViewModel : INotifyPropertyChanged
    {
        private static readonly ILog _log = LogManager.GetLogger("ReplaysViewModel");

        public ReplaysFilterViewModel ReplayFilter { get; set; }
        public ReplaysManager ReplaysManager { get; set; }
        public DossierRepository DossierRepository { get; set; }
        public ProgressControlViewModel ProgressView { get; set; }
        public PlayerChartsViewModel ChartView { get; set; }

        public DelegateCommand<ReplayFile> ReplayUploadCommand { get; set; }
        public DelegateCommand<object> ReplaysUploadCommand { get; set; }
        public DelegateCommand<ReplayFile> ReplayDeleteCommand { get; set; }
        public DelegateCommand<ReplayFile> CopyLinkToClipboardCommand { get; set; }
        public DelegateCommand<ReplayFile> PlayReplayCommand { get; set; }
        public DelegateCommand<object> ReplayRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> ReplayRowsDeleteCommand { get; set; }
        public DelegateCommand<object> ReplayRowsZipCommand { get; set; }

        public DelegateCommand<ReplayFolder> AddFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> ZipFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> DeleteFolderCommand { get; set; }

        private List<ReplayFile> _replays = new List<ReplayFile>();
        public List<ReplayFile> Replays
        {
            get { return ReplayFilter.Filter(_replays); }
            set
            {
                _replays = value;
                OnPropertyChanged("Replays");
            }
        }

        private List<ReplayFolder> _replaysFolders;
        public List<ReplayFolder> ReplaysFolders
        {
            get { return _replaysFolders; }
            set
            {
                _replaysFolders = value;
                OnPropertyChanged("ReplaysFolders");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReplaysViewModel(DossierRepository dossierRepository, ProgressControlViewModel progressControlView, PlayerChartsViewModel playerChartsViewModel)
        {
            ReplayRowDoubleClickCommand = new DelegateCommand<object>(OnReplayRowDoubleClick);
            ReplayUploadCommand = new DelegateCommand<ReplayFile>(OnUploadReplay, CanUploadReplay);
            ReplaysUploadCommand = new DelegateCommand<object>(OnUploadReplays, CanUploadReplays);
            ReplayDeleteCommand = new DelegateCommand<ReplayFile>(OnReplayRowDelete);
            ReplayRowsDeleteCommand = new DelegateCommand<object>(OnReplayRowsDelete);
            ReplayRowsZipCommand = new DelegateCommand<object>(OnReplayRowsZip);
            CopyLinkToClipboardCommand = new DelegateCommand<ReplayFile>(OnCopyLinkToClipboard, CanCopyLinkToClipboard);
            PlayReplayCommand = new DelegateCommand<ReplayFile>(OnPlayReplay);

            AddFolderCommand = new DelegateCommand<ReplayFolder>(OnAddFolder);
            ZipFolderCommand = new DelegateCommand<ReplayFolder>(OnZipFolder);
            DeleteFolderCommand = new DelegateCommand<ReplayFolder>(OnDeleteFolderCommand);

            ReplayFilter = new ReplaysFilterViewModel();
            ReplayFilter.PropertyChanged += ReplayFilterOnPropertyChanged;

            ReplaysManager = new ReplaysManager();
            DossierRepository = dossierRepository;
            ProgressView = progressControlView;
            playerChartsViewModel.ReplaysDataSource = new CallBackDataSource<ReplayFile>(() => _replays);
            ChartView = playerChartsViewModel;

            EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Subscribe(OnReplayFileMove);
        }

        public void LoadReplaysList()
        {
            ReplaysFolders = ReplaysManager.GetFolders();
            _replays.Clear();

            List<ReplayFolder> replayFolders = ReplaysFolders.GetAll();

            ProcessReplaysFolders(replayFolders);
        }

        private void OnReplayFileMove(ReplayFileMoveEventArgs eventArgs)
        {
            if (ReplayFilter.SelectedFolder != eventArgs.TargetFolder)
            {
                ReplaysManager.Move(eventArgs.ReplayFile, eventArgs.TargetFolder);
                eventArgs.ReplayFile.FolderId = eventArgs.TargetFolder.Id;
                OnPropertyChanged("Replays");
            }
        }

        private void OnPlayReplay(ReplayFile replay)
        {
            if (replay != null && File.Exists(replay.FileInfo.FullName))
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
                        proc.StartInfo.Arguments = string.Format("\"{0}\"", replay.FileInfo.FullName);
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on play replay ({0} {1})", e, path, replay.FileInfo.FullName);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnPlayReplay, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private ReplayFolder FindParentFolder(ReplayFolder parent, ReplayFolder folder)
        {
            if (parent.Folders.Contains(folder))
            {
                return parent;
            }
            return parent.Folders.Select(child => FindParentFolder(child, folder)).FirstOrDefault(foundItem => foundItem != null);
        }

        private void OnCopyLinkToClipboard(ReplayFile model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Link))
            {
                Clipboard.SetText(model.Link);
            }
        }

        private bool CanCopyLinkToClipboard(ReplayFile model)
        {
            return model != null && !string.IsNullOrEmpty(model.Link);
        }

        private void OnDeleteFolderCommand(ReplayFolder folder)
        {
            ReplayFolder root = ReplaysFolders.FirstOrDefault();
            ReplayFolder parent = FindParentFolder(root, folder);
            if (parent != null)
            {
                parent.Folders.Remove(folder);
                ReplaysManager.SaveFolder(root);
            }
        }

        private void OnAddFolder(ReplayFolder folder)
        {
            AddReplayFolderViewModel viewModel = CompositionContainerFactory.Instance.GetExport<AddReplayFolderViewModel>();
            viewModel.Show();

            if (viewModel.ReplayFolder != null)
            {
                ReplayFolder root = ReplaysFolders.FirstOrDefault();
                folder.Folders.Add(viewModel.ReplayFolder);
                ReplaysManager.SaveFolder(root);
                ProcessReplaysFolders(new List<ReplayFolder> { viewModel.ReplayFolder });
            }
        }

        private void OnZipFolder(ReplayFolder folder)
        {
            List<ReplayFile> replays = _replays.Where(x => x.FolderId == folder.Id).ToList();

            string name = folder.Name;

            PackReplays(replays, name);
        }

        private static void PackReplays(List<ReplayFile> replays, string name)
        {
            using (new WaitCursor())
            {
                using (ZipFile zip = new ZipFile())
                {
                    // add this map file into the "images" directory in the zip archive
                    foreach (ReplayFile replayFile in replays)
                    {
                        zip.AddFile(replayFile.FileInfo.FullName, @"\" + name);
                    }

                    VistaSaveFileDialog dialog = new VistaSaveFileDialog();
                    dialog.DefaultExt = ".zip"; // Default file extension
                    dialog.Filter = "ZIP (.zip)|*.zip"; // Filter files by extension 
                    dialog.Title = Resources.Resources.WindowCaption_Pack;
                    bool? showDialog = dialog.ShowDialog();
                    if (showDialog == true)
                    {
                        zip.Save(dialog.FileName);
                    }
                }
            }
        }

        private void ProcessReplaysFolders(List<ReplayFolder> replayFolders)
        {
            ProgressView.Execute(
                Resources.Resources.ProgressTitle_Loading_replays, (bw, we) =>
                {
                    List<ReplayFile> replayFiles = new List<ReplayFile>();

                    foreach (var replayFolder in replayFolders)
                    {
                        string folderPath = replayFolder.Path;

                        if (string.IsNullOrEmpty(folderPath))
                        {
                            continue;
                        }

                        if (Directory.Exists(folderPath))
                        {
                            string[] files = Directory.GetFiles(folderPath, "*.wotreplay");
                            List<FileInfo> replays =
                                files.Select(x => new FileInfo(Path.Combine(folderPath, x))).Where(x => x.Length > 0).ToList();

                            int count = replays.Count();

                            int index = 0;
                            foreach (FileInfo replay in replays)
                            {
                                try
                                {
                                    Domain.Replay.Replay data = WotApiClient.Instance.ReadReplay2Blocks(replay);
                                    if (data != null)
                                    {
                                        replayFiles.Add(new ReplayFile(replay, data, replayFolder.Id));
                                    }
                                }
                                catch (Exception e)
                                {
                                    _log.Error("Error on replay processing", e);
                                }
                                index++;
                                int percent = (index + 1) * 100 / count;
                                if (ProgressView.ReportWithCancellationCheck(bw, we, percent,
                                    Resources.Resources.ProgressLabel_Processing_file_format, index + 1, count, replay.Name))
                                {
                                    return;
                                }
                            }
                            replayFolder.Count = index;
                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressView.CheckForPendingCancellation(bw, we);
                        }
                    }

                    ReplayFolder root = ReplaysFolders.First();

                    root.Count = ReplaysFolders.GetAll().Skip(1).Sum(x => x.Count);

                    IList<ReplayEntity> dbReplays = DossierRepository.GetReplays();

                    dbReplays.Join(replayFiles, x => new { x.PlayerId, x.ReplayId }, y => new { y.PlayerId, y.ReplayId },
                        (x, y) => new { ReplayEntity = x, ReplayFile = y })
                        .ToList()
                        .ForEach(x => x.ReplayFile.Link = x.ReplayEntity.Link);

                    _replays.AddRange(replayFiles.OrderByDescending(x => x.PlayTime).ToList());
                    OnPropertyChanged("Replays");

                    ReplayFilter.SelectedFolder = replayFolders.FirstOrDefault();

                    ChartView.InitBattlesByMapChart();
                    ChartView.InitWinReplaysPercentByMapChart();

                }, new ProgressDialogSettings(true, true, false));
        }

        private void ReplayFilterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged("Replays");
        }

        private void OnReplayRowDoubleClick(object rowData)
        {
            ReplayFile replayFile = rowData as ReplayFile;

            if (replayFile != null)
            {
                string jsonFile = replayFile.FileInfo.FullName.Replace(replayFile.FileInfo.Extension, ".json");
                //convert dossier cache file to json
                if (!File.Exists(jsonFile))
                {
                    CacheHelper.ReplayToJson(replayFile.FileInfo);
                }

                if (!File.Exists(jsonFile))
                {
                    MessageBox.Show(Resources.Resources.Msg_Error_on_replay_file_read, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Domain.Replay.Replay replay = WotApiClient.Instance.ReadReplay(jsonFile);
                if (replay != null && replay.datablock_battle_result != null)
                {
                    ReplayViewModel viewModel = CompositionContainerFactory.Instance.GetExport<ReplayViewModel>();
                    viewModel.Init(replay, replayFile);
                    viewModel.Show();
                }
                else
                {
                    MessageBox.Show(Resources.Resources.Msg_File_incomplete_or_not_supported, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnReplayRowDelete(ReplayFile replayFile)
        {
            if (replayFile != null)
            {
                if (replayFile.FileInfo.Exists)
                {
                    try
                    {
#if !DEBUG
                        replayFile.FileInfo.Delete();
#endif
                        _replays.Remove(replayFile);
                        OnPropertyChanged("Replays");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.FileInfo.Name);
                        MessageBox.Show(string.Format(Resources.Resources.ErrorMsg_ErrorOnFileDeletion, replayFile.FileInfo.Name),
                            Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OnReplayRowsZip(object rowData)
        {
            ObservableCollection<object> selectedItems = rowData as ObservableCollection<object> ?? new ObservableCollection<object>();
            List<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();
            PackReplays(replays, "pack");
        }

        private void OnReplayRowsDelete(object rowData)
        {
            ObservableCollection<object> selectedItems = rowData as ObservableCollection<object> ?? new ObservableCollection<object>();
            IEnumerable<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();
            bool error = false;
            foreach (ReplayFile replayFile in replays)
            {
                try
                {
#if !DEBUG
                    replayFile.FileInfo.Delete();
#endif
                    _replays.Remove(replayFile);
                }
                catch (Exception e)
                {
                    _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.FileInfo.Name);
                    error = true;
                }
            }

            OnPropertyChanged("Replays");

            if (error)
            {
                MessageBox.Show(Resources.Resources.ErrorMsg_ErrorOnFilesDeletion,
                    Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUploadReplay(object row)
        {
            ReplayFile model = row as ReplayFile;
            return model != null && string.IsNullOrEmpty(model.Link);
        }

        private bool CanUploadReplays(object rows)
        {
            ObservableCollection<object> model = rows as ObservableCollection<object>;
            return model != null;
        }

        private void OnUploadReplay(ReplayFile replayFile)
        {
            if (replayFile != null)
            {
                UploadReplayViewModel viewModel = CompositionContainerFactory.Instance.GetExport<UploadReplayViewModel>();
                viewModel.ReplayFile = replayFile;
                viewModel.Show();
            }
        }

        private void OnUploadReplays(object rows)
        {
            ReplayUploader replayUploader = new ReplayUploader();

            ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ?? new ObservableCollection<object>();
            IEnumerable<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();

            using (new WaitCursor())
            {
                AppSettings appSettings = SettingsReader.Get();

                foreach (var replay in replays)
                {
                    WotReplaysSiteResponse response = replayUploader.Upload(replay.FileInfo, appSettings.PlayerId, appSettings.PlayerName);
                    if (response != null && response.Result == true)
                    {
                        replay.Link = response.Url;
                        //_repository.SaveReplay(ReplayFile.PlayerId, ReplayFile.ReplayId, ReplayFile.Link);    
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CallBackDataSource<T> : IEnumerable<T>
    {
        private readonly Func<List<T>> _func;

        public CallBackDataSource(Func<List<T>> func)
        {
            _func = func;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return _func().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
