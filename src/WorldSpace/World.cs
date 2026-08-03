using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.WorldSpace
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

        private readonly ITile[,] tiles;
        private bool[,] entityTileBlocks;
        private bool[] entityTileBlocksColumnChanged;
        private readonly List<IEntity> entities;
        public int Width;
        public int Height;

        public int ClearingEntityTileBlocksColumns = 0;

        public int TileSize = 16;

        public World(int width, int height, int seed)
        {
            Width = width;
            Height = height;

            tiles = new ITile[width, height];
            entityTileBlocks = new bool[width, height];
            entityTileBlocksColumnChanged = new bool[width];
            entities = [];

            GenerateWorld(seed);
        }

        public void Draw(Camera camera, ulong tick)
        {
            int offsetX = (int) camera.X;
            int offsetY = (int) camera.Y;
            double offsetLittleX = camera.X % 1;
            double offsetLittleY = camera.Y % 1;

            for(int rx = -Program.ScreenTilesHor / 2 - 8; rx < Program.ScreenTilesHor / 2 + 8; rx++)
            {
                for (int ry = -Program.ScreenTilesVer / 2 - 8; ry < Program.ScreenTilesVer / 2 + 8; ry++)
                {
                    int x = rx + offsetX;
                    int y = ry + offsetY;

                    if (x < 0 || x >= Width || y < 0 || y >= Height)
                        continue;

                    ITile tile = tiles[x, y];

                    string tileId = $"tiles/tile_{tile.GetTextureId(tick):D4}.png";
                    Vector2 position = new((float) ((rx - offsetLittleX) * TileSize), (float) ((ry - offsetLittleY) * TileSize));

                    TileRenderer.DrawTile(tileId, position, TileSize);
                }
            }
            foreach (var entity in entities)
            {
                DrawEntity(entity, camera, tick);
            }
        }

        public ITile GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return voidTile;
            return tiles[x, y];
        }

        public void DrawEntity(IEntity entity, Camera camera, ulong tick)
        {
            Vector2 entitySize = entity.GetTextureSize(tick);
            Texture2D texture = TextureManager.GetTexture(entity.GetTexture(tick));
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width * entitySize.X, texture.Height * entitySize.Y);
            Rectangle destRec = new Rectangle((int) ((entity.GetPosition(tick).X - camera.X) * TileSize * entitySize.X) + Program.ScreenCenterX - TileSize / 2, (int) ((entity.GetPosition(tick).Y - camera.Y) * TileSize * entitySize.Y) + Program.ScreenCenterY - TileSize / 2, TileSize, TileSize);
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }

        private void GenerateWorld(int seed)
        {
            FastNoiseLite heightNoise = new(seed);

            heightNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
            heightNoise.SetFrequency(0.03f);
            heightNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
            heightNoise.SetFractalOctaves(4);
            heightNoise.SetFractalLacunarity(2f);
            heightNoise.SetFractalGain(0.5f);
            heightNoise.SetFractalWeightedStrength(0f);

            /*
            FastNoiseLite riverNoise = new(seed);

            riverNoise.SetNoiseType(FastNoiseLite.NoiseType.ValueCubic);
            riverNoise.SetFrequency(0.2f);
            riverNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
            riverNoise.SetFractalOctaves(2);
            riverNoise.SetFractalLacunarity(2f);
            riverNoise.SetFractalGain(0.5f);
            riverNoise.SetFractalWeightedStrength(0f);
            */

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double height = heightNoise.GetNoise(x, y);
                    if (height > 0.1)
                    {
                        tiles[x, y] = new Tiles.GrassTile();
                    }
                    else if (height > 0)
                    {
                        tiles[x, y] = new Tiles.SandTile();
                    }
                    else if (height > -0.5)
                    {
                        tiles[x, y] = new Tiles.WaterTile();
                    }
                    else
                    {
                        tiles[x, y] = new Tiles.DeepWaterTile();
                    }
                }
            }
        }

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public void UpdateEntites(Player player, ulong tick)
        {
            // Clear entity block buffer
            {
                ClearingEntityTileBlocksColumns = 0;
                for (int x = 0; x < Width; x++)
                {
                    if (!entityTileBlocksColumnChanged[x])
                    {
                        continue;
                    }
                    entityTileBlocksColumnChanged[x] = false;
                    ClearingEntityTileBlocksColumns++;
                    for (int y = 0; y < Height; y++)
                    {
                        entityTileBlocks[x, y] = false;
                    }
                }
            }

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                entity.Update(player, this, tick);
                IEntity.EntityRequest entityRequest = entity.GetRequest();

                if (entityRequest == IEntity.EntityRequest.SelfDelete)
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }

        public void EntityBlockTile(int x, int y)
        {
            if (0 <= x && x < Width)
            {
                if (0 <= y && y < Height)
                {
                    entityTileBlocks[x, y] = true;
                    entityTileBlocksColumnChanged[x] = true;
                }
            }
        }

        public bool IsTileEntityBlocked(int x, int y)
        {
            if (0 <= x && x < Width)
            {
                if (0 <= y && y < Height)
                {
                    return entityTileBlocks[x, y];
                }
            }
            return false;
        }

        public bool GetWalkable(int x, int y)
        {
            if (IsTileEntityBlocked(x, y)) return false;

            ITile tile = GetTile(x, y);

            return tile.GetWalkable();
        }
    }
}
