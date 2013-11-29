using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WotDossier.Framework.Controls.AutoCompleteTextBox;

namespace WotDossier.Applications
{
    public class PlayerNameSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            IEnumerable<FileInfo> files = Directory.GetFiles(Folder.GetDossierCacheFolder(), "*.dat").Select(x => new FileInfo(x));
            return files.Select(CacheHelper.GetPlayerName).Distinct();
        }
    }
}
