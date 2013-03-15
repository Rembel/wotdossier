using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WotDossier.Framework.Forms.Commands
{
    /// <summary>
    ///     This class contains methods for the CommandManager that help avoid memory leaks by
    ///     using weak references.
    /// </summary>
    internal class CommandManagerHelper
    {
        internal static Action<List<WeakReference>> CallWeakReferenceHandlers = x =>
        {
            if (x != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.

                var callers = new EventHandler[x.Count];
                int count = 0;

                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var reference = x[i];
                    var handler = reference.Target as EventHandler;
                    if (handler == null)
                    {
                        // Clean up old handlers that have been collected
                        x.RemoveAt(i);
                    }
                    else
                    {
                        callers[count] = handler;
                        count++;
                    }
                }

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++)
                {
                    var handler = callers[i];
                    handler(null, EventArgs.Empty);
                }
            }
        };

        internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var handler = y.Target as EventHandler;
                    if (handler != null)
                    {
                        CommandManager.RequerySuggested += handler;
                    }
                });
            }
        };

        internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var handler = y.Target as EventHandler;
                    if (handler != null)
                    {
                        CommandManager.RequerySuggested -= handler;
                    }
                });
            }
        };

        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        internal static Action<List<WeakReference>, EventHandler> RemoveWeakReferenceHandler = (x, y) =>
        {
            if (x != null)
            {
                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var reference = x[i];
                    var existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == y))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        x.RemoveAt(i);
                    }
                }
            }
        };
    }
}