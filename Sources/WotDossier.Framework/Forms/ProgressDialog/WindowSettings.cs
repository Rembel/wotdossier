//
// Parago Media GmbH & Co. KG, Jürgen Bäurle (jbaurle@parago.de)
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System.Windows;

namespace WotDossier.Framework.Forms.ProgressDialog
{
	public class WindowSettings
	{
		#region public bool HideCloseButton (attached)

		public static readonly DependencyProperty HideCloseButtonProperty =
			 DependencyProperty.RegisterAttached("HideCloseButton", typeof(bool), typeof(WindowSettings), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnHideCloseButtonPropertyChanged)));

		public static bool GetHideCloseButton(FrameworkElement element)
		{
			return (bool)element.GetValue(HideCloseButtonProperty);
		}

		public static void SetHideCloseButton(FrameworkElement element, bool hideCloseButton)
		{
			element.SetValue(HideCloseButtonProperty, hideCloseButton);
		}

		static void OnHideCloseButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;

			if(window != null)
			{
				var hideCloseButton = (bool)e.NewValue;

				if(hideCloseButton && !GetIsCloseButtonHidden(window))
				{
					if(!window.IsLoaded)
						window.Loaded += OnWindowLoaded;
					else
						NativeMethods.HideCloseButton(window);

					SetIsCloseButtonHidden(window, true);
				}
				else if(!hideCloseButton && GetIsCloseButtonHidden(window))
				{
					if(!window.IsLoaded)
						window.Loaded -= OnWindowLoaded;
					else
                        NativeMethods.ShowCloseButton(window);

					SetIsCloseButtonHidden(window, false);
				}
			}
		}

		static readonly RoutedEventHandler OnWindowLoaded = (s, e) => {

			if(s is Window)
			{
				Window window = s as Window;
                NativeMethods.HideCloseButton(window);
				window.Loaded -= OnWindowLoaded;
			}

		};

		#endregion

		#region public bool IsCloseButtonHidden (readonly attached)

		static readonly DependencyPropertyKey IsHiddenCloseButtonKey =
			DependencyProperty.RegisterAttachedReadOnly("IsCloseButtonHidden", typeof(bool), typeof(WindowSettings), new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsCloseButtonHiddenProperty =
			 IsHiddenCloseButtonKey.DependencyProperty;

		public static bool GetIsCloseButtonHidden(FrameworkElement element)
		{
			return (bool)element.GetValue(IsCloseButtonHiddenProperty);
		}

		static void SetIsCloseButtonHidden(FrameworkElement element, bool isCloseButtonHidden)
		{
			element.SetValue(IsHiddenCloseButtonKey, isCloseButtonHidden);
		}

		#endregion
		
	}
}
