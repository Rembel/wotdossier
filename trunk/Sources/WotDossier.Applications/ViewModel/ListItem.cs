namespace WotDossier.Applications.ViewModel
{
    public class ListItem<TId>
    {
        public TId Id { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ListItem(TId id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}