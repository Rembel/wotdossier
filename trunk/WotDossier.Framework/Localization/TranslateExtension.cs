using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WotDossier.Framework.Localization
{
    /// <summary>
    /// The Translate Markup extension returns a binding to a TranslationData
    /// that provides a translated resource of the specified key
    /// http://www.wpftutorial.net/LocalizeMarkupExtension.html
    /// <TextBlock Text="{l:Translate CustomerForm.FirstName}" />
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        #region Private Members

        private string _key;

        #endregion

        #region Construction

        public TranslateExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslateExtension(string key)
        {
            _key = key;
        }

        #endregion

        [ConstructorArgument("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value;}
        }

        /// <summary>
        /// See <see cref="MarkupExtension.ProvideValue" />
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
                  {
                      Source = new TranslationData(_key)
                  };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
