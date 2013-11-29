using System;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Framework.Controls.AutoCompleteTextBox
{
    public class BindingEvaluator : FrameworkElement
    {
        #region "Fields"

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof (string),
            typeof (BindingEvaluator), new FrameworkPropertyMetadata(String.Empty));

        #endregion 'Fields

        #region "Constructors"

        public BindingEvaluator(Binding binding)
        {
            ValueBinding = binding;
            SetBinding(ValueProperty, binding);
        }

        #endregion 'Constructors

        #region "Properties"

        public String Value
        {
            get { return (string) GetValue(ValueProperty); }

            set { SetValue(ValueProperty, value); }
        }

        public Binding ValueBinding { get; set; }

        #endregion 'Properties

        #region "Methods"

        public string Evaluate(Object dataItem)
        {
            DataContext = dataItem;
            return Value;
        }

        #endregion 'Methods
    }
}