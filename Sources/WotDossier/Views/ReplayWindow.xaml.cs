﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Framework;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for ReplayWindow.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IReplayView))]
    public partial class ReplayWindow : Window, IReplayView
    {
        public ReplayWindow()
        {
            InitializeComponent();
            //Owner = Application.Current.MainWindow;
            KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
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

            Util.SaveWindow(this, 96, path);
        }
    }
}
