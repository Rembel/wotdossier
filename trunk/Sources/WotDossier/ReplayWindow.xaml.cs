using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.View;

namespace WotDossier
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
        }
    }
}
