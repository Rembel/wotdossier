using System.Collections.Generic;
using WotDossier.Applications.View;
using WotDossier.Domain;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private SettingsReader _reader;
        private AppSettings _appSettings;
        private List<string> _servers = new List<string>{"ru", "eu"};
        public DelegateCommand SaveCommand { get; set; }

        public AppSettings AppSettings
        {
            get { return _appSettings; }
        }

        public List<string> Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }

        public SettingsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public SettingsViewModel(ISettingsView view) : base(view)
        {
            SaveCommand = new DelegateCommand(OnSave);
            _reader = new SettingsReader(WotDossierSettings.SettingsPath);
            _appSettings = _reader.Get();
        }

        private void OnSave()
        {
            _reader.Save(_appSettings);
            ViewTyped.Close();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> then this object is a child of another ViewModel.</param>
        public SettingsViewModel(ISettingsView view, bool isChild) : base(view, isChild)
        {
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }
    }
}
