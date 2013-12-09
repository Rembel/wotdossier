using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using WotDossier.Applications.View;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for PlayerStatisticWindow.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IPlayerServerStatisticView))]
    public partial class PlayerServerStatisticWindow : Window, IPlayerServerStatisticView
    {
        public PlayerServerStatisticWindow()
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

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null && hyperlink.NavigateUri != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
        }
    }
}
