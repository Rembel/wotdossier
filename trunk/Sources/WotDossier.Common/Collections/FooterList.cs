using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common.Reflection;

namespace WotDossier.Common.Collections
{
    public class FooterList<T> : List<T>, IFooterList
    {
        public FooterList()
        {
        }

        public FooterList(int capacity)
            : base(capacity)
        {
        }

        public FooterList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public void SortButFirstRows(int count, string propertyName, ListSortDirection direction)
        {
            var comparerLast = new PropertyComparer<T>(propertyName, direction);
            int totalCount = Count;
            int countToSort = totalCount > count ? totalCount - count : 0;
            Sort(count, countToSort, comparerLast);
        }
    }
}