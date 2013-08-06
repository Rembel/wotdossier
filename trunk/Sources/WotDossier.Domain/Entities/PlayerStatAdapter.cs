using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Entities
{
    public class PlayerStatAdapter
    {
        private readonly List<TankJson> _tanks;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks)
        {
            _tanks = tanks;

            Battles_count = _tanks.Sum(x => x.Tankdata.battlesCount);
            Wins = _tanks.Sum(x => x.Tankdata.wins);
            Losses = _tanks.Sum(x => x.Tankdata.losses);
            Survived_battles = _tanks.Sum(x => x.Tankdata.survivedBattles);
            Xp = _tanks.Sum(x => x.Tankdata.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp/Battles_count;
            }
            Max_xp = _tanks.Max(x => x.Tankdata.maxXP);
            Frags = _tanks.Sum(x => x.Tankdata.frags);
            Spotted = _tanks.Sum(x => x.Tankdata.spotted);
            Hits_percents = (int)(_tanks.Sum(x => x.Tankdata.hits) / ((double)_tanks.Sum(x => x.Tankdata.shots)) * 100.0);
            Damage_dealt = _tanks.Sum(x => x.Tankdata.damageDealt);
            Capture_points = _tanks.Sum(x => x.Tankdata.capturePoints);
            Dropped_capture_points = _tanks.Sum(x => x.Tankdata.droppedCapturePoints);
            Updated = _tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier*x.Tankdata.battlesCount)/(double) Battles_count;
            }
        }

        public PlayerStatAdapter(PlayerStat stat)
        {
            Battles_count = stat.data.summary.Battles_count;
            Wins = stat.data.summary.Wins;
            Losses = stat.data.summary.Losses;
            Survived_battles = stat.data.summary.Survived_battles;
            Xp = stat.data.experience.Xp;
            Battle_avg_xp = stat.data.experience.Battle_avg_xp;
            Max_xp = stat.data.experience.Max_xp;
            Frags = stat.data.battles.Frags;
            Spotted = stat.data.battles.Spotted;
            Hits_percents = stat.data.battles.Hits_percents;
            Damage_dealt = stat.data.battles.Damage_dealt;
            Capture_points = stat.data.battles.Capture_points;
            Dropped_capture_points = stat.data.battles.Dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.data.updated_at).ToLocalTime();
            if (Battles_count > 0)
            {
                AvgLevel = stat.data.vehicles.Sum(x => x.level*x.battle_count)/(double) stat.data.summary.Battles_count;
            }
        }

        public int Battles_count { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Survived_battles { get; set; }

        public int Xp { get; set; }

        public int Battle_avg_xp { get; set; }

        public int Max_xp { get; set; }

        public int Frags { get; set; }

        public int Spotted { get; set; }

        public int Hits_percents { get; set; }

        public int Damage_dealt { get; set; }

        public int Capture_points { get; set; }

        public int Dropped_capture_points { get; set; }

        public DateTime Updated { get; set; }

        public double AvgLevel { get; set; }
    }
}