using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;
using WotDossier.Domain.Player;

namespace WotDossier.Applications.ViewModel
{
    public class CommonPlayerStatistic : INotifyPropertyChanged
    {
        private readonly List<CommonPlayerStatistic> _list;
        public int BattlesCount { get; set; }
        public int BattlesCountDelta { get; set; }
        public int Wins { get; set; }
        public int WinsDelta { get; set; }
        public int Losses { get; set; }
        public int LossesDelta { get; set; }
        public int SurvivedBattles { get; set; }
        public int SurvivedBattlesDelta { get; set; }
        public int Xp { get; set; }
        public int XpDelta { get; set; }
        public int BattleAvgXp { get; set; }
        public int BattleAvgXpDelta { get; set; }
        public int MaxXp { get; set; }
        public int MaxXpDelta { get; set; }
        public int Frags { get; set; }
        public int FragsDelta { get; set; }
        public int Spotted { get; set; }
        public int SpottedDelta { get; set; }
        public int HitsPercents { get; set; }
        public int HitsPercentsDelta { get; set; }
        public int DamageDealt { get; set; }
        public int DamageDealtDelta { get; set; }
        public int CapturePoints { get; set; }
        public int CapturePointsDelta { get; set; }
        public int DroppedCapturePoints { get; set; }
        public int DroppedCapturePointsDelta { get; set; }
        public string Name { get; set; }
        public double Created { get; set; }
        public double Updated { get; set; }
        public DateTime Date { get; set; }
        private DateTime PreviousDate { get; set; }

        private CommonPlayerStatistic PreviousValues;

        public CommonPlayerStatistic(PlayerStat stat) : this(stat, null)
        {
        }

        public CommonPlayerStatistic(PlayerStat stat, List<CommonPlayerStatistic> list)
        {
            _list = list;
            BattlesCount = stat.data.summary.Battles_count;
            Wins = stat.data.summary.Wins;
            Losses = stat.data.summary.Losses;
            SurvivedBattles = stat.data.summary.Survived_battles;
            Xp = stat.data.experience.Xp;
            BattleAvgXp = stat.data.experience.Battle_avg_xp;
            MaxXp = stat.data.experience.Max_xp;
            Frags = stat.data.battles.Frags;
            Spotted = stat.data.battles.Spotted;
            HitsPercents = stat.data.battles.Hits_percents;
            DamageDealt = stat.data.battles.Damage_dealt;
            CapturePoints = stat.data.battles.Capture_points;
            DroppedCapturePoints = stat.data.battles.Dropped_capture_points;
            Date = Utils.UnixDateToDateTime((long) stat.data.updated_at);
            Created = stat.data.created_at;
            Updated = stat.data.updated_at;
            Name = stat.data.name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
