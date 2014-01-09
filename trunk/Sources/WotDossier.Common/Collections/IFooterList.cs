using System.Collections;
using System.ComponentModel;

namespace WotDossier.Common.Collections
{
    public interface IFooterList : IEnumerable
    {
        void SortButFirstRows(int count, string propertyName, ListSortDirection direction);
    }
}