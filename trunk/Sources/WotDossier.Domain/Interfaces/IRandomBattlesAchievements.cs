namespace WotDossier.Domain.Interfaces
{
    public interface IRandomBattlesAchievements : IStatisticBattleAwards, IStatisticEpic, IStatisticSpecialAwards, IStatisticMedals, IStatisticSeries
    {
        int MedalMonolith { get; set; }
        int MedalAntiSpgFire { get; set; }
        int MedalGore { get; set; }
        int MedalCoolBlood { get; set; }
        int MedalStark { get; set; }
        int DamageRating { get; set; }
    }
}