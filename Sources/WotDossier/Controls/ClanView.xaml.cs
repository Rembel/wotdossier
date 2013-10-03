using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for ClanView.xaml
    /// </summary>
    public partial class ClanView : UserControl
    {
        public static readonly DependencyProperty AbbreviationProperty =
            DependencyProperty.Register("Abbreviation", typeof (string), typeof (ClanView), new PropertyMetadata(default(string)));

        public string Abbreviation
        {
            get { return (string) GetValue(AbbreviationProperty); }
            set { SetValue(AbbreviationProperty, value); }
        }

        public static readonly DependencyProperty ClanNameProperty =
            DependencyProperty.Register("ClanName", typeof (string), typeof (ClanView), new PropertyMetadata(default(string)));

        public string ClanName
        {
            get { return (string) GetValue(ClanNameProperty); }
            set { SetValue(ClanNameProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof (string), typeof (ClanView), new PropertyMetadata(default(string)));

        public string Position
        {
            get { return (string) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty DaysProperty =
            DependencyProperty.Register("Days", typeof (int), typeof (ClanView), new PropertyMetadata(default(int)));

        public int Days
        {
            get { return (int) GetValue(DaysProperty); }
            set { SetValue(DaysProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof (DateTime), typeof (ClanView), new PropertyMetadata(default(DateTime)));

        public DateTime Date
        {
            get { return (DateTime) GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty ClanImageSourceProperty =
            DependencyProperty.Register("ClanImageSource", typeof (ImageSource), typeof (ClanView), new PropertyMetadata(default(ImageSource)));

        public ImageSource ClanImageSource
        {
            get { return (ImageSource) GetValue(ClanImageSourceProperty); }
            set { SetValue(ClanImageSourceProperty, value); }
        }

        public static readonly DependencyProperty OpenClanCommandProperty =
            DependencyProperty.Register("OpenClanCommand", typeof (ICommand), typeof (ClanView), new PropertyMetadata(default(ICommand)));

        public ICommand OpenClanCommand
        {
            get { return (ICommand) GetValue(OpenClanCommandProperty); }
            set { SetValue(OpenClanCommandProperty, value); }
        }

        public ClanView()
        {
            InitializeComponent();
        }

        private void On_ClanClick(object sender, RoutedEventArgs e)
        {
            if (OpenClanCommand != null)
            {
                OpenClanCommand.Execute(null);
            }
        }
    }
}
