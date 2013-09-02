using System.ComponentModel.Composition;
using System.IO;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.View;
using WotDossier.Domain;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(AddReplayFolderViewModel))]
    public class AddReplayFolderViewModel : ViewModel<IAddReplayFolderView>
    {
        private string _replaysFolderPath;
        private string _folderName;
        private ReplayFolder _replayFolder;
        public DelegateCommand OkCommand { get; set; }
        public DelegateCommand SelectReplaysFolderCommand { get; set; }

        public string ReplaysFolderPath
        {
            get { return _replaysFolderPath; }
            set
            {
                _replaysFolderPath = value;
                RaisePropertyChanged("ReplaysFolderPath");
            }
        }

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                RaisePropertyChanged("FolderName");
            }
        }

        public ReplayFolder ReplayFolder
        {
            get { return _replayFolder; }
            set { _replayFolder = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AddReplayFolderViewModel"/> class.
        /// </summary>
        public AddReplayFolderViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> then this object is a child of another ViewModel.</param>
        public AddReplayFolderViewModel(IAddReplayFolderView view, bool isChild)
            : base(view, isChild)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public AddReplayFolderViewModel([Import(typeof(IAddReplayFolderView))]IAddReplayFolderView view)
            : base(view)
        {
            OkCommand = new DelegateCommand(OnOk);
            SelectReplaysFolderCommand = new DelegateCommand(OnSelectReplaysFolder);
        }

        private void OnSelectReplaysFolder()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            bool? showDialog = dialog.ShowDialog();
            if (showDialog == true)
            {
                ReplaysFolderPath = dialog.SelectedPath;
                FolderName = new DirectoryInfo(ReplaysFolderPath).Name;
            }
        }

        private void OnOk()
        {
            if (!string.IsNullOrEmpty(FolderName) && !string.IsNullOrEmpty(ReplaysFolderPath))
            {
                ReplayFolder = new ReplayFolder {Name = FolderName, Path = ReplaysFolderPath};
                ViewTyped.Close();    
            }
        }

        public virtual void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
