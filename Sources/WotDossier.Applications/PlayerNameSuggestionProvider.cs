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
            var dossierCacheFolder = Folder.GetDossierCacheFolder();
            if (Directory.Exists(dossierCacheFolder))
            {
                IEnumerable<FileInfo> files =
                    Directory.GetFiles(dossierCacheFolder, "*.dat").Select(x => new FileInfo(x));
                IEnumerable<string> suggestions =
                    files.Select(CacheFileHelper.GetPlayerName)
                        .Distinct()
                        .Where(x => x.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase));
                return suggestions;
            }
            return new string[0];
        }
    }
}
