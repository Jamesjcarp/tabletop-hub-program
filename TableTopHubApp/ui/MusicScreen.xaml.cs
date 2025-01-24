// <copyright file="MusicScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The music screen contains UI elements to interact with the other two screens of the program
    /// it contains options for music, overlay and battlemap.
    /// </summary>
    public partial class MusicScreen : Window
    {
        private Grid activeGrid = new Grid();
        private Grid activeSubGrid = new Grid();
        private Grid activeSubSubGrid = new Grid();

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

            this.iconOptions.ItemsSource = MapManager.GetIconTitles();
            this.iconOptions.SelectedIndex = 0;

            this.mapOptions.ItemsSource = MapManager.GetMapTitles();
            this.mapOptions.SelectedIndex = 0;

            this.activeGrid = this.addContentGrid;
            this.activeSubGrid = this.addContentSubGrid;
        }

        private void ScreenChangeClick(object sender, RoutedEventArgs e)
        {
            this.ContentClearAll();

            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button)) 
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            this.activeGrid.IsEnabled = false;
            this.activeGrid.Visibility = Visibility.Hidden;

            if (sourceButton.Name == "mainScreenButton")
            {
                this.mainGrid.IsEnabled = true;
                this.mainGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.mainGrid;
            }
            else if(sourceButton.Name == "addContentScreenButton")
            {
                this.addContentGrid.IsEnabled = true;
                this.addContentGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.addContentGrid;
            }
            else if(sourceButton.Name == "statsScreenButton")
            {
                this.statsGrid.IsEnabled = true;
                this.statsGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.statsGrid;
            }
            else
            {
                throw new Exception("Specific button not found");
            }
        }

        private void AddContentSubScreenChange(object sender, RoutedEventArgs e)
        {
            this.ContentClearAll();

            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button))
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            this.activeSubGrid.IsEnabled = false;
            this.activeSubGrid.Visibility = Visibility.Hidden;

            if(sourceButton.Name == "addContentButton")
            {
                this.addContentSubGrid.IsEnabled = true;
                this.addContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.addContentSubGrid;
            }
            else if(sourceButton.Name == "editContentButton")
            {
                this.editContentSubGrid.IsEnabled = true;
                this.editContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.editContentSubGrid;
            }
            else if(sourceButton.Name == "removeContentButton")
            {
                this.removeContentSubGrid.IsEnabled = true;
                this.removeContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.removeContentSubGrid;
            }

        }

        private void AddContentTypeDropdownChanged(object sender, RoutedEventArgs e)
        {
            string selected = this.addContentTypeDropdown.SelectedValue.ToString();

            this.activeSubSubGrid.IsEnabled = false;
            this.activeSubSubGrid.Visibility = Visibility.Hidden;

            this.ContentClearAll();


            if (selected == "System.Windows.Controls.ComboBoxItem: music")
            {
                this.addContentMusicGrid.IsEnabled = true;
                this.addContentMusicGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentMusicGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: sound")
            {
                this.addContentSoundGrid.IsEnabled = true;
                this.addContentSoundGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentSoundGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: overlay")
            {
                this.addContentOverlayGrid.IsEnabled = true;
                this.addContentOverlayGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentOverlayGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: icon")
            {
                this.addContentIconGrid.IsEnabled = true;
                this.addContentIconGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentIconGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: map")
            {
                this.addContentMapGrid.IsEnabled = true;
                this.addContentMapGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentMapGrid;
            }
            else if(selected == null)
            {
                // do nothing
            }
            else
            {
                throw new Exception("type not implemented");
            }
        }

        private void ContentClearAll()
        {
            FileManager.ClearBuffers();

            // add section
            this.addContentMusicIntroOpenFileFeedback.Text = string.Empty;
            this.addContentMusicOpenFileFeedback.Text = string.Empty;
            this.addContentMusicNameBox.Text = string.Empty;
            this.addContentMusicConfirmFeedback.Text = string.Empty;

            this.addContentSoundOpenFileFeedback.Text = string.Empty;
            this.addContentSoundNameBox.Text = string.Empty;
            this.addContentSoundConfirmFeedback.Text = string.Empty;

            this.addContentIconOpenFileFeedback.Text = string.Empty;
            this.addContentIconNameBox.Text = string.Empty;
            this.AddContentIconWidthBox.Text = string.Empty;
            this.AddContentIconHeightBox.Text = string.Empty;
            this.addContentIconConfirmFeedback.Text = string.Empty;
            // edit section

            // remove section
        }

        private void AddContentOpenFileClick(object sender, RoutedEventArgs e)
        {
            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button))
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            bool result = false;

            if(sourceButton.Name == "addContentMusicOpenFileButton")
            {
                result = FileManager.OpenFile("music");
                if (result == true)
                {
                    this.addContentMusicOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentMusicOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if (sourceButton.Name == "addContentMusicIntroOpenFileButton")
            {
                result = FileManager.OpenIntroFile();
                if (result == true)
                {
                    this.addContentMusicIntroOpenFileFeedback.Text = "Current file: " + FileManager.CurrentIntroPath;
                }
                else
                {
                    this.addContentMusicIntroOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if(sourceButton.Name == "addContentSoundOpenFileButton")
            {
                result = FileManager.OpenFile("sound");
                if (result == true)
                {
                    this.addContentSoundOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentSoundOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if(sourceButton.Name == "addContentOverlayOpenFileButton")
            {
                result = FileManager.OpenFile("overlay");
            }
            else if(sourceButton.Name == "addContentMapOpenFileButton")
            {
                result = FileManager.OpenFile("map");
            }
            else if(sourceButton.Name == "addContentIconOpenFileButton")
            {
                result = FileManager.OpenFile("icon");
                if (result == true)
                {
                    this.addContentIconOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentIconOpenFileFeedback.Text = "Error: could not open file";
                }
            }
            else
            {
                throw new Exception("unimplemented resource type");
            }
        }

        private void AddContentMusicConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[3];

            if (this.addContentMusicNameBox.Text.Length > 0)
            {
                data[0] = this.addContentMusicNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentMusicConfirmFeedback.Text = "Error: no track name provided";
                return;
            }

            if(FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentMusicConfirmFeedback.Text = "Error: no loop file selected";
                return;
            }

            if(FileManager.CurrentIntroPath != string.Empty)
            {
                data[2] = Path.GetFileName(FileManager.CurrentIntroPath);
            }
            else
            {
                data[2] = "NULL";
            }

            if (FileManager.CopyFile("music") == true && (data[2] == "NULL" || FileManager.CopyFile("intro") == true))
            {
                FileManager.AddData("music", data);
                this.ContentClearAll();
                this.addContentMusicConfirmFeedback.Text = "Successfully added!";
            }
            else
            {
                this.addContentMusicConfirmFeedback.Text = "Error: file duplicate detected";
                return;
            }
        }

        private void AddContentSoundConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];

            if(this.addContentSoundNameBox.Text.Length > 0)
            {
                data[0] = this.addContentSoundNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: no sound name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            if(FileManager.CopyFile("sound") == true)
            {
                FileManager.AddData("sound", data);
                this.ContentClearAll();
                this.addContentSoundConfirmFeedback.Text = "Successfully added!";
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: file duplicate detected";
                return;
            }
        }

        private void AddContentIconConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[5];

            if (this.addContentIconNameBox.Text.Length > 0)
            {
                data[0] = this.addContentIconNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            if(Path.GetExtension(FileManager.CurrentFilePath) == ".gif")
            {
                data[2] = "ANIMATED";
            }
            else
            {
                data[2] = "STATIC";
            }

            int width = 1;
            if (this.AddContentIconWidthBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentIconWidthBox.Text, out width))
                {
                    data[3] = System.Text.RegularExpressions.Regex.Replace(this.AddContentIconWidthBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.addContentIconConfirmFeedback.Text = "Error: please use only numbers in your width, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no width provided";
                return;
            }

            int height = 1;
            if (this.AddContentIconHeightBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentIconHeightBox.Text, out height))
                {
                    data[4] = System.Text.RegularExpressions.Regex.Replace(this.AddContentIconHeightBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.addContentIconConfirmFeedback.Text = "Error: please use only numbers in your height, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no height provided";
                return;
            }

            if (FileManager.CopyFile("icon") == true)
            {
                FileManager.AddData("icon", data);
                this.ContentClearAll();
                this.addContentIconConfirmFeedback.Text = "Successfully added!";
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: file duplicate detected";
                return;
            }
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

        private void AddIconClick(object sender, RoutedEventArgs e)
        {
            App.BattleTab.AddCreature(this.iconOptions.SelectedValue.ToString()!);
        }

        private void SelectMapClick(object sender, RoutedEventArgs e)
        {
            App.BattleTab.OpenMap(this.mapOptions.SelectedValue.ToString()!);
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
