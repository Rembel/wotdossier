using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WotDossier.Framework.EventAggregator
{
    /// <summary>
    /// An Event Aggregator acts as a single source of events for many objects. 
    /// It registers for all the events of the many objects allowing clients to register with just the aggregator
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly List<BaseEvent> _events = new List<BaseEvent>();
        private readonly ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <typeparam name="TEventType">The type of the event.</typeparam>
        /// <returns></returns>
        public TEventType GetEvent<TEventType>() where TEventType : BaseEvent
        {
            _rwl.EnterUpgradeableReadLock();

            try
            {
                TEventType eventInstance = _events.SingleOrDefault(evt => evt.GetType() == typeof(TEventType)) as TEventType;

                if (eventInstance == null)
                {
                    _rwl.EnterWriteLock();

                    try
                    {
                        eventInstance = _events.SingleOrDefault(evt => evt.GetType() == typeof(TEventType)) as TEventType;

                        if (eventInstance == null)
                        {
                            eventInstance = Activator.CreateInstance<TEventType>();
                            _events.Add(eventInstance);
                        }
                    }
                    finally
                    {
                        _rwl.ExitWriteLock();
                    }
                }

                return eventInstance;
            }
            finally
            {
                _rwl.ExitUpgradeableReadLock();
            }
        }
    }
}