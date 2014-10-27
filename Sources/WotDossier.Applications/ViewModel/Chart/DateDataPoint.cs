using System;

namespace WotDossier.Applications.ViewModel.Chart
{
    public class DateDataPoint : DataPoint
    {
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateDataPoint" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="date">The date.</param>
        public DateDataPoint(double x, double y, DateTime date) : base(x, y)
        {
            Date = date;
        }
    }
}