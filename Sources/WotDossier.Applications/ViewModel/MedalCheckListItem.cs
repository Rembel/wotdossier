using System;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel
{
    public class MedalCheckListItem : CheckListItem<int>
    {
        public Medal Medal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedalCheckListItem" /> class.
        /// </summary>
        /// <param name="onCheckedChanged">The on checked changed.</param>
        /// <param name="medal">The medal.</param>
        public MedalCheckListItem(Medal medal, Action<CheckListItem<int>, bool> onCheckedChanged)
            : base(medal.Id, medal.Name, false, onCheckedChanged)
        {
            Medal = medal;
        }
    }
}