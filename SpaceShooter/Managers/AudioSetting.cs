using System.Collections.Generic;

namespace SpaceShooter
{
    public static class AudioSettings
    {
        public static float MusicVolume { get; set; } = 0.5f;
        public static float SfxVolume { get; set; } = 1.0f;
        public static bool MusicMuted { get; set; } = false;
        public static bool SfxMuted { get; set; } = false;
        public static int SelectedTrackIndex { get; set; } = 0;

        public static readonly List<(string Name, string Path)> Tracks = new List<(string, string)>
            {
                ("Track 1", @"Resources\Background2.wav"),
                ("Track 2", @"Resources\Background1.wav"),
                ("Track 3", @"Resources\Background4.wav"),
                ("Track 4", @"Resources\Background3.wav"),
            };
    }
}
