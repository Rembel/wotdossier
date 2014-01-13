using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common.Reflection;

namespace WotDossier.Common.Collections
{
    public class FooterList<T> : List<T>, IFooterList
    {
        private SortDescriptionCollection _sortDescriptions;

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.SortDescription"/> objects that describe how the items in the collection are sorted in the view.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of <see cref="T:System.ComponentModel.SortDescription"/> objects that describe how the items in the collection are sorted in the view.
        /// </returns>
        public SortDescriptionCollection SortDescriptions
        {
            get { return _sortDescriptions ?? (_sortDescriptions = new SortDescriptionCollection()); }
        }

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

        public void SortButFirstRows(int count, string propertyName, ListSortDirection direction, bool clearExistingSortDescriptions)
        {
            if (clearExistingSortDescriptions)
            {
                SortDescriptions.Clear();
            }
            SortDescriptions.Add(new SortDescription(propertyName, direction));
            IComparer<T> comparerLast = new MultiPropertyComparer<T>(SortDescriptions);
            int totalCount = Count;
            int countToSort = totalCount > count ? totalCount - count : 0;
            Sort(count, countToSort, comparerLast);
        }
    }
}