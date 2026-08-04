using Raylib_cs;

namespace trosecnik.src
{
    public static class SoundManager
    {
        private static readonly Dictionary<string, Sound> _cache = [];
        private static readonly List<Sound> _playingUncached = [];

        /// <summary>
        /// Plays a sound, loading and caching it from disk if not cached yet.
        /// </summary>
        public static void Play(string path)
        {
            Play(path, true);
        }

        /// <summary>
        /// Plays a sound, either from the cache or as a one-shot that is unloaded once it stops playing.
        /// </summary>
        public static void Play(string path, bool cached)
        {
            if (cached)
            {
                if (_cache.TryGetValue(path, out Sound cachedSound))
                {
                    Raylib.PlaySound(cachedSound);
                    return;
                }

                string filePath = $"assets/audio/{path}";

                Sound newSound = Raylib.LoadSound(filePath);
                _cache[path] = newSound;

                Raylib.PlaySound(newSound);
            }
            else
            {
                string filePath = $"assets/audio/{path}";

                Sound newSound = Raylib.LoadSound(filePath);
                Raylib.PlaySound(newSound);

                _playingUncached.Add(newSound);
            }
        }

        /// <summary>
        /// Unloads one-shot sounds that have finished playing (Call each frame).
        /// </summary>
        public static void Update()
        {
            for (int i = _playingUncached.Count - 1; i >= 0; i--)
            {
                if (!Raylib.IsSoundPlaying(_playingUncached[i]))
                {
                    Raylib.UnloadSound(_playingUncached[i]);
                    _playingUncached.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Unloads all cached and pending sounds (Call when exiting the game).
        /// </summary>
        public static void UnloadAll()
        {
            foreach (var sound in _cache.Values)
            {
                Raylib.UnloadSound(sound);
            }
            _cache.Clear();

            foreach (var sound in _playingUncached)
            {
                Raylib.UnloadSound(sound);
            }
            _playingUncached.Clear();
        }
    }
}
