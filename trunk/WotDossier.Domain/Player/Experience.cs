using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Player
{
    public class Experience
    {
        private int xp;
        private int battle_avg_xp;
        private int max_xp;

        public int Xp
        {
            get { return xp; }
            set { xp = value; }
        }

        public int Battle_avg_xp
        {
            get { return battle_avg_xp; }
            set { battle_avg_xp = value; }
        }

        public int Max_xp
        {
            get { return max_xp; }
            set { max_xp = value; }
        }
    }
}
