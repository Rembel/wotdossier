using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WotDossier.Applications;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationController _controller;

        protected override void OnStartup(StartupEventArgs e)
        {
            // start application
            _controller = new ApplicationController();
            _controller.Run(new MainWindow());

            base.OnStartup(e);
        }
    }
}
