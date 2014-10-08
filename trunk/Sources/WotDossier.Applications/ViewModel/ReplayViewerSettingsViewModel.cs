using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Settings;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ReplayViewerSettingsViewModel))]
    public class ReplayViewerSettingsViewModel : ViewModel<IReplayViewerSettingsView>
    {
        private readonly AppSettings _appSettings;

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

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        public List<ListItem<Version>> Versions
        {
            get { return _versions; }
            set { _versions = value; }
        }

        public List<ReplayPlayer> ReplayPlayers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewerSettingsViewModel([Import(typeof(IReplayViewerSettingsView))]IReplayViewerSettingsView view)
            : base(view)
        {
            _appSettings = SettingsReader.Get();

            ReplayPlayers = new List<ReplayPlayer>{new ReplayPlayer()};
            
            view.Closing += ViewOnClosing;
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            SettingsReader.Save(_appSettings);
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }
    }
}
