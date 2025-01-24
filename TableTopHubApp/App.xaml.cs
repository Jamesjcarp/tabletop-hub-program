// <copyright file="App.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Configuration;
    using System.Data;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private static MusicScreen musicTab;
        private static BattleMapScreen battleTab;
        private static OverlayScreen overlayTab;

        /// <summary>
        /// Gets or sets the music tab.
        /// </summary>
        public static MusicScreen MusicTab
        {
            get => musicTab;
            set => musicTab = value;
        }

        /// <summary>
        /// Gets or sets the battle tab.
        /// </summary>
        public static BattleMapScreen BattleTab
        {
            get => battleTab;
            set => battleTab = value;
        }

        /// <summary>
        /// Gets or sets the ovewrlay tab.
        /// </summary>
        public static OverlayScreen OverlayTab
        {
            get => overlayTab;
            set => overlayTab = value;
        }

        /// <summary>
        /// Ran when the application is first run.
        /// </summary>
        /// <param name="e">Arguements for when program opens.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Thread musicThread = new Thread(this.StartMusicWindow);
            musicThread.SetApartmentState(ApartmentState.STA);
            musicThread.Start();

            Thread battleThread = new Thread(this.StartBattleWindow);
            battleThread.SetApartmentState(ApartmentState.STA);
            battleThread.Start();

            //Thread overlayThread = new Thread(this.StartOverlayWindow);
            //overlayThread.SetApartmentState(ApartmentState.STA);
            //overlayThread.Start();
        }

        private void StartMusicWindow()
        {
            musicTab = new MusicScreen();
            musicTab.ShowDialog();
        }

        private void StartBattleWindow()
        {
            battleTab = new BattleMapScreen();
            battleTab.ShowDialog();
        }

        private void StartOverlayWindow()
        {
            overlayTab = new OverlayScreen();
            overlayTab.ShowDialog();
        }
    }
}
