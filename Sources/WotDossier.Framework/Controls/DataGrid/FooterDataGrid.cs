using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WotDossier.Common.Collections;

namespace WotDossier.Framework.Controls.DataGrid
{
    public class FooterDataGrid : System.Windows.Controls.DataGrid
    {
        //public static readonly DependencyProperty FooterRowsCountProperty =
        //    DependencyProperty.Register("FooterRowsCount", typeof(int), typeof(FooterDataGrid),
        //                                new PropertyMetadata(default(int)));

        //public int FooterRowsCount
        //{
        //    get { return (int)GetValue(FooterRowsCountProperty); }
        //    set { SetValue(FooterRowsCountProperty, value); }
        //}

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

            var direction = (column.SortDirection != ListSortDirection.Ascending)
                                ? ListSortDirection.Ascending
                                : ListSortDirection.Descending;
            column.SortDirection = direction;
            //TODO: multisorting
            lastRowList.SortButFirstRows(1, e.Column.SortMemberPath, direction);

            Items.Refresh();
        }

        #region Constructors

        static FooterDataGrid()
        {
            Type ownerType = typeof(FooterDataGrid);
            DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(ownerType));
            ItemsPanelProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(FooterDataGridRowsPresenter)))));
        }

        public FooterDataGrid()
        {
            this.Loaded += new RoutedEventHandler(FooterDataGrid_Loaded);
        }

        void FooterDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FooterDataGridRowsPresenter panel = (FooterDataGridRowsPresenter)Helper.GetVisualChild<FooterDataGridRowsPresenter>(this);
            if (panel != null)
            {
                panel.InvalidateArrange();
            }
        }        

        #endregion

        #region Frozen Rows

        /// <summary>
        /// Dependency Property fro FrozenRowCount Property
        /// </summary>
        public static readonly DependencyProperty FrozenRowCountProperty =
            DependencyProperty.Register("FrozenRowCount",
                                        typeof(int),
                                        typeof(System.Windows.Controls.DataGrid),
                                        new FrameworkPropertyMetadata(1,
                                                                      new PropertyChangedCallback(OnFrozenRowCountPropertyChanged),
                                                                      new CoerceValueCallback(OnCoerceFrozenRowCount)),
                                        new ValidateValueCallback(ValidateFrozenRowCount));

        /// <summary>
        /// Property which determines the number of rows which are frozen from 
        /// the beginning in order of display
        /// </summary>
        public int FrozenRowCount
        {
            get { return (int)GetValue(FrozenRowCountProperty); }
            set { SetValue(FrozenRowCountProperty, value); }
        }

        /// <summary>
        /// Coercion call back for FrozenRowCount property, which ensures that 
        /// it is never more that Item count
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceFrozenRowCount(DependencyObject d, object baseValue)
        {
            System.Windows.Controls.DataGrid dataGrid = (System.Windows.Controls.DataGrid)d;
            int frozenRowCount = (int)baseValue;

            if (frozenRowCount > dataGrid.Items.Count)
            {
                return dataGrid.Items.Count;
            }

            return baseValue;
        }

        /// <summary>
        /// Property changed callback fro FrozenRowCount
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFrozenRowCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FooterDataGridRowsPresenter panel = (FooterDataGridRowsPresenter)Helper.GetVisualChild<FooterDataGridRowsPresenter>(d as Visual);
            if (panel != null)
            {
                panel.InvalidateArrange();
                (d as System.Windows.Controls.DataGrid).UpdateLayout();
                panel.InvalidateArrange();
            }
        }

        /// <summary>
        /// Validation call back for frozen row count
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool ValidateFrozenRowCount(object value)
        {
            int frozenCount = (int)value;
            return (frozenCount >= 0);
        }

        /// <summary>
        /// Dependency Property key for NonFrozenColumnsViewportHorizontalOffset Property
        /// </summary>
        private static readonly DependencyPropertyKey NonFrozenRowsViewportVerticalOffsetPropertyKey =
                DependencyProperty.RegisterReadOnly(
                        "NonFrozenRowsViewportVerticalOffset",
                        typeof(double),
                        typeof(System.Windows.Controls.DataGrid),
                        new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Dependency property for NonFrozenRowsViewportVerticalOffset Property
        /// </summary>
        public static readonly DependencyProperty NonFrozenRowsViewportVerticalOffsetProperty = NonFrozenRowsViewportVerticalOffsetPropertyKey.DependencyProperty;

        /// <summary>
        /// Property which gets/sets the start y coordinate of non frozen rows in view port
        /// </summary>
        public double NonFrozenRowsViewportVerticalOffset
        {
            get
            {
                return (double)GetValue(NonFrozenRowsViewportVerticalOffsetProperty);
            }
            internal set
            {
                SetValue(NonFrozenRowsViewportVerticalOffsetPropertyKey, value);
            }
        }

        /// <summary>
        /// Method which gets called when Vertical scroll occurs on the scroll viewer of datagrid.
        /// Forwards the call to rows and header presenter.
        /// </summary>
        internal void OnVerticalScroll()
        {
            FooterDataGridRowsPresenter panel = (FooterDataGridRowsPresenter)Helper.GetVisualChild<FooterDataGridRowsPresenter>(this);
            panel.InvalidateArrange();
        }

        #endregion
    }
}
