using System;
using System.Collections;

namespace WotDossier.Framework.Controls.AutoCompleteTextBox
{
    public class SuggestionProvider : ISuggestionProvider
    {
        private readonly Func<string, IEnumerable> _method;

        public SuggestionProvider(Func<string, IEnumerable> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            _method = method;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return _method(filter);
        }
    }
}