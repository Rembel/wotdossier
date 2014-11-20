using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using WotDossier.Applications.Parser;
using WotDossier.Dal;

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

        public ReplayViewer(Domain.Replay.Replay replay, List<MapVehicle> vehicles)
        {
            _replay = replay;

            var map = Dictionaries.Instance.Maps[replay.datablock_1.mapName];

            var values = map.BottomLeft.Replace(".", ",").Split(' ');

            var x = Convert.ToDouble(values[0]);
            var y = Convert.ToDouble(values[1]);

            values = map.UpperRight.Replace(".", ",").Split(' ');

            var x1 = Convert.ToDouble(values[0]);
            var y1 = Convert.ToDouble(values[1]);


            _mapGrid = new MapGrid(new Rect(x, y, x1 - x, y1 - y), MAP_CONTROL_SIZE, MAP_CONTROL_SIZE);

            CellSize = MAP_CONTROL_SIZE/10;

            Vehicles = vehicles;
            ReplayUser = Vehicles.First(v => v.AccountDBID == replay.datablock_1.playerID);
            ReplayUser.Recorder = true;

            _arena = new Arena(_mapGrid);
        }

        public MapVehicle ReplayUser { get; set; }
        
        private readonly MapGrid _mapGrid;
        public MapGrid MapGrid
        {
            get { return _mapGrid; }
        }

        private readonly Arena _arena;

        public Arena Arena
        {
            get { return _arena; }
        }

        private float _clock;

        private bool _click;

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

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void InitializeItems()
        {
            //var bv = this;
            //this.mapGrid.addItem('clock', $('<div/>').attr('id', 'battleviewer-clock').html('--:--'));
            //if(this.gameType == "ctf") {
            //    for(i = 0; i < this.positions.base.length; i++) {
            //        this.positions.base[i].forEach(function(basedata) {
            //            var isEnemy = bv.getArena().isEnemyTeam(i + 1);
            //            var base = new BasePoint();
            //            if(isEnemy) {
            //                base.setEnemy();
            //            } else {
            //                base.setFriendly();
            //            }
            //            base.setPosition(bv.getArena().convertArrayPosition(basedata));
            //            bv.mapGrid.addItem('base-' + i, base.render().el);
            //            // we want the arena to get a copy too
            //            bv.getArena().addBasePoint(i, 0, base); // 0 because it's 0-based >_<
            //        });
            //    }
            //    for(i = 0; i < this.positions.team.length; i++) {
            //        if(this.positions.team[i] != null) {
            //            for(j = 0; j < this.positions.team[i].length; j++) {
            //                var spawn = new SpawnPoint();
            //                spawn.setPoint(j + 1);
            //                if(bv.getArena().isEnemyTeam(i + 1)) {
            //                    spawn.setEnemy();
            //                } else {
            //                    spawn.setFriendly();
            //                }
            //                spawn.setPosition(bv.getArena().convertArrayPosition(this.positions.team[i][j]));
            //                bv.getMapGrid().addItem('spawn-' + i + '-' + j, spawn.render().el);
            //            }
            //        }
            //    }
            //} else if(this.gameType == 'assault') {
            //    // depending on who's owning the base...
            //    var control = new BasePoint();
            //    if(this.positions.base[0] == null) {
            //        control.setPosition(bv.getArena().convertArrayPosition(this.positions.base[1][0]));
            //        if(this.getArena().isEnemyTeam(2)) {
            //            control.setEnemy();
            //        } else {
            //            control.setFriendly();
            //        }
            //    } else {
            //        control.setPosition(bv.getArena().convertArrayPosition(this.positions.base[0][0]));
            //        if(this.getArena().isEnemyTeam(1)) {
            //            control.setEnemy();
            //        } else {
            //            control.setFriendly();
            //        }
            //    }

            //    bv.getMapGrid().addItem('base-control', control.render().el);

            //    // now iterate over the spawn points for both teams
            //    for(i = 0; i < this.positions.team.length; i++) {
            //        if(this.positions.team[i] != null) {
            //            for(j = 0; j < this.positions.team[i].length; j++) {
            //                var spawn = new SpawnPoint();
            //                spawn.setPoint(j + 1);
            //                if(bv.getArena().isEnemyTeam(i + 1)) {
            //                    spawn.setEnemy();
            //                } else {
            //                    spawn.setFriendly();
            //                }
            //                spawn.setPosition(bv.getArena().convertArrayPosition(this.positions.team[i][j]));
            //                bv.mapGrid.addItem('spawn-' + i + '-' + j, spawn.render().el);
            //            }
            //        }
            //    }
            //} else if(this.gameType == 'domination') {
            //    // set the control point
            //    var control = new BasePoint();
            //    control.setNeutral();
            //    control.setPosition(bv.getArena().convertArrayPosition(this.positions.control[0]));
            //    bv.mapGrid.addItem('base-control', control.render().el);

            //    // add the team spawns
            //    for(i = 0; i < this.positions.team.length; i++) {
            //        for(j = 0; j < this.positions.team[i].length; j++) {
            //            var isEnemy = bv.getArena().isEnemyTeam(i + 1);
            //            var spawn = new SpawnPoint();
            //            spawn.setPoint(j + 1);
            //            if(isEnemy) {
            //                spawn.setEnemy();
            //            } else {
            //                spawn.setFriendly();
            //            }
            //            spawn.setPosition(bv.getArena().convertArrayPosition(this.positions.team[i][j]));
            //            bv.mapGrid.addItem('spawn-' + i + '-' + j, spawn.render().el);
            //        }
            //    }
            //}
        }

        private void PacketHandler(Packet packet)
        {
            Thread.Sleep(_updateSpeed);

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
                MapVehicle member = Vehicles.FirstOrDefault(x => x.Id == (int)packet.PlayerId);
                if (member != null && data.destroyed == 1)
                {
                    member.CurrentHealth = 0;
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

        public void Replay()
        {
            var parser = ReplayFileHelper.GetParser(_replay);
            parser.ReadReplayStream(new MemoryStream(_replay.Stream), PacketHandler);
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
