using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Security.Authentication;
using System.Windows;
using System.Windows.Input;
using Common.Logging;
using WotDossier.Applications.Logic;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel.Replay
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (UploadReplayViewModel))]
    public class UploadReplayViewModel : ViewModel<IUploadReplayView>
    {
        private readonly DossierRepository _repository;
        private static readonly ILog _log = LogManager.GetLogger("UploadReplayViewModel");

        public DelegateCommand OnReplayUploadCommand { get; set; }

        public ReplayFile ReplayFile { get; set; }

        public string ReplayDescription { get; set; }

        public string ReplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="repository">The repository.</param>
        [ImportingConstructor]
        public UploadReplayViewModel([Import(typeof(IUploadReplayView))]IUploadReplayView view, [Import(typeof(DossierRepository))]DossierRepository repository)
            : base(view)
        {
            _repository = repository;
            OnReplayUploadCommand = new DelegateCommand(OnReplayUpload);
        }

        private void OnReplayUpload()
        {
            AppSettings appSettings = SettingsReader.Get();

            if (ReplayFile != null)
            {
                try
                {
                    Mouse.SetCursor(Cursors.Wait);
                    ReplayUploader replayUploader = new ReplayUploader();
                    ReplayFile.Link = replayUploader.Upload(ReplayFile.FileInfo, ReplayName, ReplayDescription,
                        string.Format(appSettings.ReplaysUploadServerPath, appSettings.Server, appSettings.PlayerId, appSettings.PlayerName));
                    _repository.SaveReplay(ReplayFile.PlayerId, ReplayFile.ReplayId, ReplayFile.Link);
                    ViewTyped.Close();
                }
                catch (AuthenticationException e)
                {
                    _log.Error("Authentication error", e);
                    MessageBoxResult result =
                        MessageBox.Show(Resources.Resources.Msg_ReplayUpload_AuthentificationFailure,
                            Resources.Resources.WindowCaption_AuthFailure, MessageBoxButton.YesNo, MessageBoxImage.Error);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Internet Explorer\iexplore.exe");
                        proc.StartInfo.Arguments = string.Format("http://wotreplays.{0}", appSettings.Server);
                        proc.Start();
                    }
                }
                finally
                {
                    Mouse.SetCursor(Cursors.Arrow);   
                }
            }
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
