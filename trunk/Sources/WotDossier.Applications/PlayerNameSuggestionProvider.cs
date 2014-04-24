using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WotDossier.Framework.Controls.AutoCompleteTextBox;

namespace WotDossier.Applications
{
    public class PlayerNameSuggestionProvider : ISuggestionProvider
    {
        /// <summary>
        /// Gets the suggestions.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IEnumerable GetSuggestions(string filter)
        {
            IEnumerable<FileInfo> files = Directory.GetFiles(Folder.GetDossierCacheFolder(), "*.dat").Select(x => new FileInfo(x));
            IEnumerable<string> suggestions = files.Select(CacheHelper.GetPlayerName).Distinct().Where(x => x.StartsWith(filter,StringComparison.InvariantCultureIgnoreCase));
            return suggestions;
        }
    }
}
