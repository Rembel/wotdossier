using System;
using System.ComponentModel.Composition;
using WotDossier.Applications.ViewModel;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications
{
    [Export(typeof(ApplicationController))]
    public class ApplicationController : Controller, IDisposable
    {
        private ShellViewModel _shellViewModel;
        private readonly DelegateCommand _exitCommand;

        #region Commands


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        [ImportingConstructor]
        public ApplicationController([Import(typeof(ShellViewModel))]ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
            _exitCommand = new DelegateCommand(Close);
        }

        private void InitShellViewModel(ShellViewModel shellViewModel)
        {
            //shellViewModel.ExitCommand = _exitCommand;
        }

        public void Run()
        {
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
