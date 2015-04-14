using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WotDossier.Common;

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

        private bool _updatingColumnInfo;
        
        public static readonly DependencyProperty ColumnInfoProperty = DependencyProperty.Register("ColumnInfo",
                typeof(ObservableCollection<ColumnInformation>), typeof(FooterDataGrid),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ColumnInfoChangedCallback)
            );

        public ObservableCollection<ColumnInformation> ColumnInfo
        {
            get { return (ObservableCollection<ColumnInformation>)GetValue(ColumnInfoProperty); }
            set { SetValue(ColumnInfoProperty, value); }
        }

        public static readonly DependencyProperty HideColumnChooseProperty = DependencyProperty.Register(
            "HideColumnChooser", typeof(Boolean), typeof(FooterDataGrid), new PropertyMetadata(true));

        public Boolean HideColumnChooser
        {
            get { return (Boolean)GetValue(HideColumnChooseProperty); }
            set { SetValue(HideColumnChooseProperty, value); }
        }

        #region Constructors

        static FooterDataGrid()
        {
            Type ownerType = typeof(FooterDataGrid);
            DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(ownerType));
        }

        public FooterDataGrid()
        {
        }

        #endregion

        private static void ColumnInfoChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var grid = (FooterDataGrid)dependencyObject;
            if (!grid._updatingColumnInfo) { grid.ColumnInfoChanged(); }
        }

        private void ColumnInfoChanged()
        {
            Items.SortDescriptions.Clear();
            foreach (var column in ColumnInfo)
            {
                var realColumn = Columns.FirstOrDefault(x => column.SortMemberPath.Equals(x.SortMemberPath));
                if (realColumn == null) { continue; }
                column.Apply(realColumn, Columns.Count, Items.SortDescriptions);
            }
        }

        public string GetColumnInformation()
        {
            UpdateColumnInfo();
            return XmlSerializer.StoreObjectInXml(ColumnInfo);
        }

        private void UpdateColumnInfo()
        {
            _updatingColumnInfo = true;
            ColumnInfo = new ObservableCollection<ColumnInformation>(Columns.Select(x => new ColumnInformation(x)));
            _updatingColumnInfo = false;
        }

        public bool SetColumnInformation(string xmlOfColumnInformation)
        {
            try
            {
                ColumnInfo = XmlSerializer.LoadObjectFromXml<ObservableCollection<ColumnInformation>>(xmlOfColumnInformation);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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

        public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register("SelectedItems", typeof(IList), typeof(FooterDataGrid), new PropertyMetadata(default(IList)));

        public new IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { throw new Exception("This property is read-only. To bind to it you must use 'Mode=OneWayToSource'."); }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SetValue(SelectedItemsProperty, base.SelectedItems);
        }
    }
}
