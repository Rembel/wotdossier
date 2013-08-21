using System.Collections.Generic;
using Microsoft.Research.DynamicDataDisplay;

namespace WotDossier.Applications
{
    public class ChartsRegistry
    {
        private List<ChartPlotter> _list = new List<ChartPlotter>();

        public List<ChartPlotter> List
        {
            get { return _list; }
        }

        public void Add(ChartPlotter chart)
        {
            _list.Add(chart);
        }
    }
}
