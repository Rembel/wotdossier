using System;
using System.Windows.Input;

namespace WotDossier.Framework
{
    internal static class CommandLibraryHelper
    {
        // Methods
        internal static RoutedCommand CreateUICommand(string name, Type ownerType, byte commandId)
        {
            RoutedCommand command;
            command = new RoutedCommand(name, ownerType);
            return command;
        }
    }

    public class ApplicationRouteCommands
    {
        // Fields
        private static RoutedCommand[] _internalCommands = new RoutedCommand[1];

        // Methods
        private static RoutedCommand _EnsureCommand(CommandId idCommand)
        {
            lock (_internalCommands.SyncRoot)
            {
                if (_internalCommands[(int)idCommand] == null)
                {
                    RoutedCommand command = CommandLibraryHelper.CreateUICommand(GetPropertyName(idCommand), typeof(ApplicationCommands), (byte)idCommand);
                    _internalCommands[(int)idCommand] = command;
                }
            }
            return _internalCommands[(int)idCommand];
        }

        public static RoutedCommand ApplyTemplate
        {
            get
            {
                return _EnsureCommand(CommandId.ApplyTemplate);
            }
        }

        private static string GetPropertyName(CommandId commandId)
        {
            string str = string.Empty;
            switch (commandId)
            {
                case CommandId.ApplyTemplate:
                    return "ApplyTemplate";
            }
            return str;
        }

        // Nested Types
        private enum CommandId : byte
        {
            ApplyTemplate = 0
        }

    }
}
