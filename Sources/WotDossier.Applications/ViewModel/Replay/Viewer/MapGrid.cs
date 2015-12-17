using System.Collections.Generic;
using System.Windows;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class MapGrid
    {
        private readonly MapElementContext _elementContext;

        public MapElementContext ElementContext
        {
            get { return _elementContext; }
        }

        private List<MapImageElement> _elements;
        public List<MapImageElement> Elements
        {
            get
            {
                if (_elements == null)
                {
                    Elements = _elementContext.GetMapImageElements();
                }
                return _elements;
            }
            set { _elements = value; }
        }

        public MapGrid(MapElementContext elementContext)
        {
            _elementContext = elementContext;
        }
    }
}
