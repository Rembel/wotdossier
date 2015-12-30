using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class dbversion
    {
        public int id { get; set; }
        public DateTime applied { get; set; }
        public string schemaversion { get; set; }
    }
}
