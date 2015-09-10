namespace WotDossier.Applications.ViewModel.Chart
{
    public class LocalizedGenericPoint<T1, T2> : GenericPoint<T1, T2>
    {
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPoint{T1,T2}" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="title">The title.</param>
        public LocalizedGenericPoint(T1 x, T2 y, string title) : base(x, y)
        {
            Title = title;
        }
    }
}