using Raylib_cs;
using System.Numerics;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public class Player(World world)
    {
        public int X = 0;
        public int Y = 0;
        public int Health = 100;
        private byte direction = 0;
        public Pathfinder PlayerPathfinder = new(world);

        public void Update(ulong tick)
        {
            if (tick % 5 == 0)
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

            if (Health <= 0)
            {
                Program.appMode = Program.AppMode.YouDiedMenu;
            }
        }

        public void Draw(int tileSize, Camera camera)
        {
            Texture2D texture = TextureManager.GetTexture($"player/player_{direction + 1:D4}.png");
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle destRec = new Rectangle((int) ((X - camera.X) * tileSize) + Program.ScreenCenterX - tileSize / 2, (int) ((Y - camera.Y) * tileSize) + Program.ScreenCenterY - tileSize / 2, tileSize, tileSize);
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);

            if (!PlayerPathfinder.Finished)
            {
                texture = TextureManager.GetTexture($"player/player_0005.png");
                sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
                destRec = new Rectangle((int) ((PlayerPathfinder.GetTargetX() - camera.X) * tileSize) + Program.ScreenCenterX - tileSize / 2, (int) ((PlayerPathfinder.GetTargetY() - camera.Y) * tileSize) + Program.ScreenCenterY - tileSize / 2, tileSize, tileSize);
                origin = Vector2.Zero;
                Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
            }
        }
    }
}