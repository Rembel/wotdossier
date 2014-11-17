using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using WotDossier.Applications.Parser;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class ReplayViewer : INotifyPropertyChanged
    {
        public List<MapVehicle> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged("Vehicles");
            }
        }

        private readonly Domain.Replay.Replay _replay;
        private readonly MapGrid _mapGrid;
        private readonly Arena _arena;
        private bool _stopping;
        private int _updateSpeed;

        private string _time;
        private int _enemiesCapturePoints;
        private int _alliesCapturePoints;
        private List<MapVehicle> _vehicles;
        private int _periodLength = -1;
        private float _clockAtPeriod;
        private float _clock;

        public int EnemiesCapturePoints
        {
            get { return _enemiesCapturePoints; }
            set
            {
                _enemiesCapturePoints = value;
                OnPropertyChanged("EnemiesCapturePoints");
            }
        }

        public int AlliesCapturePoints
        {
            get { return _alliesCapturePoints; }
            set
            {
                _alliesCapturePoints = value;
                OnPropertyChanged("AlliesCapturePoints");
            }
        }

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
            _mapGrid = new MapGrid(new Rect(-500, -500, 1000, 1000), 500, 500);

            Vehicles = vehicles;
            ReplayUser = Vehicles.First(x => x.AccountDBID == replay.datablock_1.playerID);

            //    this.mapGrid = new MapGrid({
            //    container: options.container,
            //    ident: options.map.ident,
            //    map: {
            //        bounds: options.map.bounds,
            //        width: options.map.width,
            //        height: options.map.height,
            //    }
            //});
            //this.positions      = options.map.positions;
            //this.gameType       = options.gametype;
            //this.tracercount    = 1;
            //this.packet_url     = options.packets;
            //this.onError        = options.onError;
            //this.onLoaded       = options.onLoaded;
            //this.stopping       = false;
            //this.updateSpeed    = 100; // realtime?
            //this.container      = options.container;
            _arena = new Arena(_mapGrid);
            //this.handlers       = {
            //    loaded: [],
            //    error: [],
            //    progress: [],
            //    start: [],
            //    stop: [],
            //};

            //this.arena.setPlayerTeam(options.playerTeam);
            //this.initializeItems();
        }

        public MapVehicle ReplayUser { get; set; }


        //_handle: function(what, handler) {
        //    this.handlers[what].push(handler);
        //},
        //onStart: function(handler) {
        //    this._handle('start', handler);
        //},
        //onPacketsProgress: function(handler) {
        //    this._handle('progress', handler);
        //},
        //onPacketsError: function(handler) {
        //    this._handle('error', handler);
        //},
        //onPacketsLoaded: function(handler) {
        //    this._handle('loaded', handler);
        //},
        //onStop: function(handler) {
        //    this._handle('stop', handler);
        //},
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

        public MapGrid MapGrid
        {
            get { return _mapGrid; }
        }

        public Arena Arena
        {
            get { return _arena; }
        }

        public float Clock
        {
            get { return _clock; }
            set
            {
                _clock = value;
                OnPropertyChanged("Clock");
            }
        }

        public void Stop()
        {
            _stopping = true;
        }

        public void SetSpeed(int newspeed)
        {
            _updateSpeed = newspeed;
        }

        public void Replay()
        {
            var parser = ReplayFileHelper.GetParser(_replay);
            parser.ReadReplayStream(new MemoryStream(_replay.Stream), PacketHandler);
        }

        private void PacketHandler(Packet packet)
        {
            dynamic data = packet.Data;

            if (packet.Type == PacketType.PlayerPos)
            {
                Point point = MapGrid.game_to_map_coord(data.position);

                Thread.Sleep(1);

                MapVehicle member = Vehicles.FirstOrDefault(x => x.Id == data.PlayerId);

                if (member != null)
                {
                    member.X = point.X;
                    member.Y = point.Y;
                    member.Clock = packet.Clock;
                    member.Show();
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
                _periodLength = data.period_length;
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

            if (_periodLength > 0)
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

        public void Update(List<Packet> packets, double windowStart, double windowSize, int startIx)
        {
            var windowEnd = windowStart + windowSize;
            int ix;
            for (ix = startIx; ix < packets.Count; ix++)
            {
                var packet = packets[ix];
                //if (typeof (packet.clock) == 'undefined' && (typeof (packet.period) == 'undefined'))
                //    continue; // chat has clock, period doesnt(?)
                if (packet.Clock > windowEnd) break;
                _arena.update(packet);
            }

            if (this._stopping) ix = packets.Count;
            if (ix < packets.Count)
            {
                Thread.Sleep(this._updateSpeed);
                Update(packets, windowEnd, windowSize, ix);
            }
            else
            {
                //this.dispatch('stop');
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
