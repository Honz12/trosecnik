using Raylib_cs;
using System.Numerics;

namespace trosecnik.src
{
    public class Player
    {
        public int X = 0;
        public int Y = 0;
        private byte direction = 0;
        public bool Flying = false;

        public void Update()
        {
            int targetX = X;
            int targetY = Y;

            if (Raylib.IsKeyPressed(KeyboardKey.W) || (Raylib.IsKeyDown(KeyboardKey.W) && Flying))
            {
                direction = 1;
                targetY--;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.A) || (Raylib.IsKeyDown(KeyboardKey.A) && Flying))
            {
                direction = 2;
                targetX--;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.S) || (Raylib.IsKeyDown(KeyboardKey.S) && Flying))
            {
                direction = 0;
                targetY++;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.D) || (Raylib.IsKeyDown(KeyboardKey.D) && Flying))
            {
                direction = 3;
                targetX++;
            }

            if (Program.world.GetTile(targetX, targetY).GetWalkable() || Flying)
            {
                X = targetX;
                Y = targetY;
            }
        }

        public void Draw(int tileSize)
        {
            Texture2D texture = TextureManager.GetTexture($"player/player_{direction + 1:D4}.png");
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle destRec = new Rectangle(Program.ScreenCenterX - tileSize / 2, Program.ScreenCenterY - tileSize / 2, tileSize, tileSize);
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }
    }
}