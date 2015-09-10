using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace WotDossier.Domain
{
    [DataContract]
    public class MapConfig
    {
        private Dictionary<string, GameplayDescription> _gameplayTypes = new Dictionary<string, GameplayDescription>();
        public Rect BoundingBox { get; set; }

        public Dictionary<string, GameplayDescription> GameplayTypes
        {
            get { return _gameplayTypes; }
            set { _gameplayTypes = value; }
        }
    }
}