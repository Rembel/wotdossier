using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WotDossier.Common.Reflection
{
    public class MultiPropertyComparer<T> : IComparer<T>
    {
        private readonly List<PropertyComparer<T>> _comparers = new List<PropertyComparer<T>>();

        public MultiPropertyComparer(IEnumerable<SortDescription> sortDescriptions)
        {
            foreach (SortDescription description in sortDescriptions)
            {
                _comparers.Add(new PropertyComparer<T>(description.PropertyName, description.Direction));
            }
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            int result = 0;

            for (int index = _comparers.Count - 1, weight = 0; index >= 0; index--, weight++)
            {
                PropertyComparer<T> propertyComparer = _comparers[index];
                result += propertyComparer.Compare(x, y) * (int)Math.Pow(10, weight);
            }

            return result;
        }

        #endregion
    }
}