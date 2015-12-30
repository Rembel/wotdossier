using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class replay
    {
        public Guid uid { get; set; }
        public int id { get; set; }
        public string link { get; set; }
        public long playerid { get; set; }
        public byte[] raw { get; set; }
        public long replayid { get; set; }
        public int rev { get; set; }
    }
}
