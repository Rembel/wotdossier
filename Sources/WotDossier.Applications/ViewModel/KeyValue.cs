namespace WotDossier.Applications.ViewModel
{
    public class KeyValue<TKey,TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}