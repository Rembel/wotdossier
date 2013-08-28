using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using WotDossier.Applications.View;

namespace WotDossier.Views
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
            KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
