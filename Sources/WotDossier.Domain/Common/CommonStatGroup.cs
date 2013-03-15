using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Common
{
    public class CommonStatGroup
    {
        public string Name { get; set; }

        public List<CommonStatRow> Type { get; set; }
    }
}
