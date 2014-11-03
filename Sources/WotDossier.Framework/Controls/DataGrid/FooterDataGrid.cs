using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WotDossier.Common.Collections;

namespace WotDossier.Framework.Controls.DataGrid
{
    public class FooterDataGrid : System.Windows.Controls.DataGrid, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        public System.Windows.Controls.DataGrid RowSummariesGrid { get; set; }
        
        public bool IsVerticalScrolling { get; set; }

        public static readonly DependencyProperty ShowRowSummariesProperty = DependencyProperty.Register(
            "ShowRowSummaries", typeof(Boolean), typeof(FooterDataGrid), new PropertyMetadata(false));

        /// <summary>
        /// Show Row Summaries
        /// </summary>
        public Boolean ShowRowSummaries
        {
            get { return (Boolean)GetValue(ShowRowSummariesProperty); }
            set { SetValue(ShowRowSummariesProperty, value); }
        }

        public static readonly DependencyProperty FooterItemsSourceProperty = DependencyProperty.Register(
            "FooterItemsSource", typeof (IEnumerable), typeof (FooterDataGrid), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable FooterItemsSource
        {
            get { return (IEnumerable) GetValue(FooterItemsSourceProperty); }
            set { SetValue(FooterItemsSourceProperty, value); }
        }

        #region Constructors

        static FooterDataGrid()
        {
            Type ownerType = typeof(FooterDataGrid);
            DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(ownerType));
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="E:Sorting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DataGridSortingEventArgs"/> instance containing the event data.</param>
        protected override void OnSorting(DataGridSortingEventArgs e)
        {
            var lastRowList = ItemsSource as IFooterList;
            if (lastRowList == null)
            {
                base.OnSorting(e);
                return;
            }

            var column = e.Column;
            e.Handled = true;

            var direction = (column.SortDirection != ListSortDirection.Descending)
                                ? ListSortDirection.Descending
                                : ListSortDirection.Ascending;
            column.SortDirection = direction;
            lastRowList.SortButFirstRows(1, e.Column.SortMemberPath, direction, (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift);

            Items.Refresh();
        }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public DataGridColumn GetRowSummariesGridColumnForSortMemberPath(string sortMemberPath)
        {
            if (RowSummariesGrid != null)
            {
                return RowSummariesGrid.Columns.FirstOrDefault(column => column.SortMemberPath == sortMemberPath);
            }
            return null;
        }
    }
}
