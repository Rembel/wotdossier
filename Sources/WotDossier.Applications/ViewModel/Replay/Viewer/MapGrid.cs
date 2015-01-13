using System.Collections.Generic;
using System.Windows;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class MapGrid
    {
        private readonly Rect _bounds;
        private readonly int _width;
        private readonly int _height;

        public MapGrid(Rect bounds, int width, int height)
        {
            _bounds = bounds;
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
    }
}
