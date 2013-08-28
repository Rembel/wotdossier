using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using WotDossier.Applications.View;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ISettingsView))]
    public partial class SettingsWindow : Window, ISettingsView
    {
        public SettingsWindow()
        {
            InitializeComponent();
            KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
