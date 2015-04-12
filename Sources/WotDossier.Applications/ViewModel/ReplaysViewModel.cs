using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Framework;
using WotDossier.Framework.Controls.DataGrid;
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
        public DelegateCommand<ReplayFile> CopyFileNameToClipboardCommand { get; set; }
        public DelegateCommand<ReplayFile> PlayReplayCommand { get; set; }
        public DelegateCommand<ReplayFile> PlayReplayWithCommand { get; set; }
        public DelegateCommand<ReplayFile> ShowFileInFolderCommand { get; set; }
        public DelegateCommand<ReplayFile> ShowDetailsCommand { get; set; }
        public DelegateCommand<object> ReplayRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> ReplayRowsDeleteCommand { get; set; }
        public DelegateCommand<object> ReplayRowsZipCommand { get; set; }

        public DelegateCommand<ReplayFolder> AddFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> ZipFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> DeleteFolderCommand { get; set; }

        private List<ReplayFile> _replays = new List<ReplayFile>();

        public IList SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged("SelectedItems");
            }
        }

        /// <summary>
        /// Gets or sets the replays.
        /// </summary>
        /// <value>
        /// The replays.
        /// </value>
        public List<ReplayFile> Replays
        {
            get
            {
                var replayFiles = ReplayFilter.Filter(_replays);

                if (replayFiles.Any())
                {
                    ReplaysSummary = new List<TotalReplayFile> { new TotalReplayFile(replayFiles, Guid.NewGuid()) };
                }

                return replayFiles;
            }
            set
            {
                _replays = value;
                OnPropertyChanged("Replays");
            }
        }

        public List<TotalReplayFile> ReplaysSummary
        {
            get { return _replaysSummary; }
            set
            {
                _replaysSummary = value;
                OnPropertyChanged("ReplaysSummary");
            }
        }

        private List<ReplayFolder> _replaysFolders;
        private Guid? _selectedFolderId = null;
        private bool _processing;
        private List<TotalReplayFile> _replaysSummary;
        private ObservableCollection<ColumnInformation> _columnInfo;
        private IList _selectedItems;

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

        public ObservableCollection<ColumnInformation> ColumnInfo
        {
            get { return _columnInfo; }
            set
            {
                _columnInfo = value;
                OnPropertyChanged("ColumnInfo");
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
            ShowFileInFolderCommand = new DelegateCommand<ReplayFile>(OnShowFileInFolder, CanOnShowFileInFolder);
            ShowDetailsCommand = new DelegateCommand<ReplayFile>(OnReplayRowDoubleClick, CanShowDetails);
            CopyFileNameToClipboardCommand = new DelegateCommand<ReplayFile>(OnCopyFileNameToClipboard, CanCopyFileNameToClipboard);

            AddFolderCommand = new DelegateCommand<ReplayFolder>(OnAddFolder);
            ZipFolderCommand = new DelegateCommand<ReplayFolder>(OnZipFolder);
            DeleteFolderCommand = new DelegateCommand<ReplayFolder>(OnDeleteFolderCommand);

            ReplayFilter = new ReplaysFilterViewModel();
            ReplayFilter.FilterChanged += ReplayFilterOnPropertyChanged;

            ReplaysManager = new ReplaysManager();

            PlayReplayCommand = new DelegateCommand<ReplayFile>(ReplaysManager.Play);
            PlayReplayWithCommand = new DelegateCommand<ReplayFile>(ReplaysManager.PlayWith);

            DossierRepository = dossierRepository;
            ProgressView = progressControlView;
            playerChartsViewModel.ReplaysDataSource = new CallbackDataSource<ReplayFile>(() => _replays);
            ChartView = playerChartsViewModel;

            LoadListSettings();

            EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Subscribe(OnReplayFileMove);
        }

        private void LoadListSettings()
        {
            AppSettings appSettings = SettingsReader.Get();
            if (!string.IsNullOrEmpty(appSettings.ColumnInfo))
            {
                try
                {
                    ColumnInfo = XmlSerializer.LoadObjectFromXml<ObservableCollection<ColumnInformation>>(appSettings.ColumnInfo);
                }
                catch (Exception e)
                {
                    _log.Error("Error on grid configuration load", e);
                }
            }
        }

        private bool CanCopyFileNameToClipboard(ReplayFile replay)
        {
            return replay is PhisicalReplay;
        }

        private void OnCopyFileNameToClipboard(ReplayFile replay)
        {
            Clipboard.SetText(replay.Name);
        }

        private bool CanShowDetails(ReplayFile replay)
        {
            return replay != null;
        }

        private bool CanOnShowFileInFolder(ReplayFile replay)
        {
            return replay is PhisicalReplay;
        }

        private void OnShowFileInFolder(ReplayFile replay)
        {
            if (replay is PhisicalReplay)
            {
                Process PrFolder = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                string file = replay.PhisicalPath;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.FileName = "explorer";
                psi.Arguments = @"/n, /select, " + file;
                PrFolder.StartInfo = psi;
                PrFolder.Start();

                //Process PrFolder = new Process();
                //ProcessStartInfo psi = new ProcessStartInfo();
                //string file = replay.PhisicalPath;
                //psi.CreateNoWindow = true;
                //psi.WindowStyle = ProcessWindowStyle.Normal;
                //psi.FileName = "totalcmd.exe";
                //psi.Arguments = @"/n, /select, " + file;
                //PrFolder.StartInfo = psi;
                //PrFolder.Start();
            }
        }

        /// <summary>
        /// Loads the replays list.
        /// </summary>
        public void LoadReplaysList()
        {
            if (_processing)
            {
                return;
            }

            _processing = true;

            if (ReplayFilter.SelectedFolder != null)
            {
                _selectedFolderId = ReplayFilter.SelectedFolder.Id;
            }

            if (ReplaysFolders == null)
            {
                ReplaysFolders = ReplaysManager.GetFolders();
            }
            //_replays.Clear();

            List<ReplayFolder> replayFolders = ReplaysFolders.GetAll();

            ProcessReplaysFoldersInBackground(replayFolders);
        }

        private void OnReplayFileMove(ReplayFileMoveEventArgs eventArgs)
        {
            using (new WaitCursor())
            {
                foreach (var replayFile in eventArgs.ReplayFiles)
                {
                    PhisicalReplay phisicalReplay = replayFile as PhisicalReplay;

                    if (ReplayFilter.SelectedFolder != eventArgs.TargetFolder && phisicalReplay != null)
                    {
                        phisicalReplay.Move(eventArgs.TargetFolder);
                        phisicalReplay.FolderId = eventArgs.TargetFolder.Id;
                        eventArgs.TargetFolder.Count += 1;
                        ReplayFilter.SelectedFolder.Count -= 1;
                    }
                }
                OnPropertyChanged("Replays");
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
            try
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
            catch (Exception e)
            {
                _log.Error("Error on copy link to clipboard", e);
            }
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
                ReplayFolder root = ReplaysFolders.First();
                ReplayFolder parent = FindParentFolder(root, folder);
                if (parent != null)
                {
                    _replays.RemoveAll(x => x.FolderId == folder.Id);
                    parent.Folders.Remove(folder);
                    root.Count = _replays.Count;
                    ReplaysManager.SaveFolder(root);
                    OnPropertyChanged("Replays");
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
                ProcessReplaysFoldersInBackground(new List<ReplayFolder> { viewModel.ReplayFolder });
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
                using (ZipFile zip = new ZipFile(Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
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

        private void ProcessReplaysFoldersInBackground(List<ReplayFolder> replayFolders)
        {
            ProgressView.Execute(
                Resources.Resources.ProgressTitle_Loading_replays, (bw, we) =>
                {
                    Reporter reporter = new Reporter(bw, we, ProgressView);
                    ProcessReplaysFolders(replayFolders, reporter);
                }, new ProgressDialogSettings(true, true, false));
        }

        public void ProcessReplaysFolders(List<ReplayFolder> replayFolders, IReporter reporter)
        {
            try
            {
                CultureHelper.SetUiCulture();

                foreach (var replayFolder in replayFolders)
                {
                    string folderPath = replayFolder.Path;

                    if (string.IsNullOrEmpty(folderPath))
                    {
                        continue;
                    }

                    if (Directory.Exists(folderPath))
                    {
                        _log.WarnFormat("replays before update count: {0}", _replays.Count());

                        string[] newFiles = Directory.GetFiles(folderPath, "*.wotreplay").Where(x => !x.EndsWith("temp.wotreplay", StringComparison.InvariantCultureIgnoreCase)).ToArray();

                        _log.WarnFormat("new files count: {0}", newFiles.Count());

                        List<string> oldFiles = replayFolder.Files;

                        _log.WarnFormat("old files count: {0}", oldFiles.Count());

                        //get operations for replays list update
                        var operations = GetUpdateOperations(replayFolder.Id, oldFiles, newFiles);

                        _log.WarnFormat("operations count: {0}", operations.Count());

                        int count = operations.Count();
                        int index = 0;
                        foreach (var operation in operations)
                        {
                            //perform operation
                            operation.Perform(_replays);
                            FileInfo replay = new FileInfo(operation.Item);
                            int percent = (index + 1) * 100 / count;
                            reporter.Report(percent, Resources.Resources.ProgressLabel_Processing_file_format, index + 1, count, replay.Name);
                            index++;
                        }

                        _log.WarnFormat("replays after update count: {0}", _replays.Count());

                        replayFolder.Files = newFiles.ToList();
                        replayFolder.Count = _replays.Count(x => x.FolderId == replayFolder.Id);
                    }
                }

                ReplayFolder root = ReplaysFolders.First();

                root.Count = ReplaysFolders.GetAll().Skip(1).Sum(x => x.Count);

                IList<ReplayEntity> dbReplays = DossierRepository.GetReplays();
                dbReplays.Join(_replays, x => new { x.PlayerId, x.ReplayId }, y => new { y.PlayerId, y.ReplayId },
                    (x, y) => new { ReplayEntity = x, ReplayFile = y })
                    .ToList()
                    .ForEach(x =>
                    {
                        x.ReplayFile.Link = x.ReplayEntity.Link;
                        x.ReplayFile.Comment = x.ReplayEntity.Comment;
                    });

                //add db replays
                List<DbReplay> collection =
                    dbReplays.Where(x => x.Raw != null)
                        .Select(
                            x =>
                                new DbReplay(CompressHelper.DecompressObject<Domain.Replay.Replay>(x.Raw),
                                    ReplaysManager.DeletedFolder.Id))
                        .ToList();

                _replays.RemoveAll(x => x.FolderId == ReplaysManager.DeletedFolder.Id);
                _replays.AddRange(collection);

                //sort
                _replays = _replays.OrderByDescending(x => x.PlayTime).ToList();

                //add folder for deleted replays
                ReplayFolder deletedFolder =
                    root.Folders.FirstOrDefault(x => x.Id == ReplaysManager.DeletedFolder.Id);
                if (deletedFolder == null)
                {
                    deletedFolder = ReplaysManager.DeletedFolder;
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke((Action)(() => root.Folders.Add(deletedFolder)));
                    }
                }
                deletedFolder.Count = collection.Count;

                reporter.Report(100, Resources.Resources.Progress_DataLoadCompleted);

                //restore folder selection
                ReplayFilter.SelectedFolder =
                    replayFolders.FirstOrDefault(x => x.Id == _selectedFolderId
                        || ReplayFilter.SelectedFolder != null && x.Id == ReplayFilter.SelectedFolder.Id) ?? root;

                ChartView.InitReplaysStat();
                //ChartView.InitBattlesByMapChart();
                //ChartView.InitWinReplaysPercentByMapChart();
            }
            finally
            {
                _processing = false;
            }
        }

        private List<ListUpdateOperation<ReplayFile>> GetUpdateOperations(Guid folderId, IEnumerable<string> oldFiles, string[] newList)
        {
            List<ListUpdateOperation<ReplayFile>> result = new List<ListUpdateOperation<ReplayFile>>();
            List<ListUpdateOperation<ReplayFile>> toDel = oldFiles.Except(newList).Select(x => (ListUpdateOperation<ReplayFile>)new DeleteOperation(x)).ToList();
            List<ListUpdateOperation<ReplayFile>> toAdd = newList.Except(oldFiles).Select(x => (ListUpdateOperation<ReplayFile>)new AddOperation(x, folderId)).ToList();
            result.AddRange(toDel);
            result.AddRange(toAdd);
            return result;
        }

        abstract class ListUpdateOperation<T>
        {
            private readonly string _item;

            public string Item
            {
                get { return _item; }
            }

            protected ListUpdateOperation(string item)
            {
                _item = item;
            }

            public abstract void Perform(List<T> targetList);
        }

        class AddOperation : ListUpdateOperation<ReplayFile>
        {
            private readonly Guid _folder;
            private readonly FileInfo _file;

            public FileInfo File
            {
                get { return _file; }
            }

            public AddOperation(string item, Guid folder)
                : base(item)
            {
                _folder = folder;
                _file = new FileInfo(Item);
            }

            public override void Perform(List<ReplayFile> targetList)
            {
                try
                {
                    Domain.Replay.Replay data = ReplayFileHelper.ParseReplay_8_11(File);
                    if (data != null)
                    {
                        targetList.Add(new PhisicalReplay(File, data, _folder));
                    }
                    else
                    {
                        _log.WarnFormat("Null data for file. Path - [{0}]", File.FullName);
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat("Error on replay processing. Path - [{0}]", File.FullName, e);
                }
            }
        }

        class DeleteOperation : ListUpdateOperation<ReplayFile>
        {
            public DeleteOperation(string item)
                : base(item)
            {
            }

            public override void Perform(List<ReplayFile> targetList)
            {
                targetList.RemoveAll(x => x.PhisicalPath == Item);
            }
        }

        private void ReplayFilterOnPropertyChanged()
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
                            DossierRepository.SaveReplay(replayFile.PlayerId, replayFile.ReplayId, replayFile.Link, replayData);
                            _replays.Add(new DbReplay(replayData, ReplaysManager.DeletedFolder.Id));
                        }

                        replayFile.Delete();
                        _replays.Remove(replayFile);

                        ReplayFolder replayFolder = _replaysFolders.GetAll().First(x => x.Id == replayFile.FolderId);
                        replayFolder.Files.Remove(replayFile.PhisicalPath);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.Name);
                        error = true;
                    }
                }
                var root = _replaysFolders.First();
                root.Count = _replays.Count;
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
