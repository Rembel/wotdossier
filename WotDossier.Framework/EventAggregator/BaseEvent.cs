using System;
using System.Collections.Generic;
using System.Linq;

namespace WotDossier.Framework.EventAggregator
{
    public abstract class BaseEvent
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        protected ICollection<IEventSubscription> Subscriptions
        {
            get
            {
                return _subscriptions.AsReadOnly();
            }
        }

        protected virtual SubscriptionToken Subscribe(IEventSubscription eventSubscription)
        {
            eventSubscription.SubscriptionToken = new SubscriptionToken();

            lock (_subscriptions)
            {
                _subscriptions.Add(eventSubscription);
            }

            return eventSubscription.SubscriptionToken;
        }

        protected virtual void Publish(params object[] arguments)
        {
            List<Action<object[]>> executionStrategies = PruneAndReturnStrategies();

            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(arguments);
            }
        }

        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock (_subscriptions)
            {
                IEventSubscription subscription = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);

                if (subscription != null)
                {
                    _subscriptions.Remove(subscription);
                }
            }
        }

        public virtual bool Contains(SubscriptionToken token)
        {
            lock (_subscriptions)
            {
                IEventSubscription subscription = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);

                return (subscription != null);
            }
        }

        private List<Action<object[]>> PruneAndReturnStrategies()
        {
            List<Action<object[]>> returnList = new List<Action<object[]>>();

            lock (_subscriptions)
            {
                for (int i = _subscriptions.Count - 1; i >= 0; i--)
                {
                    Action<object[]> listItem = _subscriptions[i].GetExecutionStrategy();

                    if (listItem == null)
                    {
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }
    }
}