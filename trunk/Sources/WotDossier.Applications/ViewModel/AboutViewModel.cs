using System.ComponentModel.Composition;
using System.Diagnostics;
using Common.Logging;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(AboutViewModel))]
    public class AboutViewModel : ViewModel<IAboutView>
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public DelegateCommand CheckUpdateCommand { get; set; }
        public DelegateCommand SysInfoCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public AboutViewModel([Import(typeof(IAboutView))]IAboutView view)
            : base(view)
        {
            CheckUpdateCommand = new DelegateCommand(OnCheckUpdate);
            SysInfoCommand = new DelegateCommand(OnSysInfo);
        }

        private void OnCheckUpdate()
        {
            using (new WaitCursor())
            {
                UpdateChecker.CheckNewVersionAvailable();
            }
        }

        private void OnSysInfo()
        {
            Process.Start("sysdm.cpl");
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
