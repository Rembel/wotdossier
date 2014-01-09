using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace WotDossier.Common.Reflection
{
    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly IComparer comparer;

        private PropertyAccessor accessor;
        private int reverse;

        public PropertyComparer(string propertyName, ListSortDirection direction)
        {
            accessor = new PropertyAccessor(typeof(T), propertyName);
            var comparerForPropertyType =
                typeof(Comparer<>).MakeGenericType(typeof(T).GetProperty(propertyName).PropertyType);
            comparer =
                (IComparer)
                comparerForPropertyType.InvokeMember("Default",
                                                     BindingFlags.Static | BindingFlags.GetProperty |
                                                     BindingFlags.Public, null, null, null);
            SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return reverse * comparer.Compare(accessor.Get(x), accessor.Get(y));
        }

        #endregion

        private void SetListSortDirection(ListSortDirection direction)
        {
            reverse = direction == ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(string name, ListSortDirection direction)
        {
            SetListSortDirection(direction);
            accessor = new PropertyAccessor(typeof(T), name);
        }
    }
}