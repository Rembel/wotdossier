using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WotDossier.Framework.Controls.AutoCompleteTextBox
{
    public class SelectionAdapter
    {
        #region "Fields"

        #endregion

        #region "Constructors"

        public SelectionAdapter(Selector selector)
        {
            SelectorControl = selector;
            SelectorControl.PreviewMouseUp += OnSelectorMouseDown;
            //SelectorControl.SelectionChanged += SelectorControlOnSelectionChanged ;
        }

        private void SelectorControlOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (Commit != null)
            {
                Commit();
            }
        }

        #endregion

        #region "Events"

        public event Action Cancel;
        public event Action Commit;
        public event Action SelectionChanged;

        #endregion

        #region "Properties"

        public Selector SelectorControl { get; set; }

        #endregion

        #region "Methods"

        public void HandleKeyDown(KeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Down:
                    IncrementSelection();
                    break;
                case Key.Up:
                    DecrementSelection();
                    break;
                case Key.Enter:
                    if (Commit != null)
                    {
                        Commit();
                    }

                    break;
                case Key.Escape:
                    if (Cancel != null)
                    {
                        Cancel();
                    }

                    break;
            }
        }

        private void DecrementSelection()
        {
            if (SelectorControl.SelectedIndex == -1)
            {
                SelectorControl.SelectedIndex = SelectorControl.Items.Count - 1;
            }
            else
            {
                SelectorControl.SelectedIndex -= 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void IncrementSelection()
        {
            if (SelectorControl.SelectedIndex == SelectorControl.Items.Count - 1)
            {
                SelectorControl.SelectedIndex = -1;
            }
            else
            {
                SelectorControl.SelectedIndex += 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void OnSelectorMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Commit != null)
            {
                Commit();
            }
        }

        #endregion
    }
}