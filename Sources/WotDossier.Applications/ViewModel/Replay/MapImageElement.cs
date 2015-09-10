using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class MapImageElement
    {
        private string _owner = "neutral";
        public double X { get; set; }
        public double Y { get; set; }
        public string Type { get; set; }
        public int Team { get; set; }
        public int Position { get; set; }

        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
    }
}
