using System.Collections.Generic;
using System.Windows;

namespace WotDossier.Domain
{
    public class GameplayDescription
    {
        private Dictionary<int, List<Point>> _teamBasePositions = new Dictionary<int, List<Point>>();
        private Dictionary<int, List<Point>> _teamSpawnPoints = new Dictionary<int, List<Point>>();
        //"teamSpawnPoints": {
        //  "team1": {
        //    "position": [
        //      "-37.348 393.967",
        //      "-229.021 417.866"
        //    ]
        //  },
        //  "team2": {
        //    "position": [
        //      "10.414 -410.010",
        //      "-331.576 -428.692"
        //    ]
        //  }
        //},
        public Dictionary<int, List<Point>> TeamSpawnPoints
        {
            get { return _teamSpawnPoints; }
            set { _teamSpawnPoints = value; }
        }

        //"teamBasePositions": {
        //  "team1": {
        //    "position1": "-163.430 256.265",
        //    "position2": "154.715 150.255"
        //  },
        //  "team2": ""
        //}
        public Dictionary<int, List<Point>> TeamBasePositions
        {
            get { return _teamBasePositions; }
            set { _teamBasePositions = value; }
        }

        public Point? ControlPoint { get; set; }
    }
}