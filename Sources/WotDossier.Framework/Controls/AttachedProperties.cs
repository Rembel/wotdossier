using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WotDossier.Framework.Controls
{
    public class AttachedProperties
    {
        public static readonly DependencyProperty UpdateSourceOnKeyProperty = DependencyProperty.RegisterAttached("UpdateSourceOnKey", typeof(Key), typeof(TextBox), new FrameworkPropertyMetadata(Key.None));

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

        public static readonly DependencyProperty InputBindingsProperty =
            DependencyProperty.RegisterAttached("InputBindings", typeof(InputBindingCollection), typeof(AttachedProperties), new FrameworkPropertyMetadata(new InputBindingCollection(),
            (sender, e) =>
            {
                var element = sender as UIElement;
                if (element == null) return;
                element.InputBindings.Clear();
                element.InputBindings.AddRange((InputBindingCollection)e.NewValue);
            }));

        public static InputBindingCollection GetInputBindings(UIElement element)
        {
            return (InputBindingCollection)element.GetValue(InputBindingsProperty);
        }

        public static void SetInputBindings(UIElement element, InputBindingCollection inputBindings)
        {
            element.SetValue(InputBindingsProperty, inputBindings);
        }
    }
}
