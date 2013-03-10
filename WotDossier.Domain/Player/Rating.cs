using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Player
{
    public class Rating
    {
        private int place;
        private int value;

        public int Place
        {
            get { return place; }
            set { place = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
