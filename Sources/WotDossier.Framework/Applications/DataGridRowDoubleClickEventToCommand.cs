using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using WotDossier.Framework.Forms.Commands;

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
    public class DataGridRowDoubleClickEventToCommand : TriggerAction<FrameworkElement>
    {
        // Fields
        private object _commandParameterValue;
        private bool? _mustToggleValue;
        private bool _isSendEventArgsToCommand;

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DataGridRowDoubleClickEventToCommand), new PropertyMetadata(null, OnCommandParameterChanged));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(DelegateCommand<object>), typeof(DataGridRowDoubleClickEventToCommand), new PropertyMetadata(null, OnCommandChanged));
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(DataGridRowDoubleClickEventToCommand), new PropertyMetadata(false, OnMustToggleIsEnabledChanged));

        // Properties
        public DelegateCommand<object> Command
        {
            get { return (DelegateCommand<object>)GetValue(CommandProperty); }
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

        private DelegateCommand<object> GetCommand()
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
                DelegateCommand<object> command = GetCommand();

                Selector control = GetAssociatedObject() as Selector;
                
                if ((command != null) && control != null && command.CanExecute(control.SelectedItem))
                {
                    command.Execute(control.SelectedItem);
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
            DataGridRowDoubleClickEventToCommand command = d as DataGridRowDoubleClickEventToCommand;
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
            DataGridRowDoubleClickEventToCommand command = d as DataGridRowDoubleClickEventToCommand;
            if ((command != null) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }

        private static void OnMustToggleIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridRowDoubleClickEventToCommand command = d as DataGridRowDoubleClickEventToCommand;
            if ((command != null) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }
    }
}
