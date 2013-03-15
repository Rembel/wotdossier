using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Player
{
    public class Battles
    {
        private int spotted;
        private int hits_percents;
        private int capture_points;
        private int damage_dealt;
        private int frags;
        private int dropped_capture_points;

        public int Spotted
        {
            get { return spotted; }
            set { spotted = value; }
        }

        public int Hits_percents
        {
            get { return hits_percents; }
            set { hits_percents = value; }
        }

        public int Capture_points
        {
            get { return capture_points; }
            set { capture_points = value; }
        }

        public int Damage_dealt
        {
            get { return damage_dealt; }
            set { damage_dealt = value; }
        }

        public int Frags
        {
            get { return frags; }
            set { frags = value; }
        }

        public int Dropped_capture_points
        {
            get { return dropped_capture_points; }
            set { dropped_capture_points = value; }
        }
    }
}
