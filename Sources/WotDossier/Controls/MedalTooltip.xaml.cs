using System.Windows;
using System.Windows.Controls;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for MedalTooltip.xaml
    /// </summary>
    public partial class MedalTooltip : UserControl
    {
        #region public string Header

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (MedalTooltip), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string), typeof (MedalTooltip), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion public string Header

        public MedalTooltip()
        {
            InitializeComponent();
        }
    }
}
