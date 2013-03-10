namespace WotDossier.Framework.EventAggregator
{
    public static class EventAggregatorFactory
    {
        // Singleton instance of the EventAggregator service.
        private static readonly EventAggregator _eventAggregator = new EventAggregator();

        public static IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }
    }
}
