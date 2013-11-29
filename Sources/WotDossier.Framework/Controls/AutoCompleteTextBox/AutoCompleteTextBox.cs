using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WotDossier.Framework.Controls.AutoCompleteTextBox
{
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSelector, Type = typeof(Selector))]
    public class AutoCompleteTextBox : Control
    {
        #region "Fields"

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";
        public const string PartSelector = "PART_Selector";
    
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register("DisplayMember", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsPopulatingProperty = DependencyProperty.Register("IsPopulating", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(ISuggestionProvider), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        private BindingEvaluator _bindingEvaluator;
        private TextBox _editor;
        private DispatcherTimer _fetchTimer;
        private string _filter;
        private bool _isUpdatingText;
        private Selector _itemsSelector;
        private Popup _popup;

        private SelectionAdapter _selectionAdapter;
        #endregion

        #region "Constructors"

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #endregion

        #region "Properties"

        public BindingEvaluator BindingEvaluator
        {
            get { return _bindingEvaluator; }
            set { _bindingEvaluator = value; }
        }

        public int Delay
        {
            get { return (int) GetValue(DelayProperty); }

            set { SetValue(DelayProperty, value); }
        }

        public string DisplayMember
        {
            get { return (string) GetValue(DisplayMemberProperty); }

            set { SetValue(DisplayMemberProperty, value); }
        }

        public TextBox Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public DispatcherTimer FetchTimer
        {
            get { return _fetchTimer; }
            set { _fetchTimer = value; }
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public bool IsDropDownOpen
        {
            get { return (bool) GetValue(IsDropDownOpenProperty); }

            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public bool IsPopulating
        {
            get { return (bool) GetValue(IsPopulatingProperty); }

            set { SetValue(IsPopulatingProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool) GetValue(IsReadOnlyProperty); }

            set { SetValue(IsReadOnlyProperty, value); }
        }

        public Selector ItemsSelector
        {
            get { return _itemsSelector; }
            set { _itemsSelector = value; }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }

            set { SetValue(ItemTemplateProperty, value); }
        }

        public Popup Popup
        {
            get { return _popup; }
            set { _popup = value; }
        }

        public ISuggestionProvider Provider
        {
            get { return (ISuggestionProvider) GetValue(ProviderProperty); }

            set { SetValue(ProviderProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }

            set { SetValue(SelectedItemProperty, value); }
        }

        public SelectionAdapter SelectionAdapter
        {
            get { return _selectionAdapter; }
            set { _selectionAdapter = value; }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region "Methods"

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Editor = (TextBox) Template.FindName(PartEditor, this);
            Popup = (Popup) Template.FindName(PartPopup, this);
            ItemsSelector = (Selector) Template.FindName(PartSelector, this);
            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (Editor != null)
            {
                Editor.TextChanged += OnEditroTextChanged;
                Editor.PreviewKeyDown += OnEditorKeyDown;
                Editor.LostFocus += OnEditorLostFocus;
            }
            if (Popup != null)
            {
                Popup.StaysOpen = false;
                Popup.Opened += OnPopupOpened;
                Popup.Closed += OnPopupClosed;
            }
            if (ItemsSelector != null)
            {
                SelectionAdapter = new SelectionAdapter(ItemsSelector);
                SelectionAdapter.Commit += OnSelectionAdapterCommit;
                SelectionAdapter.Cancel += OnSelectionAdapterCancel;
                SelectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
            }
        }

        private string GetDisplayText(object dataItem)
        {
            if (BindingEvaluator == null)
            {
                BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                return dataItem.ToString();
            }
            return BindingEvaluator.Evaluate(dataItem);
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (SelectionAdapter != null)
            {
                SelectionAdapter.HandleKeyDown(e);
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = false;
        }

        private void OnEditroTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText)
                return;
            if (FetchTimer == null)
            {
                FetchTimer = new DispatcherTimer();
                FetchTimer.Interval = TimeSpan.FromMilliseconds(Delay);
                FetchTimer.Tick += OnFetchTimerTick;
            }
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            if (Editor.Text.Length > 0)
            {
                FetchTimer.IsEnabled = true;
                FetchTimer.Start();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            if (Provider != null && ItemsSelector != null)
            {
                Filter = Editor.Text;
                ItemsSelector.ItemsSource = Provider.GetSuggestions(Editor.Text);
                ItemsSelector.SelectedIndex = -1;
                if (ItemsSelector.HasItems && IsKeyboardFocusWithin)
                {
                    IsDropDownOpen = true;
                }
                else
                {
                    IsDropDownOpen = false;
                }
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
        }

        private void OnSelectionAdapterCancel()
        {
            IsDropDownOpen = false;
        }

        private void OnSelectionAdapterCommit()
        {
            SelectedItem = ItemsSelector.SelectedItem;
            _isUpdatingText = true;
            Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);
            _isUpdatingText = false;
            IsDropDownOpen = false;
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;
            if (ItemsSelector.SelectedItem == null)
            {
                Editor.Text = Filter;
            }
            else
            {
                Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);
            }
            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            _isUpdatingText = false;
        }

        #endregion

    }
}