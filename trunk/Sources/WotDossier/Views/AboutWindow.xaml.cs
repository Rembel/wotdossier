using System.Diagnostics;
using System.Windows;
using WotDossier.Applications.Update;
using WotDossier.Framework;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void buttonSysInfo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("sysdm.cpl");
        }

        private void buttonCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor())
            {
                UpdateChecker.CheckNewVersionAvailable();
            }
        }
    }
}
