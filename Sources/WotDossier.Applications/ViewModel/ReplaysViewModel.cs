using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Common.Logging;
using Ionic.Zip;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.Events;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Chart;
using WotDossier.Applications.ViewModel.Filter;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Framework;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;
using WotReplaysSiteResponse = WotDossier.Applications.Logic.WotReplaysSiteResponse;

namespace WotDossier.Applications.ViewModel
{
    public class ReplaysViewModel : INotifyPropertyChanged
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public ReplaysFilterViewModel ReplayFilter { get; set; }
        public ReplaysManager ReplaysManager { get; set; }
        public DossierRepository DossierRepository { get; set; }
        public ProgressControlViewModel ProgressView { get; set; }
        public PlayerChartsViewModel ChartView { get; set; }

        public DelegateCommand<ReplayFile> ReplayUploadCommand { get; set; }
        public DelegateCommand<object> ReplaysUploadCommand { get; set; }
        public DelegateCommand<object> CopyLinkToClipboardCommand { get; set; }
        public DelegateCommand<ReplayFile> PlayReplayCommand { get; set; }
        public DelegateCommand<object> ReplayRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> ReplayRowsDeleteCommand { get; set; }
        public DelegateCommand<object> ReplayRowsZipCommand { get; set; }

        public DelegateCommand<ReplayFolder> AddFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> ZipFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> DeleteFolderCommand { get; set; }

        private List<ReplayFile> _replays = new List<ReplayFile>();

        /// <summary>
        /// Gets or sets the replays.
        /// </summary>
        /// <value>
        /// The replays.
        /// </value>
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
        /// <summary>
        /// Gets or sets the replays folders.
        /// </summary>
        /// <value>
        /// The replays folders.
        /// </value>
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
            ReplayRowsDeleteCommand = new DelegateCommand<object>(OnReplayRowsDelete);
            ReplayRowsZipCommand = new DelegateCommand<object>(OnReplayRowsZip);
            CopyLinkToClipboardCommand = new DelegateCommand<object>(OnCopyLinkToClipboard, CanCopyLinkToClipboard);
            PlayReplayCommand = new DelegateCommand<ReplayFile>(OnPlay);

            AddFolderCommand = new DelegateCommand<ReplayFolder>(OnAddFolder);
            ZipFolderCommand = new DelegateCommand<ReplayFolder>(OnZipFolder);
            DeleteFolderCommand = new DelegateCommand<ReplayFolder>(OnDeleteFolderCommand);

            ReplayFilter = new ReplaysFilterViewModel();
            ReplayFilter.PropertyChanged += ReplayFilterOnPropertyChanged;

            ReplaysManager = new ReplaysManager();
            DossierRepository = dossierRepository;
            ProgressView = progressControlView;
            playerChartsViewModel.ReplaysDataSource = new CallbackDataSource<ReplayFile>(() => _replays);
            ChartView = playerChartsViewModel;

            EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Subscribe(OnReplayFileMove);
        }

        private void OnPlay(ReplayFile replayFile)
        {
            replayFile.Play();
        }

        /// <summary>
        /// Loads the replays list.
        /// </summary>
        public void LoadReplaysList()
        {
            ReplaysFolders = ReplaysManager.GetFolders();
            _replays.Clear();

            List<ReplayFolder> replayFolders = ReplaysFolders.GetAll();

            ProcessReplaysFolders(replayFolders);
        }

        private void OnReplayFileMove(ReplayFileMoveEventArgs eventArgs)
        {
            using (new WaitCursor())
            {
                if (ReplayFilter.SelectedFolder != eventArgs.TargetFolder && eventArgs.ReplayFile is PhisicalReplay)
                {
                    eventArgs.ReplayFile.Move(eventArgs.TargetFolder);
                    eventArgs.ReplayFile.FolderId = eventArgs.TargetFolder.Id;
                    OnPropertyChanged("Replays");
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

        private void OnCopyLinkToClipboard(object rows)
        {
            ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ?? new ObservableCollection<object>();

            List<ReplayFile> replayFiles = selectedItems.Cast<ReplayFile>().ToList();

            StringBuilder builder = new StringBuilder();
            foreach (ReplayFile replay in replayFiles)
            {
                builder.AppendLine(string.Format("{0}, {1}", replay.TankName, replay.MapName));
                builder.AppendLine(replay.Link);
                builder.AppendLine();
            }

            Clipboard.SetText(builder.ToString());
        }

        private bool CanCopyLinkToClipboard(object rows)
        {
            ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ?? new ObservableCollection<object>();
            return selectedItems.Cast<ReplayFile>().ToList().Any(x => !string.IsNullOrEmpty(x.Link));
        }

        private void OnDeleteFolderCommand(ReplayFolder folder)
        {
            if (folder.Id != ReplaysManager.DeletedFolder.Id)
            {
                ReplayFolder root = ReplaysFolders.FirstOrDefault();
                ReplayFolder parent = FindParentFolder(root, folder);
                if (parent != null)
                {
                    parent.Folders.Remove(folder);
                    ReplaysManager.SaveFolder(root);
                }
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
                        string phisicalPath = replayFile.PhisicalPath;
                        if (!string.IsNullOrEmpty(phisicalPath))
                        {
                            zip.AddFile(phisicalPath, @"\" + name);
                        }
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
                    CultureHelper.SetUiCulture();

                    IList<ReplayEntity> dbReplays = DossierRepository.GetReplays();

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
                            int processedCount = 0;
                            foreach (FileInfo replay in replays)
                            {
                                try
                                {
                                    Domain.Replay.Replay data = WotFileHelper.ParseReplay_8_11(replay);
                                    if (data != null)
                                    {
                                        replayFiles.Add(new PhisicalReplay(replay, data, replayFolder.Id));
                                        processedCount++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    _log.ErrorFormat("Error on replay processing. Path - [{0}]", replay.FullName, e);
                                }
                                index++;
                                int percent = (index + 1) * 100 / count;
                                if (ProgressView.ReportWithCancellationCheck(bw, we, percent,
                                    Resources.Resources.ProgressLabel_Processing_file_format, index + 1, count, replay.Name))
                                {
                                    return;
                                }
                            }
                            replayFolder.Count = processedCount;
                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressView.CheckForPendingCancellation(bw, we);
                        }
                    }

                    ReplayFolder root = ReplaysFolders.First();

                    root.Count = ReplaysFolders.GetAll().Skip(1).Sum(x => x.Count);

                    dbReplays.Join(replayFiles, x => new { x.PlayerId, x.ReplayId }, y => new { y.PlayerId, y.ReplayId },
                        (x, y) => new { ReplayEntity = x, ReplayFile = y })
                        .ToList()
                        .ForEach(x => x.ReplayFile.Link = x.ReplayEntity.Link);

                    //add phisical replays
                    _replays.AddRange(replayFiles.OrderByDescending(x => x.PlayTime).ToList());

                    //add db replays
                    List<DbReplay> collection = dbReplays.Where(x => x.Raw != null).Select(x => new DbReplay(CompressHelper.DecompressObject<Domain.Replay.Replay>(x.Raw), ReplaysManager.DeletedFolder.Id)).ToList();
                    _replays.AddRange(collection);

                    //add folder for deleted replays
                    ReplayFolder deletedFolder = root.Folders.FirstOrDefault(x => x.Id == ReplaysManager.DeletedFolder.Id);
                    if (deletedFolder == null)
                    {
                        deletedFolder = ReplaysManager.DeletedFolder;
                        Application.Current.Dispatcher.Invoke((Action)(() => root.Folders.Add(deletedFolder)));
                    }
                    deletedFolder.Count = collection.Count;

                    ProgressView.Report(bw, 100, Resources.Resources.Progress_DataLoadCompleted);

                    //refresh replays
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
                Domain.Replay.Replay replay;
                
                using (new WaitCursor())
                {
                    replay = replayFile.ReplayData(SettingsReader.Get().ShowExtendedReplaysData);
                }

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

        private void OnReplayRowsZip(object rows)
        {
            using (new WaitCursor())
            {
                ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ?? new ObservableCollection<object>();
                List<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();
                PackReplays(replays, "pack");
            }
        }

        private void OnReplayRowsDelete(object rows)
        {
            using (new WaitCursor())
            {
                ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ??
                                                             new ObservableCollection<object>();
                IEnumerable<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();

                MessageBoxResult delete;
                if (Keyboard.Modifiers == ModifierKeys.Shift || replays.All(x => x is DbReplay))
                {
                    //use complete delete
                    delete = MessageBoxResult.No;
                }
                else
                {
                    delete = MessageBox.Show(Resources.Resources.Msg_ReplaysDelete,
                        Resources.Resources.WindowCaption_Information,
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);

                    if (delete == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }


                bool error = false;
                foreach (ReplayFile replayFile in replays)
                {
                    try
                    {
                        if (delete == MessageBoxResult.Yes)
                        {
                            Domain.Replay.Replay replayData = replayFile.ReplayData();
                            DossierRepository.SaveReplay(replayFile.PlayerId, replayFile.ReplayId, replayFile.Link,
                                replayData);
                            _replays.Add(new DbReplay(replayData, ReplaysManager.DeletedFolder.Id));
                        }

                        replayFile.Delete();
                        _replays.Remove(replayFile);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.Name);
                        error = true;
                    }
                }

                OnPropertyChanged("Replays");

                if (error)
                {
                    MessageBox.Show(Resources.Resources.Msg_ErrorOnFilesDeletion,
                        Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanUploadReplay(object row)
        {
            ReplayFile model = row as ReplayFile;
            return model != null && model.PhisicalFile != null && string.IsNullOrEmpty(model.Link);
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
            using (new WaitCursor())
            {
                ReplayUploader replayUploader = new ReplayUploader();

                ObservableCollection<object> selectedItems = rows as ObservableCollection<object> ??
                                                             new ObservableCollection<object>();
                IEnumerable<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();

                using (new WaitCursor())
                {
                    AppSettings appSettings = SettingsReader.Get();

                    foreach (var replay in replays)
                    {
                        if (replay.PhisicalFile != null)
                        {
                            WotReplaysSiteResponse response = replayUploader.Upload(replay.PhisicalFile,
                                appSettings.PlayerId, appSettings.PlayerName);
                            if (response != null && response.Result == true)
                            {
                                replay.Link = response.Url;
                                DossierRepository.SaveReplay(replay.PlayerId, replay.ReplayId, replay.Link);
                            }
                        }
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
}
