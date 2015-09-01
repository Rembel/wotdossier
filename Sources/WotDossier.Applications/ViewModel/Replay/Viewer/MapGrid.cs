using System.Collections.Generic;
using System.Windows;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class MapGrid
    {
        private readonly Rect _bounds;
        private readonly Map _mapDescription;
        private readonly string _gameplayId;
        private readonly int _team;
        private readonly int _width;
        private readonly int _height;

        private List<MapImageElement> _elements;
        public List<MapImageElement> Elements
        {
            get
            {
                if (_elements == null)
                {
                    if (_mapDescription.Config.GameplayTypes.ContainsKey(_gameplayId))
                    {
                        GameplayDescription gameplayDescription = _mapDescription.Config.GameplayTypes[_gameplayId];

                        Elements = GetMapImageElements(gameplayDescription, _team);
                    }
                }
                return _elements;
            }
            set { _elements = value; }
        }

        public MapGrid(Map mapDescription, string gameplayId, int team, int width, int height)
        {
            _bounds = mapDescription.Config.BoundingBox;
            _mapDescription = mapDescription;
            _gameplayId = gameplayId;
            _team = team;
            _width = width;
            _height = height;
        }

        public Point? game_to_map_coord(Point position)
        {
            return game_to_map_coord(new float[] {(float) position.X, (float) position.Y});
        }

        public Point? game_to_map_coord(float [] position)
        {
            try
            {
                double x = 0;
                double y = 0;
                
                if (position.Length == 3)
                {
                    x = position[0];
                    y = position[2];
                }
                else if (position.Length == 2)
                {
                    x = position[0];
                    y = position[1];
                }
                
                x = (x - _bounds.X)*(_width/(_bounds.Width + 1)) - 14;
                y = (_bounds.Bottom - y) * (_height / (_bounds.Height + 1)) - 9;

                return new Point(x, y);
            }
            catch
            {
                //console.log('mapGrid.game_to_map_coord error: ', err);
                return null;
            }
        }

        public List<MapImageElement> GetMapImageElements(GameplayDescription gameplayDescription, int replayUserTeam)
        {
            List<MapImageElement> elements = new List<MapImageElement>();

            int xShift = -16;
            int yShift = -20;

            foreach (KeyValuePair<int, List<Point>> teamBasePosition in gameplayDescription.TeamBasePositions)
            {
                int i = 1;
                foreach (var point in teamBasePosition.Value)
                {
                    MapImageElement element = new MapImageElement();
                    Point? mapCoord = game_to_map_coord(point);
                    element.X = mapCoord.Value.X + xShift;
                    element.Y = mapCoord.Value.Y + yShift;
                    element.Type = "base";
                    element.Team = teamBasePosition.Key;
                    element.Owner = GetElementOwner(element.Team, replayUserTeam);
                    element.Position = i++;
                    elements.Add(element);
                }
            }
            foreach (KeyValuePair<int, List<Point>> teamSpawns in gameplayDescription.TeamSpawnPoints)
            {
                int i = 1;
                foreach (var point in teamSpawns.Value)
                {
                    MapImageElement element = new MapImageElement();
                    Point? mapCoord = game_to_map_coord(point);
                    element.X = mapCoord.Value.X + xShift;
                    element.Y = mapCoord.Value.Y + yShift;
                    element.Type = "spawn";
                    element.Team = teamSpawns.Key;
                    element.Owner = GetElementOwner(element.Team, replayUserTeam);
                    element.Position = i++;
                    elements.Add(element);
                }
            }

            if (gameplayDescription.ControlPoint != null)
            {
                MapImageElement element = new MapImageElement();
                Point? mapCoord = game_to_map_coord(gameplayDescription.ControlPoint.Value);
                element.X = mapCoord.Value.X + xShift;
                element.Y = mapCoord.Value.Y + yShift;
                element.Type = "base";
                elements.Add(element);
            }
            return elements;
        }

        private string GetElementOwner(int team, int replayUserTeam)
        {
            if (team == replayUserTeam)
            {
                return "friendly";
            }
            return "enemy";
        }
    }
}
