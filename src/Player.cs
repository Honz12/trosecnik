using Raylib_cs;
using System.Numerics;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public class Player(World world)
    {
        public int X = 0;
        public int Y = 0;
        private byte direction = 0;
        public Pathfinder PlayerPathfinder = new(world);

        public void Update(ulong tick)
        {
            if (tick % 10 == 0)
            {
                if (!PlayerPathfinder.Finished)
                {
                    (int X, int Y)? pos = PlayerPathfinder.GetNextStep();

                    if (pos != null)
                    {
                        X = pos.Value.X;
                        Y = pos.Value.Y;
                    }
                }
            }
        }

        public void Draw(int tileSize, Camera camera)
        {
            Texture2D texture = TextureManager.GetTexture($"player/player_{direction + 1:D4}.png");
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle destRec = new Rectangle((int) ((X - camera.X) * world.TileSize) + Program.ScreenCenterX - world.TileSize / 2, (int) ((Y - camera.Y) * world.TileSize) + Program.ScreenCenterY - world.TileSize / 2, world.TileSize, world.TileSize);
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }
    }
}