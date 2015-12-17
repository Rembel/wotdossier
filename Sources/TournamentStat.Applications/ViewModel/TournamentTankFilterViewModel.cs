using WotDossier.Applications.ViewModel.Filter;

namespace TournamentStat.Applications.ViewModel
{
    public class TournamentTankFilterViewModel : TankFilterViewModel
    {
        public override bool FilterCondition<T>(T tank)
        {
            return (tank as TournamentTank).IsSelected || base.FilterCondition(tank);
        }
    }
}
