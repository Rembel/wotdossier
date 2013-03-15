using System;
using System.Threading;
using System.Windows;

namespace WotDossier.Framework.Forms
{
    /// <summary>
    /// Interaction logic for WPFMessageBox.xaml
    /// </summary>
    public partial class WpfMessageBox
    {
        private WpfMessageBoxResult _result = WpfMessageBoxResult.Undefined;
        public WpfMessageBoxResult Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public WpfMessageBox()
        {
            InitializeComponent();
        }

        public static WpfMessageBoxResult Show(string message)
        {
            return Show(message, string.Empty, string.Empty, WpfMessageBoxButton.OK, WPFMessageBoxImage.Default);
        }

        public static WpfMessageBoxResult Show(string message, string title)
        {
            return Show(message, title, string.Empty, WpfMessageBoxButton.OK, WPFMessageBoxImage.Default);
        }

        public static WpfMessageBoxResult Show(string message, string title, string details)
        {
            return Show(message, title, details, WpfMessageBoxButton.OK, WPFMessageBoxImage.Default);
        }

        public static WpfMessageBoxResult Show(string message, string title, WpfMessageBoxButton buttonOption)
        {
            return Show(message, title, string.Empty, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WpfMessageBoxResult Show(string message, string title, string details, WpfMessageBoxButton buttonOption)
        {
            return Show(message, title, details, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WpfMessageBoxResult Show(string message, string title, WPFMessageBoxImage image)
        {
            return Show(message, title, string.Empty, WpfMessageBoxButton.OK, image);
        }

        public static WpfMessageBoxResult Show(string message, string title, string details, WPFMessageBoxImage image)
        {
            return Show(message, title, details, WpfMessageBoxButton.OK, image);
        }

        public static WpfMessageBoxResult Show(string message, string title, WpfMessageBoxButton buttonOption, WPFMessageBoxImage image)
        {
            return Show(message, title, string.Empty, buttonOption, image);
        }

        public static WpfMessageBoxResult Show(string message, string title, string details, WpfMessageBoxButton buttonOption, WPFMessageBoxImage image)
        {
            return Show(message, title, details, string.Empty, buttonOption, image);
        }

        public static WpfMessageBoxResult Show(string message, string title, string details, string customCommandText, WpfMessageBoxButton buttonOption, WPFMessageBoxImage image)
        {
            var window = GetTopWindow();
            WpfMessageBox messageBox = new WpfMessageBox();
            MessageBoxViewModel viewModel = new MessageBoxViewModel(messageBox, title, message, details, customCommandText, buttonOption, image);
            messageBox.DataContext = viewModel;
            messageBox.Owner = window;
            messageBox.ShowDialog();
            return messageBox.Result;
        }

        public void NewWindowThread<T, P>(Func<P, T> constructor, P param) where T : WpfMessageBox
        {

            Thread thread = new Thread(() =>
            {

                T w = constructor(param);

                w.Show();

                w.Closed += (sender, e) => w.Dispatcher.InvokeShutdown();

                System.Windows.Threading.Dispatcher.Run();

            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();

        }


        public static Window GetTopWindow()
        {
            if (Application.Current != null && Application.Current.Windows.Count > 0)
            {
                return Application.Current.Windows[0];
            }
            return null;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            NativeMethods.RemoveIcon(this);
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(textBlock.Text);
        }
    }
}
