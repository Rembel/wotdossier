using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Framework.Controls.DataGrid;

namespace WotDossier.Styles
{
    public class FooterDataGridGeneric
    {
        public static void RowSummariesGridLoaded(object sender, RoutedEventArgs e)
        {
            var datagrid = (DataGrid)sender;
            var parentGrid = Helper.FindVisualParent<FooterDataGrid>(datagrid);


            if (parentGrid != null)
            {
                if (parentGrid.RowSummariesGrid != null)
                {
                    return;
                }
                
                parentGrid.RowSummariesGrid = datagrid;
                
                var sortedColumn = parentGrid.Columns.ToList();
                datagrid.Columns.Clear();
                foreach (var column in sortedColumn.OrderBy(c => c.DisplayIndex))
                {
                    BindingBase textBinding = null;
                    Style textElementStyle = null;

                    DataGridTextColumn textColumn = column as DataGridTextColumn;
                    if (textColumn != null)
                    {
                        textBinding = textColumn.Binding;
                        textElementStyle = textColumn.ElementStyle;
                    }

                    //textBinding = new Binding(column.SortMemberPath);

                    datagrid.Columns.Add(new DataGridTextColumn
                    {
                        SortMemberPath = column.SortMemberPath,
                        DisplayIndex = column.DisplayIndex,
                        Visibility = column.Visibility,
                        Width = column.Width,
                        Binding = textBinding,
                        CellStyle = column.CellStyle,
                        ElementStyle = textElementStyle,
                    });
                }

                var summaryRow = (Grid)(datagrid.Parent);
                if (summaryRow != null)
                {
                    summaryRow.Visibility = parentGrid.ShowRowSummaries ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            //datagrid.AutoGenerateColumns = true;
        }

        public static void OnScroll(object sender, ScrollEventArgs e)
        {
            //Get the main grid from the source
            var mainGrid = Helper.FindVisualParent<FooterDataGrid>(sender as ScrollBar);
            if (mainGrid != null)
            {
                //Find the row summaries Grid
                var summariesGrid = mainGrid.RowSummariesGrid;
                if (summariesGrid != null)
                {
                    summariesGrid.UpdateLayout();
                    var scrollViewer = Helper.FindChild<ScrollBar>(summariesGrid, "PART_HorizontalScrollBar");

                    ;
                    if (scrollViewer != null)
                    {
                        var border = VisualTreeHelper.GetChild(summariesGrid, 0) as Decorator;
                        if (border != null)
                        {
                            var scroll = border.Child as ScrollViewer;
                            if (scroll != null)
                            {
                                scroll.CanContentScroll = false;
                                scroll.ScrollToHorizontalOffset(e.NewValue);
                            }

                        }
                    }
                }
            }
        }

        public static void ColumnSizeChanged(object sender, SizeChangedEventArgs e)
        {

            var grid = Helper.FindVisualParent<FooterDataGrid>((FrameworkElement)sender);
            if (grid == null) return;
            foreach (var column in grid.Columns)
            {
                //Adjust width of summary row headers
                var summarieRowGridColumn = grid.GetRowSummariesGridColumnForSortMemberPath(column.SortMemberPath);
                if (summarieRowGridColumn != null)
                {
                    summarieRowGridColumn.Visibility = column.Visibility;
                    summarieRowGridColumn.Width = column.ActualWidth;
                }
            }

            var columnPositions = new List<double>();
            double tillNowWidth = 0;
            foreach (var column in grid.Columns)
            {
                //Adjust width of summary row headers
                var summarieRowGridColumn = grid.GetRowSummariesGridColumnForSortMemberPath(column.SortMemberPath);
                if (summarieRowGridColumn != null)
                {
                    summarieRowGridColumn.Visibility = column.Visibility;
                    summarieRowGridColumn.Width = column.ActualWidth;
                }

                if (column.Visibility == Visibility.Visible)
                {
                    tillNowWidth = tillNowWidth + column.ActualWidth - .5;
                    columnPositions.Add(tillNowWidth);
                }
            }
        }
    }
}
