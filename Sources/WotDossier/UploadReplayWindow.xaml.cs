using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.View;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for UploadReplayWindow.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IUploadReplayView))]
    public partial class UploadReplayWindow : Window, IUploadReplayView
    {
        public UploadReplayWindow()
        {
            InitializeComponent();
        }
    }
}
