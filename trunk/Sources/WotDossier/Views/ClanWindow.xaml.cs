using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WotDossier.Applications.View;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for ClanWindow.xaml
    /// </summary>
    [Export(typeof(IClanView))]
    public partial class ClanWindow : Window
    {
        public ClanWindow()
        {
            InitializeComponent();
        }
    }
}
