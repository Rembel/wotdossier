using System;
using WotDossier.Applications.ViewModel;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.Events
{
    public class AddFavoritePlayerEvent : BaseEvent<SearchResultRowViewModel>
    {
    }
}