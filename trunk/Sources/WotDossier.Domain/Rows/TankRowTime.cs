using System;
using WotDossier.Common;

namespace WotDossier.Domain.Rows
{
    public class TankRowTime : TankRowBase
    {
         public DateTime LastBattle { get; set; }
         public TimeSpan PlayTime { get; set; }
         public TimeSpan AverageBattleTime { get; set; }

         public TankRowTime(Tank tank)
        {
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            LastBattle = Utils.UnixDateToDateTime(tank.Tankdata.lastBattleTime);
            PlayTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime);
            AverageBattleTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime / tank.Tankdata.battlesCount);
        }
    }
}
