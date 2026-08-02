using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.World
{
    public class TileRenderer
    {
        public static void DrawTile(string tileId, Vector2 position, int tileSize)
        {
            Texture2D texture = TextureManager.GetTexture(tileId);

            // Entire source image
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);

            // Destination rectangle on screen (Position + Size)
            Rectangle destRec = new Rectangle(position.X + Program.ScreenCenterX - tileSize / 2, position.Y + Program.ScreenCenterY - tileSize / 2, tileSize, tileSize);

            // Origin for rotation (top-left is 0,0)
            Vector2 origin = Vector2.Zero;

            // Draw with scaling
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }
    }

    public class World
    {
        public static Tiles.VoidTile voidTile = new();

        ITile[,] tiles;
        public int Width;
        public int Height;

        public int TileSize = 16;

        public World(int width, int height, int seed)
        {
            Width = width;
            Height = height;

            tiles = new ITile[width, height];

            GenerateWorld(seed);
        }

        public void Draw(int offsetX, int offsetY, ulong tick)
        {
            for(int rx = -Program.ScreenTilesHor; rx < Program.ScreenTilesHor / 2 + 1; rx++)
            {
                for (int ry = -Program.ScreenTilesVer; ry < Program.ScreenTilesVer / 2 + 1; ry++)
                {
                    int x = rx + offsetX;
                    int y = ry + offsetY;

                    if (x < 0 || x >= Width || y < 0 || y >= Height)
                        continue;

                    ITile tile = tiles[x, y];

                    string tileId = $"tiles/tile_{tile.GetTextureId(tick):D4}.png";
                    Vector2 position = new(rx * TileSize, ry * TileSize);

                    TileRenderer.DrawTile(tileId, position, TileSize);
                }
            }
        }

        public ITile GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return voidTile;
            return tiles[x, y];
        }

        private void GenerateWorld(int seed)
        {
            FastNoiseLite heightNoise = new(seed);

            heightNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
            heightNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
            heightNoise.SetFrequency(0.03f);
            heightNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
            heightNoise.SetFractalOctaves(4);
            heightNoise.SetFractalLacunarity(2f);
            heightNoise.SetFractalGain(0.5f);
            heightNoise.SetFractalWeightedStrength(0f);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double height = heightNoise.GetNoise(x, y);
                    if (height > 0.1)
                    {
                        tiles[x, y] = new Tiles.GrassTile();
                    }
                    else
                    {
                        tiles[x, y] = new Tiles.WaterTile();
                    }
                }
            }
        }
    }
}
