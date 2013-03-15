using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Player
{
    public class Ratings
    {
        private Rating spotted;
        private Rating dropped_ctf_points;
        private Rating battle_avg_xp;
        private Rating xp;
        private Rating battles;
        private Rating damage_dealt;
        private Rating ctf_points;
        private Rating integrated_rating;
        private Rating battle_avg_performance;
        private Rating frags;
        private Rating battle_wins;

        public Rating Spotted
        {
            get { return spotted; }
            set { spotted = value; }
        }

        public Rating Dropped_ctf_points
        {
            get { return dropped_ctf_points; }
            set { dropped_ctf_points = value; }
        }

        public Rating Xp
        {
            get { return xp; }
            set { xp = value; }
        }

        public Rating Battles
        {
            get { return battles; }
            set { battles = value; }
        }

        public Rating Frags
        {
            get { return frags; }
            set { frags = value; }
        }

        public Rating Battle_wins
        {
            get { return battle_wins; }
            set { battle_wins = value; }
        }

        public Rating Battle_avg_performance
        {
            get { return battle_avg_performance; }
            set { battle_avg_performance = value; }
        }

        public Rating Integrated_rating
        {
            get { return integrated_rating; }
            set { integrated_rating = value; }
        }

        public Rating Ctf_points
        {
            get { return ctf_points; }
            set { ctf_points = value; }
        }

        public Rating Damage_dealt
        {
            get { return damage_dealt; }
            set { damage_dealt = value; }
        }

        public Rating Battle_avg_xp
        {
            get { return battle_avg_xp; }
            set { battle_avg_xp = value; }
        }
    }
}
