using Raylib_cs;

namespace trosecnik.src
{
    public static class TextureManager
    {
        private static readonly Dictionary<string, Texture2D> _cache = [];

        /// <summary>
        /// Retrieves a texture from the cache or loads it from disk if not cached yet.
        /// </summary>
        public static Texture2D GetTexture(string textureId)
        {
            if (_cache.TryGetValue(textureId, out Texture2D cachedTexture))
            {
                return cachedTexture;
            }

            string filePath = $"assets/textures/{textureId}";

            Texture2D newTexture = Raylib.LoadTexture(filePath);
            _cache[textureId] = newTexture;

            return newTexture;
        }

        /// <summary>
        /// Unloads all cached textures from VRAM (Call when exiting the game).
        /// </summary>
        public static void UnloadAll()
        {
            foreach (var texture in _cache.Values)
            {
                Raylib.UnloadTexture(texture);
            }
            _cache.Clear();
        }
    }
}