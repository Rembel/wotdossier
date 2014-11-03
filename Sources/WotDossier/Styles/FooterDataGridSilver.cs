using System.Windows;
using System.Windows.Controls.Primitives;

namespace WotDossier.Styles
{
    public partial class FooterDataGridSilver
    {
        private void RowSummariesGridLoaded(object sender, RoutedEventArgs e)
        {
            FooterDataGridGeneric.RowSummariesGridLoaded(sender, e);
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            FooterDataGridGeneric.OnScroll(sender, e);
        }

        private void ColumnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FooterDataGridGeneric.ColumnSizeChanged(sender, e);
        }
    }
}
