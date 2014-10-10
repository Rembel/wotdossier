using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Settings;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ReplayViewerSettingsViewModel))]
    public class ReplayViewerSettingsViewModel : ViewModel<IReplayViewerSettingsView>
    {
        private List<ListItem<Version>> _versions = new List<ListItem<Version>>
            {
                new ListItem<Version>(Dictionaries.VersionAll, Resources.Resources.TankFilterPanel_Default), 
                new ListItem<Version>(Dictionaries.VersionRelease, "0.9.3"),
                new ListItem<Version>(new Version("0.9.2.0"), "0.9.2"),
                new ListItem<Version>(new Version("0.9.1.0"), "0.9.1"),
                new ListItem<Version>(new Version("0.9.0.0"), "0.9.0"),
                new ListItem<Version>(new Version("0.8.11.0"), "0.8.11"), 
                new ListItem<Version>(new Version("0.8.10.0"), "0.8.10"), 
                new ListItem<Version>(new Version("0.8.9.0"), "0.8.9"), 
                new ListItem<Version>(new Version("0.8.8.0"), "0.8.8"), 
                new ListItem<Version>(new Version("0.8.7.0"), "0.8.7"), 
                new ListItem<Version>(new Version("0.8.6.0"), "0.8.6"), 
                new ListItem<Version>(new Version("0.8.5.0"), "0.8.5"), 
                new ListItem<Version>(new Version("0.8.4.0"), "0.8.4"), 
                new ListItem<Version>(new Version("0.8.3.0"), "0.8.3"), 
                new ListItem<Version>(new Version("0.8.2.0"), "0.8.2"), 
                new ListItem<Version>(new Version("0.8.1.0"), "0.8.1"), 
                new ListItem<Version>(Dictionaries.VersionTest, "Test 0.x.x"), 
            };

        private ObservableCollection<ReplayPlayer> _replayPlayers = new ObservableCollection<ReplayPlayer>();

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        public List<ListItem<Version>> Versions
        {
            get { return _versions; }
            set { _versions = value; }
        }

        public ObservableCollection<ReplayPlayer> ReplayPlayers
        {
            get { return _replayPlayers; }
            set { _replayPlayers = value; }
        }

        public DelegateCommand<ReplayPlayer> SetPathCommand { get; set; }
        public DelegateCommand<ReplayPlayer> DeleteCommand { get; set; }
        public DelegateCommand<ReplayPlayer> PlayWithSelectedCommand { get; set; }
        public DelegateCommand PlayWithAutoSelectCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public ReplayPlayer Player { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewerSettingsViewModel([Import(typeof(IReplayViewerSettingsView))]IReplayViewerSettingsView view)
            : base(view)
        {
            AppSettings appSettings = SettingsReader.Get();

            ReplayPlayers = new ObservableCollection<ReplayPlayer>(appSettings.ReplayPlayers);

            SetPathCommand = new DelegateCommand<ReplayPlayer>(OnSetPathCommand);
            DeleteCommand = new DelegateCommand<ReplayPlayer>(OnDeleteCommand);
            PlayWithSelectedCommand = new DelegateCommand<ReplayPlayer>(OnPlayWithSelectedCommand);
            PlayWithAutoSelectCommand = new DelegateCommand(OnPlayWithAutoSelectCommand);

            AddCommand = new DelegateCommand(OnAdd);
            
            view.Closing += ViewOnClosing;
        }

        private void OnPlayWithAutoSelectCommand()
        {
            Player = null;
            ViewTyped.Close();
        }

        private void OnPlayWithSelectedCommand(ReplayPlayer replayPlayer)
        {
            Player = replayPlayer;
            ViewTyped.Close();
        }

        private void OnDeleteCommand(ReplayPlayer replayPlayer)
        {
            ReplayPlayers.Remove(replayPlayer);
        }

        private void OnAdd()
        {
            ReplayPlayers.Add(new ReplayPlayer());
        }

        private void OnSetPathCommand(ReplayPlayer replayPlayer)
        {
            if (replayPlayer != null)
            {
                VistaOpenFileDialog dialog = new VistaOpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = ".exe"; // Default file extension
                dialog.Filter = "WorldOfTanks (.exe)|*.exe"; // Filter files by extension 
                dialog.Multiselect = false;
                dialog.Title = Resources.Resources.WindowCaption_SelectPathToWorldOfTanksExecutable;
                bool? showDialog = dialog.ShowDialog();
                if (showDialog == true)
                {
                    replayPlayer.Path = dialog.FileName;
                }
            }
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            AppSettings appSettings = SettingsReader.Get();
            appSettings.ReplayPlayers = ReplayPlayers.ToList();
            SettingsReader.Save(appSettings);
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }
    }
}