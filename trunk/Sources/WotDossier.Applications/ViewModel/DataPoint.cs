namespace WotDossier.Applications.ViewModel
{
    public class DataPoint : IDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class GenericPoint<T1, T2>
    {
        public T1 X { get; set; }
        public T2 Y { get; set; }

        public GenericPoint(T1 x, T2 y)
        {
            X = x;
            Y = y;
        }
    }
}