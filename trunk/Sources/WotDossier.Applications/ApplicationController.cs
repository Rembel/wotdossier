using System;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications
{
    public class ApplicationController : Controller, IDisposable
    {
        private ShellViewModel _shellViewModel;
        private readonly DelegateCommand _exitCommand;

        #region Commands


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        public ApplicationController()
        {
            _exitCommand = new DelegateCommand(Close);
            
        }

        private void InitShellViewModel(ShellViewModel shellViewModel)
        {
            //shellViewModel.ExitCommand = _exitCommand;
        }

        public void Run(IShellView shellView)
        {
            _shellViewModel = new ShellViewModel(shellView, new DossierRepository(new DataProvider(new NHibernateSessionStorage())));
            InitShellViewModel(_shellViewModel);
            _shellViewModel.Show();
        }

        public void Dispose()
        {
        }

        private void Close()
        {
            _shellViewModel.Close();
        }
    }
}
