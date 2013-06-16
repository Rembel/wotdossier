using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for TextWithImage.xaml
    /// </summary>
    public partial class TextWithImage : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (TextWithImage), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(TextWithImage), new PropertyMetadata(default(string)));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public TextWithImage()
        {
            InitializeComponent();
        }
    }
}
