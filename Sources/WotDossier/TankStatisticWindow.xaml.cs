using System.Windows;
using WotDossier.Applications.View;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for TankStatisticWindow.xaml
    /// </summary>
    public partial class TankStatisticWindow : Window, ITankStatisticView
    {
        public TankStatisticWindow()
        {
            InitializeComponent();
        }
    }
}
