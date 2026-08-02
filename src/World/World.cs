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

        public void GenerateIsland()
        {
            int cx = Width / 2;
            int cy = Height / 2;
            int radius = Width / 4;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float dx = x - cx;
                    float dy = y - cy;
                    float dist = MathF.Sqrt(dx * dx + dy * dy);

                    float noise = Noise(x, y, 64, 12345u) * 0.6f + Noise(x, y, 16, 54321u) * 0.4f;
                    float value = (radius - dist) / radius + (noise - 0.5f) * 0.8f;

                    if (value > 0.18f)
                        tiles[x, y] = new Tiles.GrassTile();
                    else if (value > 0.0f)
                        tiles[x, y] = new Tiles.SandTile();
                    else
                        tiles[x, y] = new Tiles.WaterTile();
                }
            }
        }

        public ITile? GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) return null;
            return tiles[x, y];
        }

        public bool IsTileWalkable(float worldX, float worldY)
        {
            int tx = (int)MathF.Floor(worldX / TileSize);
            int ty = (int)MathF.Floor(worldY / TileSize);

            ITile? tile = GetTile(tx, ty);
            return tile != null && tile.GetWalkable();
        }

        public Vector2 GetSpawnPoint()
        {
            int cx = Width / 2;
            int cy = Height / 2;
            int maxRadius = Math.Max(Width, Height) / 2;

            for (int r = 0; r <= maxRadius; r++)
            {
                for (int x = cx - r; x <= cx + r; x++)
                {
                    for (int y = cy - r; y <= cy + r; y++)
                    {
                        if (GetTile(x, y) is Tiles.SandTile)
                        {
                            return new Vector2(x * TileSize + TileSize / 2f, y * TileSize + TileSize / 2f);
                        }
                    }
                }
            }

            return new Vector2(cx * TileSize, cy * TileSize);
        }

        public void Draw(float viewLeft, float viewTop, float viewWidth, float viewHeight)
        {
            int startX = Math.Max(0, (int)MathF.Floor(viewLeft / TileSize));
            int startY = Math.Max(0, (int)MathF.Floor(viewTop / TileSize));
            int endX = Math.Min(Width - 1, (int)MathF.Ceiling((viewLeft + viewWidth) / TileSize));
            int endY = Math.Min(Height - 1, (int)MathF.Ceiling((viewTop + viewHeight) / TileSize));

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    ITile tile = tiles[x, y];
                    tile.UpdateRenderState();

                    string tileId = $"tiles/tile_{tile.GetTextureId():D4}";
                    Vector2 position = new(x * TileSize, y * TileSize);

                    TileRenderer.DrawTile(tileId, position, TileSize);
                }
            }
        }

        private static float Noise(int x, int y, int scale, uint seed)
        {
            int gx = Math.DivRem(x, scale, out int rx);
            int gy = Math.DivRem(y, scale, out int ry);

            float tx = rx / (float)scale;
            float ty = ry / (float)scale;

            float a = Hash(gx, gy, seed);
            float b = Hash(gx + 1, gy, seed);
            float c = Hash(gx, gy + 1, seed);
            float d = Hash(gx + 1, gy + 1, seed);

            tx = Smoothstep(tx);
            ty = Smoothstep(ty);

            return Lerp(Lerp(a, b, tx), Lerp(c, d, tx), ty);
        }

        private static float Hash(int x, int y, uint seed)
        {
            uint n = seed;
            n ^= (uint)x * 374761393u;
            n ^= (uint)y * 668265263u;
            n = (n ^ (n >> 13)) * 1274126177u;
            n ^= n >> 16;
            return (n & 0xFFFF) / 65535f;
        }

        private static float Smoothstep(float t) => t * t * (3 - 2 * t);

        private static float Lerp(float a, float b, float t) => a + (b - a) * t;
    }
}
