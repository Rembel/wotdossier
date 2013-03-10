using System.Collections;
using System.Windows.Controls;

namespace WotDossier.Framework.Controls
{
    public class MultiSelectListBox : ListBox
    {
        public new bool SetSelectedItems(IEnumerable selectedItems)
        {
            return base.SetSelectedItems(selectedItems);
        }
    }
}
