using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using WotDossier.Applications.Parser;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class ReplayViewer
    {
        private readonly MapGrid _mapGrid;
        private string gameType;
        private readonly Arena _arena;
        private bool _stopping;
        private int _updateSpeed;
        private List<Packet> packets;

        public ReplayViewer()
        {
            _mapGrid = new MapGrid(new Rect(-500, -500, 1000, 1000), 500, 500);

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
            this._arena = new Arena(_mapGrid);
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
        public void initializeItems()
        {
            var bv = this;
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

        //start: function() {
        //var bv = this;
        //this._mapGrid.render();
        //$.ajax({
        //    url: this.packet_url,
        //    type: 'GET',
        //    dataType: 'json',
        //    crossDomain: true,
        //    timeout: 60000,
        //    success: function(d, t, x) {
        //        bv.packets = d;
        //        bv.dispatch('loaded'); 
        //    },
        //    error: function(j, t, e) {
        //        bv.dispatch('error', { error: t + ", " + e });
        //    },
        //    xhr: function() {
        //        var xhr = jQuery.ajaxSettings.xhr();
        //        xhr.addEventListener('progress', function(evt) {
        //            var percent = 0;
        //            if (evt.lengthComputable) {
        //                percent = Math.ceil(100/(evt.total / evt.loaded));
        //            }
        //            bv.dispatch('progress', percent);
        //        })
        //        return xhr;
        //    }
        //});
        //this.dispatch('start');
        //},
        public void stop()
        {
            this._stopping = true;
        }

        public void setSpeed(int newspeed)
        {
            this._updateSpeed = newspeed;
        }

        //dispatch: function(type, e) {
        //    if(!this.handlers[type]) this.handlers[type] = [];
        //    this.handlers[type].forEach(function(handler) {
        //        handler.bind(this)(e);
        //    });
        //},
        public void updateClock()
        {
            var clockHtml = "--:--";
            if (this.Arena.period_length > 0)
            {
                var clockseconds = this.Arena.period_length - (this.Arena.clock - this.Arena.clock_at_period);
                var minutes = Math.Floor(clockseconds/60.0);
                var seconds = Math.Floor(clockseconds - minutes*60);
                seconds = (seconds < 10 ? '0' + seconds : seconds);
                minutes = (minutes < 10 ? '0' + minutes : minutes);
                clockHtml = minutes + ":" + seconds;
            }
            //this._mapGrid.getItem('clock').html(clockHtml);
        }

        public void replay()
        {
            var me = this;
            update(this.packets, 0, 0.1, 0);
        }

        public void update(List<Packet> packets, double window_start, double window_size, int start_ix)
        {
            var window_end = window_start + window_size;
            int ix;
            for (ix = start_ix; ix < packets.Count; ix++)
            {
                var packet = packets[ix];
                //if (typeof (packet.clock) == 'undefined' && (typeof (packet.period) == 'undefined'))
                //    continue; // chat has clock, period doesnt(?)
                if (packet.Clock > window_end) break;
                _arena.update(packet);
            }

            this.updateClock(); // should really, really be part of something else

            if (this._stopping) ix = packets.Count;
            if (ix < packets.Count)
            {
                Thread.Sleep(this._updateSpeed);
                update(packets, window_end, window_size, ix);
            }
            else
            {
                //this.dispatch('stop');
            }
        }
    }
}
