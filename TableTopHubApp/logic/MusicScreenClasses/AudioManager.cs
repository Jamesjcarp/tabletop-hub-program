// <copyright file="AudioManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager for audio handles data reading and file opening.
    /// </summary>
    internal static class AudioManager
    {
        // dictionary containing name -> [intro file name, loop file name]
        private static readonly Dictionary<string, string[]> Tracks = new Dictionary<string, string[]>();

        private static readonly Dictionary<string, string> SoundEffects = new Dictionary<string, string>();

        /// <summary>
        /// Opens the data files and fills the dictionaries with data pertaining to the tracks and sounds.
        /// </summary>
        public static void InitTracks()
        {
            Tracks.Clear();
            SoundEffects.Clear();

            string[] musicContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\Tracklist.txt"));

            for (int i = 0; i < musicContent.Length; i++)
            {
                string[] split = musicContent[i].Split(',');
                Tracks[split[0]] = [split[1], split[2]];
            }

            Tracks.TrimExcess();

            string[] soundEffectContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\SoundEffectList.txt"));

            for (int i = 0; i < soundEffectContent.Length; i++)
            {
                string[] split = soundEffectContent[i].Split(',');
                if(split.Length > 1)
                {
                    SoundEffects[split[0]] = split[1];
                }
            }

            SoundEffects.TrimExcess();
        }

        /// <summary>
        /// Gets the list of track titles for UI.
        /// </summary>
        /// <returns>List of string titles.</returns>
        public static List<string> GetTrackTitles()
        {
            return Tracks.Keys.ToList();
        }

        /// <summary>
        /// Gets the list of sound effect names for UI.
        /// </summary>
        /// <returns>List of string sound effects.</returns>
        public static List<string> GetSoundTitles()
        {
            return SoundEffects.Keys.ToList();
        }

        /// <summary>
        /// Gets the dictionary of tracks containing all data on a track.
        /// </summary>
        /// <returns>Dictionary of song title to data.</returns>
        public static Dictionary<string, string[]> GetTracks()
        {
            return Tracks;
        }

        /// <summary>
        /// Gets the dictionary of sound effects containing all data on a sound.
        /// </summary>
        /// <returns>Dictionary of sound effect titles to data.</returns>
        public static Dictionary<string, string> GetSoundEffects()
        {
            return SoundEffects;
        }

        /// <summary>
        /// Takes a track name and gets it's corresponding file paths.
        /// </summary>
        /// <param name="trackTitle">name of the track.</param>
        /// <returns>stirng path formatted to eb ready to use.</returns>
        /// <exception cref="Exception">if function is called on a track that is not in the data then raise an exception.</exception>
        public static string[] GetTrackPath(string trackTitle)
        {
            if (Tracks.ContainsKey(trackTitle))
            {
                if (Tracks[trackTitle][1] == "NULL")
                {
                    return [Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder", Tracks[trackTitle][0]), "NULL"];
                }
                else
                {
                    return [Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder", Tracks[trackTitle][0]), Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder", Tracks[trackTitle][1])];
                }
            }
            else
            {
                throw new Exception("No song with that title found");
            }
        }

        /// <summary>
        /// Takes a sond effect and gets its corresponding file path.
        /// </summary>
        /// <param name="soundEffectTitle">name of the sound.</param>
        /// <returns>formatted file path.</returns>
        /// <exception cref="Exception">if given a name that the manager does not recognize. throw an exception.</exception>
        public static string GetSoundEffectPath(string soundEffectTitle)
        {
            if (SoundEffects.ContainsKey(soundEffectTitle))
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder", SoundEffects[soundEffectTitle]);
            }
            else
            {
                throw new Exception("No sound effect with that title found");
            }
        }
    }
}
