using System;
using System.Windows;
using System.Windows.Input;

namespace WotDossier.Framework.Forms.Commands
{
    /// <summary>
    /// This class facilitates associating a key binding in XAML markup to a command
    /// defined in a View Model by exposing a Command dependency property.
    /// The class derives from Freezable to work around a limitation in WPF when data-binding from XAML.
    /// </summary>
    public class CommandReference : Freezable, ICommand
    {
        /// <summary>
        /// The command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
            (
                "Command",
                typeof (ICommand),
                typeof (CommandReference),
                new PropertyMetadata((x, y) =>
                {
                    var commandReference = x as CommandReference;
                    var oldCommand = y.OldValue as ICommand;
                    var newCommand = y.NewValue as ICommand;

                    if (oldCommand != null) oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
                    if (newCommand != null) newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
                })
            );

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (Command != null) return Command.CanExecute(parameter);
            return false;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            Command.Execute(parameter);
        }

        /// <summary>
        /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable" /> derived class.
        /// </summary>
        /// <returns>
        /// The new instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
    }
}