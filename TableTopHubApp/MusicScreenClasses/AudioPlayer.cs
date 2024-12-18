﻿// <copyright file="AudioPlayer.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Diagnostics;
    using SFML.Audio;

    /// <summary>
    /// Handles the threading to play and stop music and sound effects.
    /// </summary>
    internal class AudioPlayer
    {
        private static readonly object MusicLock = new object();
        private static SoundBuffer? introSong;
        private static SoundBuffer? loopSong;
        private static Sound music = new Sound();
        private static Sound soundEffect = new Sound();
        private static CancellationTokenSource? cancelTok;
        private static float musicVolume = 50;
        private static float soundeffectVolume = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayer"/> class.
        /// Class constructor inits volume to be the same as the default for the UI.
        /// </summary>
        public AudioPlayer()
        {
            // default volume.
            music.Volume = musicVolume;

            // set up cancellation token.
            cancelTok = new CancellationTokenSource();
        }

        /// <summary>
        /// Event to adjust the music volume.
        /// </summary>
        /// <param name="volume">The new volume from 0-100.</param>
        public static void ChangeMusicVolume(int volume)
        {
            musicVolume = volume;
            music.Volume = musicVolume;
        }

        /// <summary>
        /// Sets the volume of the sound effect.
        /// </summary>
        /// <param name="volume">range 0-100 volume input.</param>
        public static void ChangeSoundEffectVolume(int volume)
        {
            soundeffectVolume = volume;
            soundEffect.Volume = soundeffectVolume;
        }

        /// <summary>
        /// Event to end play on a track early.
        /// </summary>
        public static void StopTrack()
        {
            cancelTok?.Cancel();
            cancelTok = new CancellationTokenSource();
        }

        /// <summary>
        /// Cancels the previous thread and starts a new one to play music.
        /// </summary>
        /// <param name="title">The string key for the song.</param>
        public static void PrepMusicWorker(string? title)
        {
            if (title == null)
            {
                return;
            }

            // these two ensure that the old thread is sent a cancellation request and then the new thread has a token created for it
            // send cancel signal to current cancellation token
            cancelTok?.Cancel();

            // create a new cancellation token
            cancelTok = new CancellationTokenSource();

            // slight delay so that cancellation occurs
            if (music.Status == SoundStatus.Playing)
            {
                Task.Delay(800).Wait();
            }

            // run the PlayTrack function in a new thread
            Task.Run(() => PlayTrack(title, cancelTok.Token));
        }

        /// <summary>
        /// begin playback on a sound effect.
        /// </summary>
        /// <param name="title">title of the sound.</param>
        public static void PrepSoundEffectWorker(string title)
        {
            Task.Run(() => PlaySoundEffect(title));
        }

        /// <summary>
        /// The PlayTrack function retrieves the file data asociated with a track and passes to the next function.
        /// </summary>
        /// <param name="title">String key for a specific track.</param>
        /// <param name="cancelTok">Cancellation token relevant to this specific instance.</param>
        private static void PlayTrack(string title, CancellationToken cancelTok)
        {
            // extracts the file path for the loop and intro
            string[] songPaths = AudioManager.GetTrackPath(title);

            // songs with no intro are represented by the intro path being "NULL"
            if (songPaths[1] == "NULL")
            {
                loopSong = new SoundBuffer(songPaths[0]);

                PlayTrackWithoutIntro(loopSong, cancelTok);
            }
            else
            {
                loopSong = new SoundBuffer(songPaths[0]);
                introSong = new SoundBuffer(songPaths[1]);

                PlayTrackWithIntro(introSong, loopSong, cancelTok);
            }
        }

        /// <summary>
        /// In the case where a track has no intro. just loop the track.
        /// </summary>
        /// <param name="loop">Sound data for the loop.</param>
        /// <param name="cancelTok">Cancellation token passed foreward.</param>
        private static void PlayTrackWithoutIntro(SoundBuffer loop, CancellationToken cancelTok)
        {
            // lock the music variable when changing to avoid race conditions
            lock (MusicLock)
            {
                FadeOut(music);

                music = new Sound(loop)
                {
                    Loop = true,
                };
                music.Volume = 0;

                music.Play();

                // fade in the music
                for (int i = 0; i <= musicVolume; i++)
                {
                    music.Volume = i;
                    Task.Delay(15).Wait();
                }
            }

            // while the music is playing check to see if the cancellation token has been flagged
            while (music.Status == SoundStatus.Playing)
            {
                if (cancelTok.IsCancellationRequested == true)
                {
                    lock (MusicLock)
                    {
                        FadeOut(music);
                    }

                    return;
                }

                Task.Delay(200).Wait();
            }
        }

        /// <summary>
        /// Same logic as the PlayTrackWithoutIntro function however includes a begining section for the intro.
        /// </summary>
        /// <param name="intro">Sound data for the intro.</param>
        /// <param name="loop">Sound data for the loop.</param>
        /// <param name="cancelTok">Cancellation token specific to this thread.</param>
        private static void PlayTrackWithIntro(SoundBuffer intro, SoundBuffer loop, CancellationToken cancelTok)
        {
            // lock the music variable to avoid race conditions
            lock (MusicLock)
            {
                FadeOut(music);

                music = new Sound(intro);

                music.Volume = 0;

                music.Play();

                for (int i = 0; i <= musicVolume; i++)
                {
                    music.Volume = i;
                    Task.Delay(15).Wait();
                    Debug.WriteLine("volume:" + i.ToString());
                }
            }

            // while the intro is playing check to see if cancellation token has been flagged
            while (music.Status == SoundStatus.Playing)
            {
                if (cancelTok.IsCancellationRequested == true)
                {
                    FadeOut(music);
                    return;
                }
            }

            // repeat process above for loop track and loop it instead of running it once
            if (cancelTok.IsCancellationRequested == false)
            {
                lock (MusicLock)
                {
                    music = new Sound(loop)
                    {
                        Loop = true,
                    };

                    music.Volume = musicVolume;

                    music.Play();
                }

                while (music.Status == SoundStatus.Playing)
                {
                    if (cancelTok.IsCancellationRequested == true)
                    {
                        lock (MusicLock)
                        {
                            FadeOut(music);
                        }

                        Debug.WriteLine(Task.CurrentId.ToString() + ": ending play");
                        return;
                    }

                    Task.Delay(200).Wait();
                }
            }
        }

        private static void PlaySoundEffect(string title)
        {
            string soundeffectPath = AudioManager.GetSoundEffectPath(title);

            if (soundEffect.Status == SoundStatus.Playing)
            {
                soundEffect.Stop();
            }

            soundEffect = new Sound(new SoundBuffer(soundeffectPath));

            soundEffect.Volume = soundeffectVolume;

            soundEffect.Play();
        }

        private static void FadeOut(Sound music)
        {
            int maxVol = (int)music.Volume;

            if (maxVol < 0)
            {
                music.Stop();
                return;
            }

            for (int i = 0; i < maxVol; i++)
            {
                music.Volume -= 1;
                Task.Delay(15).Wait();
            }

            music.Stop();
        }
    }
}
