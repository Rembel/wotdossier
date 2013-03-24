using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain
{
    public class TankContour
    {
        public static TankContour Empty = new TankContour{iconid = string.Empty, x = 0, y = 0, height = 1, width = 1};

        public string iconid;
        public int country_id;
        public string country_code;
        public int x;
        public int y;
        public int height;
        public int width;
    }
}
