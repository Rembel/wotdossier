using System;

namespace WotDossier.Framework.EventAggregator
{
    public class WeakEventHandler
    {
        public static void SetAnyGenericHandler<S, T>(
            Action<EventHandler<T>> add, //to add event listener to publisher
            Action<EventHandler<T>> remove, //to remove event listener from publisher
            S subscriber, //ref to subscriber (to pass to consume)
            Action<S, T> consume) //called when event is raised*
            where T : EventArgs
            where S : class
        {
            var subscriberWeakRef = new WeakReference(subscriber);
            EventHandler<T> handler = null;
            handler = delegate(object sender, T e)
            {
                var subscriberStrongRef = subscriberWeakRef.Target as S;
                if (subscriberStrongRef != null)
                {
                    Console.WriteLine("New event received by subscriber");
                    consume(subscriberStrongRef, e);
                }
                else
                {
                    remove(handler);
                    handler = null;
                }
            };
            add(handler);
        }
    }
}
