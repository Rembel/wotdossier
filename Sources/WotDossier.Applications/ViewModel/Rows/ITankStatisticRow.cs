using System.Collections.Generic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankStatisticRow : IStatisticBase, IStatisticExtended, IStatisticRatings, IRandomBattlesAchievements, ITeamBattlesAchievements, IHistoricalBattlesAchievements, IClanBattlesAchievements, IFortAchievements, ITankFilterable, ITankDescription
    {
        int PlayerId { get; set; }
        string PlayerName { get; set; }

        TankIcon Icon { get; set; }

        int MarksOnGunSort { get; set; }

        IEnumerable<FragsJson> TankFrags { get; set; }

        IEnumerable<ITankStatisticRow> GetAll();

        void SetPreviousStatistic(ITankStatisticRow model);

        ITankStatisticRow PreviousStatistic { get; }
    }
}