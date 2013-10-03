using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for CompareRow.xaml
    /// </summary>
    public partial class CompareRow : UserControl
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (CompareRow), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty FirstValueProperty =
            DependencyProperty.Register("FirstValue", typeof(string), typeof(CompareRow), new PropertyMetadata(default(string)));

        public string FirstValue
        {
            get { return (string)GetValue(FirstValueProperty); }
            set { SetValue(FirstValueProperty, value); }
        }

        public static readonly DependencyProperty SecondValueProperty =
            DependencyProperty.Register("SecondValue", typeof(string), typeof(CompareRow), new PropertyMetadata(default(string)));

        public string SecondValue
        {
            get { return (string)GetValue(SecondValueProperty); }
            set { SetValue(SecondValueProperty, value); }
        }

        public static readonly DependencyProperty DiffProperty =
            DependencyProperty.Register("Diff", typeof (int), typeof (CompareRow), new PropertyMetadata(default(int)));

        public int Diff
        {
            get { return (int) GetValue(DiffProperty); }
            set { SetValue(DiffProperty, value); }
        }

        public CompareRow()
        {
            InitializeComponent();
        }
    }
}
