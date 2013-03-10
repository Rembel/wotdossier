using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WotDossier.Framework.Applications
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <i:Interaction.Triggers>
    ///     <i:EventTrigger EventName="SelectionChanged">
    ///         <Applications:EventToCommand Command="{Binding Path=SelectCommand, Mode=OneWay}"/>
    ///     </i:EventTrigger>
    /// </i:Interaction.Triggers>
    /// </example>
    public class EventToCommand : TriggerAction<FrameworkElement>
    {
        // Fields
        private object _commandParameterValue;
        private bool? _mustToggleValue;
        private bool _isSendEventArgsToCommand;

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null, OnCommandParameterChanged));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, OnCommandChanged));
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventToCommand), new PropertyMetadata(false, OnMustToggleIsEnabledChanged));

        // Properties
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public object CommandParameterValue
        {
            get { return (_commandParameterValue ?? CommandParameter); }
            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        public bool IsEnabledCanExecute
        {
            get { return (bool) GetValue(MustToggleIsEnabledProperty); }
            set { SetValue(MustToggleIsEnabledProperty, value); }
        }

        public bool IsEnabledCanExecuteValue
        {
            get
            {
                if (_mustToggleValue.HasValue)
                {
                    return _mustToggleValue.Value;
                }
                return IsEnabledCanExecute;
            }
            set
            {
                _mustToggleValue = new bool?(value);
                EnableDisableElement();
            }
        }

        public bool IsSendEventArgsToCommand
        {
            get { return _isSendEventArgsToCommand; }
            set { _isSendEventArgsToCommand = value; }
        }

        // Methods
        private bool AssociatedElementIsDisabled()
        {
            Control associatedObject = GetAssociatedObject();
            return ((associatedObject != null) && !associatedObject.IsEnabled);
        }

        private void EnableDisableElement()
        {
            Control associatedObject = GetAssociatedObject();
            if (associatedObject != null)
            {
                ICommand command = GetCommand();
                if (IsEnabledCanExecuteValue && (command != null))
                {
                    associatedObject.IsEnabled = command.CanExecute(CommandParameterValue);
                }
            }
        }

        private Control GetAssociatedObject()
        {
            return (AssociatedObject as Control);
        }

        private ICommand GetCommand()
        {
            return Command;
        }

        public void Invoke()
        {
            Invoke(null);
        }

        protected override void Invoke(object parameter)
        {
            if (!AssociatedElementIsDisabled())
            {
                ICommand command = GetCommand();
                object commandParameterValue = CommandParameterValue;
                if ((commandParameterValue == null) && IsSendEventArgsToCommand)
                {
                    commandParameterValue = parameter;
                }
                if ((command != null) && command.CanExecute(commandParameterValue))
                {
                    command.Execute(commandParameterValue);
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventToCommand command = d as EventToCommand;
            if (command != null)
            {
                if (e.OldValue != null)
                {
                    ((ICommand)e.OldValue).CanExecuteChanged -= command.OnCommandCanExecuteChanged;
                }
                ICommand command2 = (ICommand)e.NewValue;
                if (command2 != null)
                {
                    command2.CanExecuteChanged += command.OnCommandCanExecuteChanged;
                }
                command.EnableDisableElement();
            }
        }

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventToCommand command = d as EventToCommand;
            if ((command != null) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }

        private static void OnMustToggleIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventToCommand command = d as EventToCommand;
            if ((command != null) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }
    }
}
