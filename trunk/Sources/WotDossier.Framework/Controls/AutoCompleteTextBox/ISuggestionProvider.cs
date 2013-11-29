using System.Collections;

namespace WotDossier.Framework.Controls.AutoCompleteTextBox
{
    public interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }
}
