using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Framework;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IShellView))]
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();

            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }

        private void PrintWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int i = 0;

            string fileName = string.Format(AppConfigSettings.FILE_NAME_FORMAT, i);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), fileName);

            while (File.Exists(path))
            {
                i++;
                fileName = string.Format(AppConfigSettings.FILE_NAME_FORMAT, i);
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), fileName);
            }

            Util.SaveWindow(this,96, path);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            tcMain.SelectedItem = tabReplays;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tcMain.SelectedIndex = 0;
        }
    }
}