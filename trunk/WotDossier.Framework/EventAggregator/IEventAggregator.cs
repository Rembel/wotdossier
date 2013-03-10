namespace WotDossier.Framework.EventAggregator
{
    public interface IEventAggregator
    {
        TEventType GetEvent<TEventType>() where TEventType : BaseEvent;
    }
}