namespace DnD_music_program
{
    /// <summary>
    /// Class that handles reading the text file containing the track data.
    /// </summary>
    internal static class AudioManager
    {
        // dictionary containing name -> [intro file name, loop file name]
        private static readonly Dictionary<string, string[]> tracks = new Dictionary<string, string[]>();

        private static readonly Dictionary<string, string> soundEffects = new Dictionary<string, string>();

        
        public static void InitTracks()
        {
            string[] musicContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "audioData\\Tracklist.txt"));

            for(int i = 0; i < musicContent.Length; i++)
            {
                string[] split = musicContent[i].Split(',');
                tracks[split[0]] = [split[1],split[2]];
            }

            string[] soundEffectContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "audioData\\SoundEffectList.txt"));

            for(int i = 0;i < soundEffectContent.Length; i++)
            {
                string[] split = soundEffectContent[i].Split(',');
                soundEffects[split[0]] = split[1];
            }
        }

        public static Dictionary<string, string[]> GetTracks()
        {
            return tracks;
        }

        public static Dictionary<string, string> GetSoundEffects()
        {
            return soundEffects;
        }

        public static string[] GetTrackPath(string trackTitle)
        {
            if (tracks.ContainsKey(trackTitle)) 
            {
                if (tracks[trackTitle][1] == "NULL")
                {
                    return [Path.Combine(Directory.GetCurrentDirectory(), "musicFolder", tracks[trackTitle][0]), "NULL"];
                }
                else
                {
                    return [Path.Combine(Directory.GetCurrentDirectory(), "musicFolder", tracks[trackTitle][0]), Path.Combine(Directory.GetCurrentDirectory(), "musicIntroFolder", tracks[trackTitle][1])];
                }
            }
            else
            {
                throw new Exception("No song with that title found");
            }
            
        }

        public static string GetSoundEffectPath(string soundEffectTitle)
        {
            if (soundEffects.ContainsKey(soundEffectTitle))
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "soundEffectsFolder", soundEffects[soundEffectTitle]);
            }
            else
            {
                throw new Exception("No sound effect with that title found");
            }
        }
    }
}
