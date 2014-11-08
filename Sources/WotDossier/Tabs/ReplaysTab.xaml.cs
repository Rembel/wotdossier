using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WotDossier.Applications;
using WotDossier.Applications.Events;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for ReplaysTab.xaml
    /// </summary>
    public partial class ReplaysTab : UserControl
    {
        private Point _startPoint;
        private int _selectedIndex;
        private const string FOLDER_DRAG_FORMAT = "folder";

        public ReplaysTab()
        {
            InitializeComponent();
            Application.Current.Exit += CurrentOnExit;
        }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            AppSettings appSettings = SettingsReader.Get();
            appSettings.ColumnInfo = dgReplays.GetColumnInformation();
            SettingsReader.Save(appSettings);
        }

        private void OnGridPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(null);
        }

        private void OnGridMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if ((e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                DataGridRow row = FindAnchestor<DataGridRow>((DependencyObject)e.OriginalSource);

                if (row != null)
                {
                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject(FOLDER_DRAG_FORMAT, dgReplays.SelectedItems);
                    DragDrop.DoDragDrop(row, dragData, DragDropEffects.Move);
                }
            }
        }

        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(FOLDER_DRAG_FORMAT) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(FOLDER_DRAG_FORMAT))
            {
                ReplayFolder target = (ReplayFolder) ((TreeViewItem) sender).DataContext;
                IList replayFiles = e.Data.GetData(FOLDER_DRAG_FORMAT) as IList;
                EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Publish(new ReplayFileMoveEventArgs { TargetFolder = target, ReplayFiles = replayFiles });
                e.Handled = true;
            }
        }

        private void DgReplays_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                _selectedIndex = dgReplays.SelectedIndex;
            }
        }

        private void DgReplays_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && e.AddedItems.Count == 0)
            {
                if (dgReplays.Items.Count > _selectedIndex)
                {
                    dgReplays.SelectedIndex = _selectedIndex;
                }
                else
                {
                    dgReplays.SelectedIndex = _selectedIndex - 1;
                }
                if (dgReplays.SelectedIndex >= 0)
                {
                    DataGridRow dgrow = (DataGridRow)dgReplays.ItemContainerGenerator.ContainerFromItem(dgReplays.Items[dgReplays.SelectedIndex]);
                    if (dgrow != null)
                    {
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }
    }
}
