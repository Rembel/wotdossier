using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WotDossier.Applications;
using WotDossier.Applications.Events;
using WotDossier.Applications.ViewModel;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for ReplaysTab.xaml
    /// </summary>
    public partial class ReplaysTab : UserControl
    {
        private Point _startPoint;
        private const string FOLDER_DRAG_FORMAT = "folder";

        public ReplaysTab()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
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

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                DataGridRow row = FindAnchestor<DataGridRow>((DependencyObject)e.OriginalSource);

                if (row != null)
                {
                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject(FOLDER_DRAG_FORMAT, row.DataContext);
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
                ReplayFile replayFile = e.Data.GetData(FOLDER_DRAG_FORMAT) as ReplayFile;
                EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Publish(new ReplayFileMoveEventArgs { TargetFolder = target, ReplayFile = replayFile });
                e.Handled = true;
            }
        }
    }
}
