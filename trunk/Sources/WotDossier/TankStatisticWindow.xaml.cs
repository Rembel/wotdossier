using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.View;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for TankStatisticWindow.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ITankStatisticView))]
    public partial class TankStatisticWindow : Window, ITankStatisticView
    {
        public TankStatisticWindow()
        {
            InitializeComponent();
        }
    }
}
