using System.Collections.Generic;

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

        protected bool Equals(DataPoint other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
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

        protected bool Equals(GenericPoint<T1, T2> other)
        {
            return EqualityComparer<T2>.Default.Equals(Y, other.Y) && EqualityComparer<T1>.Default.Equals(X, other.X);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericPoint<T1, T2>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T2>.Default.GetHashCode(Y)*397) ^ EqualityComparer<T1>.Default.GetHashCode(X);
            }
        }
    }
}