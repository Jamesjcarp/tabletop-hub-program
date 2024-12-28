// <copyright file="MusicScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// The music screen contains UI elements to interact with the other two screens of the program
    /// it contains options for music, overlay and battlemap.
    /// </summary>
    public partial class MusicScreen : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MusicScreen"/> class.
        /// </summary>
        public MusicScreen()
        {
            this.InitializeComponent();

            AudioManager.InitTracks();
            OverlayManager.InitAssets();
            AudioPlayer audioPlayer = new AudioPlayer();

            this.musicOptions.ItemsSource = AudioManager.GetTrackTitles();
            this.musicOptions.SelectedIndex = 0;

            this.soundOptions.ItemsSource = AudioManager.GetSoundTitles();
            this.soundOptions.SelectedIndex = 0;

            this.overlayOptions.ItemsSource = OverlayManager.GetOverlayTitles();
            this.overlayOptions.SelectedIndex = 0;
        }

        private void PlayMusicClick(object sender, RoutedEventArgs e)
        {
            if (this.musicOptions.SelectedValue.ToString() != null)
            {
                AudioPlayer.PrepMusicWorker(this.musicOptions.SelectedValue.ToString());
            }
        }

        private void StopMusicClick(object sender, RoutedEventArgs e)
        {
            AudioPlayer.StopTrack();
        }

        private void PlaySoundClick(object sender, RoutedEventArgs e)
        {
            if (this.soundOptions.SelectedValue != null)
            {
                AudioPlayer.PrepSoundEffectWorker(this.soundOptions.SelectedValue.ToString() !);
            }
        }

        private void StopSoundClick(object sender, RoutedEventArgs e)
        {
            // todo
        }

        private void EnableOverlayClick(object sender, RoutedEventArgs e)
        {
            if (this.overlayOptions.SelectedValue != null)
            {
                App.OverlayTab.EnableOverlayElement(this.overlayOptions.SelectedValue.ToString() !);
            }
        }

        private void DisableOverlayClick(object sender, RoutedEventArgs e)
        {
            App.OverlayTab.DisableOverlayElement();
        }

        private void EditOverlayClick(object sender, RoutedEventArgs e)
        {
            App.OverlayTab.ChangeWindowState();
        }

        /// <summary>
        /// Event raised when the window is closed. makes sure to end playback of music. in the future close the other windows when this happens.
        /// </summary>
        private void MusicScreenClosing(object sender, CancelEventArgs e)
        {
            AudioPlayer.StopTrack();
        }

        private void MusicVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.ChangeMusicVolume((int)this.musicVolume.Value);
        }

        private void SoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.ChangeSoundEffectVolume((int)this.soundVolume.Value);
        }
    }
}
