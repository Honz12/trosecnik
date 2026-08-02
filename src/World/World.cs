using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.World
{
    public static class TextureManager
    {
        private static readonly Dictionary<string, Texture2D> _cache = [];

        /// <summary>
        /// Retrieves a texture from the cache or loads it from disk if not cached yet.
        /// </summary>
        public static Texture2D GetTexture(string textureId)
        {
            // 1. Return cached texture if it exists
            if (_cache.TryGetValue(textureId, out Texture2D cachedTexture))
            {
                return cachedTexture;
            }

            // 2. Construct path (e.g., assets/textures/grass_tile.png)
            string filePath = $"assets/textures/{textureId}.png";

            // 3. Load texture from disk into GPU memory
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

    public class TileRenderer
    {
        public static void DrawTile(string tileId, Vector2 position, int tileSize)
        {
            Texture2D texture = TextureManager.GetTexture(tileId);

            // Entire source image
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);

            // Destination rectangle on screen (Position + Size)
            Rectangle destRec = new Rectangle(position.X, position.Y, tileSize, tileSize);

            // Origin for rotation (top-left is 0,0)
            Vector2 origin = Vector2.Zero;

            // Draw with scaling
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }
    }

    public class World
    {
        ITile[,] tiles;
        public int Width;
        public int Height;

        public int TileSize = 16;

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            tiles = new ITile[width, height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tiles.WaterTile();
                }
            }
        }

        public void Draw()
        {
            for(int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    ITile tile = tiles[x, y];
                    tile.UpdateRenderState();

                    string tileId = $"tiles/tile_{tile.GetTextureId():D4}";
                    Vector2 position = new(x * TileSize, y * TileSize);

                    TileRenderer.DrawTile(tileId, position, TileSize);
                }
            }
        }
    }
}
