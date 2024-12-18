using System.Diagnostics;

namespace DnD_music_program
{
    internal class UI
    {
        private Button playMusicButton = new Button()
        {
            Text = "Play",
            Size = new System.Drawing.Size(100, 50),
            Location = new System.Drawing.Point(50, 50),
        };

        private Button stopButton = new Button()
        {
            Text = "Stop",
            Size = new System.Drawing.Size(100, 50),
            Location = new System.Drawing.Point(150, 50),
        };

        private TrackBar volumeMusicSlider = new TrackBar()
        {
            Size = new System.Drawing.Size(200, 50),
            Location = new System.Drawing.Point(50, 100),
            Maximum = 100,
            Value = 50,
        };

        private ComboBox trackOptions = new ComboBox()
        {
            Size = new System.Drawing.Size(200, 50),
            Location = new System.Drawing.Point(300, 50),
        };

        private ComboBox soundEffectOptions = new ComboBox()
        {
            Size = new System.Drawing.Size(200, 50),
            Location = new System.Drawing.Point(300, 200),
        };

        private Button playSoundEffectButton = new Button()
        {
            Text = "Play",
            Size = new System.Drawing.Size(100, 50),
            Location = new System.Drawing.Point(50, 200),
        };

        private TrackBar volumeSoundEffectSlider = new TrackBar()
        {
            Size = new System.Drawing.Size(200, 50),
            Location = new System.Drawing.Point(50, 250),
            Maximum = 100,
            Value = 50,
        };

        public delegate void PlayEventHandler(string title);

        public static event PlayEventHandler? PlayTrack;

        public static event PlayEventHandler? PlaySound;

        public delegate void VolumeEventHandler(int volume);

        public static event VolumeEventHandler? VolumeMusicChanged;

        public static event VolumeEventHandler? VolumeSoundEffectChanged;

        public delegate void StopEventHandler();

        public static event StopEventHandler? Stop;

        private string? selectedTrack = new string(string.Empty);

        private string? selectedSoundEffect = new string(string.Empty);

        public UI()
        {

            string[] musicOptions = AudioManager.GetTracks().Keys.ToArray();

            for (int i = 0; i < musicOptions.Length; i++)
            {
                trackOptions.Items.Add(musicOptions[i]);
            }

            if(musicOptions.Length > 0)
            {
                trackOptions.SelectedIndex = 0;
                selectedTrack = musicOptions[0];
            }

            string[] soundOptions = AudioManager.GetSoundEffects().Keys.ToArray();

            for (int i = 0; i < soundOptions.Length; i++)
            {
                soundEffectOptions.Items.Add(soundOptions[i]);
            }

            if(soundOptions.Length > 0)
            {
                soundEffectOptions.SelectedIndex = 0;
                selectedSoundEffect = soundOptions[0];
            }



            playMusicButton.Click += OnPressPlayMusic;
            playSoundEffectButton.Click += OnPressPlaySoundEffect;
            stopButton.Click += OnPressStop;
            trackOptions.SelectedIndexChanged += OnTrackSelectionChanged;
            volumeMusicSlider.Scroll += OnMusicVolumeChanged;
            soundEffectOptions.SelectedIndexChanged += OnSoundEffectSelectionChanged;
            volumeSoundEffectSlider.Scroll += OnSoundeffectVolumeChanged;

            Program.mainForm.Controls.Add(playMusicButton);
            Program.mainForm.Controls.Add(stopButton);
            Program.mainForm.Controls.Add(trackOptions);
            Program.mainForm.Controls.Add(volumeMusicSlider);
            Program.mainForm.Controls.Add(playSoundEffectButton);
            Program.mainForm.Controls.Add(soundEffectOptions);
            Program.mainForm.Controls.Add(volumeSoundEffectSlider);

            Program.mainForm.KeyPreview = true;
            Program.mainForm.KeyDown += OnKeyPressed;

            Program.mainForm.FormClosed += OnFormClose;
        }

        private void OnFormClose(object? sender, EventArgs e)
        {
            if(Program.overlayForm != null && Program.overlayForm.IsDisposed == false)
            {
                Program.overlayForm.Invoke((MethodInvoker)(() => Program.overlayForm.Close()));
            }
        }

        private void OnPressPlayMusic(object? sender, EventArgs e)
        {
            PlayTrack?.Invoke(selectedTrack);
        }

        private void OnPressPlaySoundEffect(object? sender, EventArgs e)
        {
            PlaySound?.Invoke(selectedSoundEffect);
        }

        private void OnPressStop(object? sender, EventArgs e)
        {
            Stop?.Invoke();
        }

        private void OnTrackSelectionChanged(object? sender, EventArgs e)
        {
            selectedTrack = trackOptions.SelectedItem.ToString();
        }

        private void OnMusicVolumeChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine("changed to: " + volumeMusicSlider.Value.ToString());
            VolumeMusicChanged?.Invoke(volumeMusicSlider.Value);
        }

        private void OnSoundEffectSelectionChanged(object? sender, EventArgs e)
        {
            selectedSoundEffect = soundEffectOptions.SelectedItem.ToString();
        }

        private void OnSoundeffectVolumeChanged(object? sender, EventArgs e)
        {
            VolumeSoundEffectChanged?.Invoke(volumeSoundEffectSlider.Value);
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            
            if(e.KeyCode == Keys.M)
            {
                Task.Run(() => Program.oUI.Mumbo("mumbo.png"));
            }
            else if(e.KeyCode == Keys.V)
            {
                Debug.WriteLine("v");
                Task.Run(() => Program.oUI.Video());
            }
            else if(e.KeyCode == Keys.N)
            {
                Task.Run(() => Program.oUI.Mumbo("jessie.jpg"));
            }
        }
    }
}
