using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using WotDossier.Applications.View;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for ClanWindow.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IClanView))]
    public partial class ClanWindow : Window, IClanView
    {
        public ClanWindow()
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
