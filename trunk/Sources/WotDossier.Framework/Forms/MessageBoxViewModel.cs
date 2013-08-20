using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Framework.Forms
{
    public class MessageBoxViewModel : INotifyPropertyChanged
    {
        #region Fields

        string _title;
        string _message;
        string _innerMessageDetails;

        Visibility _yesVisibility = Visibility.Collapsed;
        Visibility _noVisibility = Visibility.Collapsed;
        Visibility _cancelVisibility = Visibility.Collapsed;
        Visibility _okVisibility = Visibility.Collapsed;
        Visibility _closeVisibility = Visibility.Collapsed;
        Visibility _customVisibility = Visibility.Collapsed;
        Visibility _showDetails = Visibility.Collapsed;

        ICommand _yesCommand;
        ICommand _noCommand;
        ICommand _cancelCommand;
        ICommand _closeCommand;
        ICommand _okCommand;
        ICommand _customCommand;

        readonly WpfMessageBox _view;
        private readonly string _customCommandText;
        private readonly WpfMessageBoxButton _buttonOption;
        ImageSource _messageImageSource;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChange("Title");
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChange("Message");
                }
            }
        }

        public string InnerMessageDetails
        {
            get { return _innerMessageDetails; }
            set
            {
                if (_innerMessageDetails != value)
                {
                    _innerMessageDetails = value;
                    NotifyPropertyChange("InnerMessageDetails");
                }
            }
        }

        public string CustomCommandText
        {
            get { return _customCommandText; }
        }

        public ImageSource MessageImageSource
        {
            get { return _messageImageSource; }
            set
            {
                _messageImageSource = value;
                NotifyPropertyChange("MessageImageSource");
            }
        }

        public Visibility YesVisibility
        {
            get { return _yesVisibility; }
            set
            {
                if (_yesVisibility != value)
                {
                    _yesVisibility = value;
                    NotifyPropertyChange("YesVisibility");
                }
            }
        }

        public Visibility NoVisibility
        {
            get { return _noVisibility; }
            set
            {
                if (_noVisibility != value)
                {
                    _noVisibility = value;
                    NotifyPropertyChange("NoVisibility");
                }
            }
        }

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                if (_cancelVisibility != value)
                {
                    _cancelVisibility = value;
                    NotifyPropertyChange("CancelVisibility");
                }
            }
        }

        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set
            {
                if (_okVisibility != value)
                {
                    _okVisibility = value;
                    NotifyPropertyChange("OkVisibility");
                }
            }
        }

        public Visibility CloseVisibility
        {
            get { return _closeVisibility; }
            set
            {
                if (_closeVisibility != value)
                {
                    _closeVisibility = value;
                    NotifyPropertyChange("CloseVisibility");
                }
            }
        }

        public Visibility CustomVisibility
        {
            get { return _customVisibility; }
            set
            {
                if (_customVisibility != value)
                {
                    _customVisibility = value;
                    NotifyPropertyChange("CustomVisibility");
                }
            }
        }

        public Visibility ShowDetails
        {
            get { return _showDetails; }
            set
            {
                if (_showDetails != value)
                {
                    _showDetails = value;
                    NotifyPropertyChange("ShowDetails");
                }
            }
        }

        public ICommand YesCommand
        {
            get { return _yesCommand ?? (_yesCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.Yes))); }
        }

        public ICommand NoCommand
        {
            get { return _noCommand ?? (_noCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.No))); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.Cancel))); }
        }


        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.Close))); }
        }

        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.Ok))); }
        }

        public ICommand CustomCommand
        {
            get { return _customCommand ?? (_customCommand = new DelegateCommand(() => ExecuteCommand(WpfMessageBoxResult.Custom))); }
        }
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerMessage">The inner message.</param>
        /// <param name="customCommandText">The custom command text.</param>
        /// <param name="buttonOption">The button option.</param>
        /// <param name="image">The image.</param>
        public MessageBoxViewModel(WpfMessageBox view, string title, string message, string innerMessage, string customCommandText,
            WpfMessageBoxButton buttonOption, WPFMessageBoxImage image)
        {
            Title = title;
            Message = message;
            InnerMessageDetails = innerMessage;
            SetButtonVisibility(buttonOption);
            SetImageSource(image);
            _view = view;
            _customCommandText = customCommandText;
            _buttonOption = buttonOption;
            _view.Closing += _view_Closing;
        }

        private void ExecuteCommand(WpfMessageBoxResult commandResult)
        {
            _view.Result = commandResult;
            _view.Close();
        }

        private void _view_Closing(object sender, CancelEventArgs e)
        {
            if (_view.Result == WpfMessageBoxResult.Undefined )
            {
                if (CancelVisibility == Visibility.Visible)
                {
                    _view.Result = WpfMessageBoxResult.Cancel;
                }
                else if (NoVisibility == Visibility.Visible)
                {
                    _view.Result = WpfMessageBoxResult.No;
                }
                else if (CloseVisibility == Visibility.Visible)
                {
                    _view.Result = WpfMessageBoxResult.Close;
                }
                else if (OkVisibility == Visibility.Visible)
                {
                    _view.Result = WpfMessageBoxResult.Ok;
                }
            }
        }

        private void NotifyPropertyChange(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void SetButtonVisibility(WpfMessageBoxButton buttonOption)
        {
            if ((buttonOption & WpfMessageBoxButton.Yes) == WpfMessageBoxButton.Yes)
            {
                YesVisibility = Visibility.Visible;
            }
            if ((buttonOption & WpfMessageBoxButton.No) == WpfMessageBoxButton.No)
            {
                NoVisibility = Visibility.Visible;
            }
            if ((buttonOption & WpfMessageBoxButton.OK) == WpfMessageBoxButton.OK)
            {
                OkVisibility = Visibility.Visible;
            }
            if ((buttonOption & WpfMessageBoxButton.Cancel) == WpfMessageBoxButton.Cancel)
            {
                CancelVisibility = Visibility.Visible;
            }
            if ((buttonOption & WpfMessageBoxButton.Close) == WpfMessageBoxButton.Close)
            {
                CloseVisibility = Visibility.Visible;
            }
            if ((buttonOption & WpfMessageBoxButton.Custom) == WpfMessageBoxButton.Custom)
            {
                CustomVisibility = Visibility.Visible;
            }
            ShowDetails = string.IsNullOrEmpty(InnerMessageDetails) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SetImageSource(WPFMessageBoxImage image)
        {
            string source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Default.png";
            switch (image)
            {
                case WPFMessageBoxImage.Warning:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Alert.png";
                    break;
                case WPFMessageBoxImage.Error:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Error.png";
                    break;
                case WPFMessageBoxImage.Information:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Info.png";
                    break;
                case WPFMessageBoxImage.OK:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/OK.png";
                    break;
                case WPFMessageBoxImage.Question:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Help.png";
                    break;
                default:
                    source = "pack://application:,,,/WotDossier.Framework;component/Forms/Images/Default.png";
                    break;

            }
            Uri imageUri = new Uri(source, UriKind.RelativeOrAbsolute);
            MessageImageSource = new BitmapImage(imageUri);
        }
    }
}
