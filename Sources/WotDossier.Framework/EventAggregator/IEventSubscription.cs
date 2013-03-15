using System;

namespace WotDossier.Framework.EventAggregator
{
    public interface IEventSubscription
    {
        SubscriptionToken SubscriptionToken
        {
            get;
            set;
        }

        Action<object[]> GetExecutionStrategy();
    }
}