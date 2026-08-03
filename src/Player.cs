using Raylib_cs;
using System.Numerics;
using trosecnik.src.InventorySpace;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public class Player
    {
        private const int MOVE_WAIT = 10;

        public enum MovementMode
        {
            Pathfind, WASD
        }

        public int X;
        public int Y;
        public int Health = 100;
        private byte direction = 0;
        public Pathfinder PlayerPathfinder;
        public Inventory PlayerInventory = new();
        public double Saturation = 100.0;
        public int Hunger = 100;
        private World PlayerWorld;
        private int moveWaiter = 0;

        public MovementMode movementMode = MovementMode.WASD;

        public Player(int x, int y, World world)
        {
            X = x;
            Y = y;
            PlayerWorld = world;
            PlayerPathfinder = new(world);

            PlayerInventory.AddItem(new InventorySpace.Items.RedBerriesItem());
        }

        public void Update(ulong tick)
        {
            Saturation -= 0.01;
            
            if (movementMode == MovementMode.Pathfind)
            {
                if (tick % MOVE_WAIT == 0)
                {
                    if (!PlayerPathfinder.Finished)
                    {
                        PlayerPathfinder.SetStart(X, Y);
                        PlayerPathfinder.Recalculate();

                        (int X, int Y)? pos = PlayerPathfinder.GetNextStep();

                        if (pos != null)
                        {
                            X = pos.Value.X;
                            Y = pos.Value.Y;
                            Saturation -= 1.0;
                        }
                    }
                }
            }
            else if (movementMode == MovementMode.WASD)
            {
                int moveX = 0;
                int moveY = 0;
                if (moveWaiter <= 0)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.W))
                    {
                        moveY--;
                        moveWaiter = MOVE_WAIT;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S))
                    {
                        moveY++;
                        moveWaiter = MOVE_WAIT;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A))
                    {
                        moveX--;
                        moveWaiter = MOVE_WAIT;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D))
                    {
                        moveX++;
                        moveWaiter = MOVE_WAIT;
                    }

                    if (PlayerWorld.GetWalkable(X + moveX, Y + moveY))
                    {
                        X += moveX;
                        Y += moveY;
                    }
                }
                moveWaiter--;
            }

            if (Health <= 0)
            {
                Program.appMode = Program.AppMode.YouDiedMenu;
            }

            if (Saturation <= 0)
            {
                Saturation += 100.0;
                Hunger--;
            }

            Health = Math.Max(0, Math.Min(100, Health));
            Hunger = Math.Max(0, Math.Min(100, Hunger));

            PlayerInventory.Update();
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