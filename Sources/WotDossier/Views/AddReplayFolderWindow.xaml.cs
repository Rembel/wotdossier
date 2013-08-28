using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.View;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for AddReplayFolder.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IAddReplayFolderView))]
    public partial class AddReplayFolderWindow : Window, IAddReplayFolderView
    {
        public AddReplayFolderWindow()
        {
            InitializeComponent();
        }
    }
}
