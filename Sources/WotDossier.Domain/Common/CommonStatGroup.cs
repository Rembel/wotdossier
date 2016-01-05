using System.Collections.Generic;

namespace WotDossier.Domain.Common
{
    public class CommonStatGroup
    {
        public string Name { get; set; }

        public List<CommonStatRow> Type { get; set; }
    }
}
