using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Player
{
    public class Summary
    {
        private int wins;
        private int losses;
        private int battles_count;
        private int survived_battles;

        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }

        public int Losses
        {
            get { return losses; }
            set { losses = value; }
        }

        public int Battles_count
        {
            get { return battles_count; }
            set { battles_count = value; }
        }

        public int Survived_battles
        {
            get { return survived_battles; }
            set { survived_battles = value; }
        }
    }
}
