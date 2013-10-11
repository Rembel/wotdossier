using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WotDossier.Framework.Controls
{
    public class AttachedProperties
    {
        public static readonly DependencyProperty UpdateSourceOnKeyProperty =
    DependencyProperty.RegisterAttached("UpdateSourceOnKey",
    typeof(Key), typeof(TextBox), new FrameworkPropertyMetadata(Key.None));

        public static void SetUpdateSourceOnKey(UIElement element, Key value)
        {
            element.PreviewKeyUp += TextBoxKeyUp;
            element.SetValue(UpdateSourceOnKeyProperty, value);
        }

        static void TextBoxKeyUp(object sender, KeyEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            var propertyValue = (Key)textBox.GetValue(UpdateSourceOnKeyProperty);
            if (e.Key != propertyValue) return;

            var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null) bindingExpression.UpdateSource();
        }

        public static Key GetUpdateSourceOnKey(UIElement element)
        {
            return (Key)element.GetValue(UpdateSourceOnKeyProperty);
        }
    }
}
