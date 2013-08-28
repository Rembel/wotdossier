using System.Diagnostics;
using System.Windows;

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
    }
}
