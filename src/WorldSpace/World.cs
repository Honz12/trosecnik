using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.WorldSpace
{
    public class TileRenderer
    {
        public static void DrawTile(Vector2 position, int tileSize, Vector2 atlasCoords)
        {
            Texture2D texture = TextureManager.GetTexture("tiles/tile.png");

            // Entire source image
            Rectangle sourceRec = new Rectangle(atlasCoords.X * Program.BASE_TILE_SIZE, atlasCoords.Y * Program.BASE_TILE_SIZE, Program.BASE_TILE_SIZE, Program.BASE_TILE_SIZE);

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

        private readonly ITile[,] layer1;
        private readonly ITile?[,] layer2;
        private bool[,] entityTileBlocks;
        private bool[] entityTileBlocksColumnChanged;
        private readonly List<IEntity> entities;
        public readonly Dictionary<Vector2, IEntity> interactableEntities;
        public int Width;
        public int Height;

        public int ClearingEntityTileBlocksColumns = 0;

        public int TileSize = 16;

        public World(int width, int height, int seed)
        {
            Width = width;
            Height = height;

            layer1 = new ITile[width, height];
            layer2 = new ITile[width, height];
            entityTileBlocks = new bool[width, height];
            entityTileBlocksColumnChanged = new bool[width];
            entities = [];
            interactableEntities = [];

            GenerateWorld(seed);
        }

        public void Draw(Player player, Camera camera, ulong tick)
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

                    ITile tileL1 = layer1[x, y];
                    ITile? tileL2 = layer2[x, y];

                    Vector2 position = new((float) ((rx - offsetLittleX) * TileSize), (float) ((ry - offsetLittleY) * TileSize));

                    TileRenderer.DrawTile(position, TileSize, tileL1.GetTextureAltlasCoords(tick));
                    if (tileL2 != null)
                    {
                        TileRenderer.DrawTile(position, TileSize, tileL2.GetTextureAltlasCoords(tick));
                    }
                }
            }
            foreach (var entity in entities)
            {
                DrawEntity(entity, camera, tick);
            }
            player.Draw(TileSize, camera);
            foreach (var entity in entities)
            {
                DrawEntityAbove(entity, camera, tick);
            }
        }

        public ITile GetTileLayer1(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return voidTile;
            return layer1[x, y];
        }

        public ITile? GetTileLayer2(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            return layer2[x, y];
        }

        public bool SetTileLayer2(ITile? tile, int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
            layer2[x, y] = tile;
            return true;
        }

        public void DrawEntity(IEntity entity, Camera camera, ulong tick)
        {
            Vector2 entitySize = entity.GetTextureSize(tick);
            Texture2D texture = TextureManager.GetTexture(entity.GetTexture(tick));
            Rectangle sourceRec = new(0, 0, texture.Width, texture.Height);
            Rectangle destRec = new(
                (int) ((entity.GetPosition(tick).X - camera.X) * TileSize) + Program.ScreenCenterX - TileSize / 2,
                (int) ((entity.GetPosition(tick).Y - camera.Y) * TileSize) + Program.ScreenCenterY - TileSize / 2,
                TileSize * entitySize.X, TileSize * entitySize.Y
            );
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }

        public void DrawEntityAbove(IEntity entity, Camera camera, ulong tick)
        {
            Vector2 entitySize = entity.GetTextureSize(tick);
            if (entity.GetTextureAbove(tick) != null) {
                Texture2D texture = TextureManager.GetTexture(entity.GetTextureAbove(tick)!);
                Rectangle sourceRec = new(0, 0, texture.Width, texture.Height);
                Rectangle destRec = new(
                    (int) ((entity.GetPosition(tick).X - camera.X) * TileSize) + Program.ScreenCenterX - TileSize / 2,
                    (int) ((entity.GetPosition(tick).Y - camera.Y) * TileSize) + Program.ScreenCenterY - TileSize / 2,
                    TileSize * entitySize.X, TileSize * entitySize.Y
                );
                Vector2 origin = Vector2.Zero;
                Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
            }
        }

        private void GenerateWorld(int seed)
        {
            FastNoiseLite heightNoise = new(seed);
            Random rng = new(seed);

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

            List<Vector2> grassTiles = [];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double height = heightNoise.GetNoise(x, y);
                    if (height > 0.1)
                    {
                        layer1[x, y] = new Tiles.GrassTile();
                        grassTiles.Add(new (x, y));
                    }
                    else if (height > 0)
                    {
                        layer1[x, y] = new Tiles.SandTile();
                    }
                    else if (height > -0.5)
                    {
                        layer1[x, y] = new Tiles.WaterTile();
                    }
                    else
                    {
                        layer1[x, y] = new Tiles.DeepWaterTile();
                    }
                }
            }

            Dictionary<Vector2, Entities.TreeEntity> trees = [];

            foreach (var coords in grassTiles)
            {
                if (rng.Next(100) == 0)
                {
                    bool validPosition = true;

                    for (int x = -2; x <= 2; x++)
                    {
                        for (int y = -2; y <= 2; y++)
                        {
                            if (trees.ContainsKey(new Vector2(x + coords.X, y + coords.Y)))
                            {
                                validPosition = false;
                                break;
                            }
                        }
                    }

                    if (!validPosition)
                    {
                        continue;
                    }

                    var treeEntity = new Entities.TreeEntity();

                    treeEntity.SetPos(coords);

                    entities.Add(treeEntity);

                    trees.Add(coords, treeEntity);
                    interactableEntities.Add(coords, treeEntity);
                }
            }
        }

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public void UpdateEntites(Player player, ulong tick, float deltaTime)
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

                entity.Update(player, this, tick, deltaTime);
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

            ITile tile = GetTileLayer1(x, y);
            ITile? layer2Tile = GetTileLayer2(x, y);

            if (layer2Tile != null)
            {
                return layer2Tile.GetWalkable();
            }

            return tile.GetWalkable();
        }

        public int GetEntityCount()
        {
            return entities.Count;
        }
    }
}
