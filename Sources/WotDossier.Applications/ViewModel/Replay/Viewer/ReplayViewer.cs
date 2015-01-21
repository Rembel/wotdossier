using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using WotDossier.Applications.Parser;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class ReplayViewer : INotifyPropertyChanged
    {
        private const int MAP_CONTROL_SIZE = 500;

        #region Properties and fields

        private readonly Domain.Replay.Replay _replay;
        private int _periodLength = -1;
        private float _clockAtPeriod;
        private int _updateSpeed = 5;

        private List<MapVehicle> _vehicles;

        public List<MapVehicle> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged("Vehicles");
            }
        }

        public List<MapImageElement> Elements
        {
            get { return _elements; }
            set
            {
                _elements = value;
                OnPropertyChanged("Elements");
            }
        }

        private int _enemiesCapturePoints;

        public int EnemiesCapturePoints
        {
            get { return _enemiesCapturePoints; }
            set
            {
                _enemiesCapturePoints = value;
                OnPropertyChanged("EnemiesCapturePoints");
            }
        }

        private int _alliesCapturePoints;

        public int AlliesCapturePoints
        {
            get { return _alliesCapturePoints; }
            set
            {
                _alliesCapturePoints = value;
                OnPropertyChanged("AlliesCapturePoints");
            }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        private List<MapVehicle> _secondTeam;
        public List<MapVehicle> SecondTeam
        {
            get { return _secondTeam; }
            set
            {
                _secondTeam = value;
                OnPropertyChanged("SecondTeam");
            }
        }

        private List<MapVehicle> _firstTeam;
        public List<MapVehicle> FirstTeam
        {
            get { return _firstTeam; }
            set
            {
                _firstTeam = value;
                OnPropertyChanged("FirstTeam");
            }
        }

        public MapVehicle ReplayUser { get; set; }
        
        private readonly MapGrid _mapGrid;
        public MapGrid MapGrid
        {
            get { return _mapGrid; }
        }

        private float _clock;
        public float Clock
        {
            get { return _clock; }
            set
            {
                _clock = value;
                OnPropertyChanged("Clock");
            }
        }

        public int CellSize { get; set; }

        private bool _click;
        public bool Click
        {
            get { return _click; }
            set
            {
                _click = value;
                OnPropertyChanged("Click");
            }
        }

        private int _cellX;

        public int CellX
        {
            get { return _cellX; }
            set
            {
                _cellX = value;
                OnPropertyChanged("CellX");
            }
        }

        private int _cellY;
        public int CellY
        {
            get { return _cellY; }
            set
            {
                _cellY = value;
                OnPropertyChanged("CellY");
            }
        }

        private int _firstTeamKills;
        public int FirstTeamKills
        {
            get { return _firstTeamKills; }
            set
            {
                _firstTeamKills = value;
                OnPropertyChanged("FirstTeamKills");
            }
        }

        private int _secondTeamKills;
        private List<MapImageElement> _elements;
        private BaseParser _parser;

        public int SecondTeamKills
        {
            get { return _secondTeamKills; }
            set
            {
                _secondTeamKills = value;
                OnPropertyChanged("SecondTeamKills");
            }
        }

        #endregion

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public ReplayViewer(Domain.Replay.Replay replay, List<MapVehicle> vehicles)
        {
            _replay = replay;

            var map = Dictionaries.Instance.Maps[replay.datablock_1.mapName];

            _mapGrid = new MapGrid(map.Config.BoundingBox, MAP_CONTROL_SIZE, MAP_CONTROL_SIZE);

            CellSize = MAP_CONTROL_SIZE / 10;

            Vehicles = vehicles;
            ReplayUser = Vehicles.First(v => v.AccountDBID == replay.datablock_1.playerID);
            ReplayUser.Recorder = true;

            FirstTeam = Vehicles.Where(v => v.TeamMate).ToList();
            SecondTeam = Vehicles.Where(v => !v.TeamMate).ToList();

            GameplayDescription gameplayDescription = map.Config.GameplayTypes[replay.datablock_1.gameplayID];

            Elements = GetMapImageElements(gameplayDescription, ReplayUser.Team);

            _parser = ReplayFileHelper.GetParser(_replay);
        }

        private List<MapImageElement> GetMapImageElements(GameplayDescription gameplayDescription, int replayUserTeam)
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
                    Point? mapCoord = MapGrid.game_to_map_coord(point);
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
                    Point? mapCoord = MapGrid.game_to_map_coord(point);
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
                Point? mapCoord = MapGrid.game_to_map_coord(gameplayDescription.ControlPoint.Value);
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

        public void Stop()
        {
            _parser.Abort();

            foreach (MapVehicle mapVehicle in Vehicles)
            {
                mapVehicle.Reset();
            }

            AlliesCapturePoints = 0;
            EnemiesCapturePoints = 0;
        }

        private void PacketHandler(Packet packet)
        {
            if (_updateSpeed > 0)
            {
                Thread.Sleep(_updateSpeed);
            }
            else
            {
                Sleep(0.0005);
            }

            dynamic data = packet.Data;

            if (packet.Type == PacketType.PlayerPos)
            {
                Point point = MapGrid.game_to_map_coord(data.position);

                MapVehicle member = Vehicles.FirstOrDefault(x => x.Id == data.PlayerId);

                if (member != null)
                {
                    member.X = point.X;
                    member.Y = point.Y;
                    member.Clock = packet.Clock;
                    member.Show();

                    if (member.Recorder)
                    {
                        var degrees = (double)data.hull_orientation[0] * (180 / Math.PI);
                        member.Orientation = degrees;
                    }
                }
            }

            if (packet.Type == PacketType.Health && packet.StreamSubType == 0x03)
            {
                MapVehicle member = Vehicles.FirstOrDefault(x => x.Id == (int)packet.PlayerId);
                if (member != null)
                {
                    member.CurrentHealth = data.health;
                }
            }

            if (packet.Type == PacketType.ArenaUpdate && data.updateType == 0x03)
            {
                if (data.period == 1)
                {
                    _periodLength = 60;
                }
                else
                {
                    _periodLength = data.period_length;
                }

                _clockAtPeriod = packet.Clock;
            }

            if (packet.Type == PacketType.ArenaUpdate && data.updateType == 0x06)
            {
                MapVehicle destroyedMember = Vehicles.FirstOrDefault(x => x.Id == data.destroyed);

                if (destroyedMember != null)
                {
                    destroyedMember.CurrentHealth = 0;

                    if (data.destroyer > 0)
                    {
                        MapVehicle destroyer = Vehicles.FirstOrDefault(x => x.Id == (int) data.destroyer);
                        if (destroyer != null) destroyer.Kills += 1;
                    }

                    if (destroyedMember.TeamMate)
                    {
                        SecondTeamKills += 1;
                    }
                    else
                    {
                        FirstTeamKills += 1;
                    }
                }
            }

            if (packet.Type == PacketType.ArenaUpdate && data.updateType == 0x08)
            {
                if (data.team != ReplayUser.Team)
                {
                    AlliesCapturePoints = data.points;
                }
                else
                {
                    EnemiesCapturePoints = data.points;
                }
            }

            if (packet.Type == PacketType.MinimapClick)
            {
                CellY = data.cellLeft * CellSize;
                CellX = data.cellTop * CellSize;
                Click = !Click;
            }

            if (_periodLength > 0 && !(packet.Type == PacketType.ArenaUpdate && data.updateType == 0x03))
            {
                var clockseconds = _periodLength - (packet.Clock - _clockAtPeriod);

                Time = TimeSpan.FromSeconds(clockseconds).ToString("mm\\:ss");
                Clock = packet.Clock;

                foreach (var vehicle in _vehicles)
                {
                    vehicle.Seen = Clock - vehicle.Clock < 10 && vehicle.CurrentHealth > 0;
                }
            }
        }

        private void Sleep(double milliSeconds)
        {
            var hiPerfTimer = new HiPerfTimer();
            double peekMs = 0;
            hiPerfTimer.Start();
            while (peekMs < milliSeconds)
            {
                Thread.Sleep(0);
                hiPerfTimer.Stop();
                peekMs = hiPerfTimer.Duration;
            }
        }

        public void Start()
        {
            using (MemoryStream memoryStream = new MemoryStream(_replay.Stream))
            {
                _parser.ReadReplayStream(memoryStream, PacketHandler);
            }
        }

        public void SetSpeed(int newspeed)
        {
            _updateSpeed = newspeed;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
