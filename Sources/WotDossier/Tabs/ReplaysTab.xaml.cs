using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for ReplaysTab.xaml
    /// </summary>
    public partial class ReplaysTab : UserControl
    {
        public ReplaysTab()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
        }
    }
}
