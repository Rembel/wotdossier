namespace WotDossier.Applications.ViewModel.Chart
{
    /// <summary>
    /// 
    /// </summary>
    public class DataPoint : GenericPoint<double, double>, IDataPoint
    {
        public DataPoint(double x, double y) : base(x, y)
        {
        }
    }
}