using Raylib_cs;
using System.Numerics;
using trosecnik.src.InventorySpace;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public class Player
    {
        public const int MAX_HEALTH = 100;
        public const int MAX_HUNGER = 100;
        public const double MAX_SATURATION = 20.0;
        public const int MAX_THIRST = 100;
        public const double MAX_THIRSTING = 20.0;
        
        public enum MovementMode
        {
            Pathfind, WASD
        }

        public int X;
        public int Y;

        public Pathfinder PlayerPathfinder;
        public Inventory PlayerInventory = new();

        public double MoveWait = 0;
        public MovementMode movementMode = MovementMode.WASD;

        public int Health = MAX_HEALTH;
        public int Hunger = MAX_HUNGER;
        public double Saturation = MAX_SATURATION;
        public int Thirst = MAX_THIRST;
        public double Thirsting = MAX_SATURATION;

        private const double MOVE_WAIT = 0.1;
        private const double EXAUSTION_PER_STEP = 0.1;
        private const double EXAUSTION_PER_TICK = 0.01; // {0.01 * 60}/s = 0.6/s
        private const double THIRSTING_PER_TICK = 0.01; // {0.01 * 60}/s = 0.6/s

        private World PlayerWorld;
        private byte direction = 0;
        private Random Rng = new();

        public Player(int x, int y, World world)
        {
            X = x;
            Y = y;
            PlayerWorld = world;
            PlayerPathfinder = new(world);

            PlayerInventory.AddItem(new InventorySpace.Items.StoneAxeItem());
        }

        private void PlayStepSound()
        {
            SoundManager.Play($"player/step/step{Rng.Next(5) + 1}.wav");
        }

        public void Update(ulong tick, Vector2 mousePosition, float deltaTime)
        {
            Saturation -= EXAUSTION_PER_TICK;
            Thirsting -= THIRSTING_PER_TICK;
            
            if (movementMode == MovementMode.Pathfind)
            {
                if (MoveWait <= 0)
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
                            Saturation -= EXAUSTION_PER_STEP;
                            MoveWait = MOVE_WAIT;
                            PlayStepSound();
                        }
                    }
                }
            }
            else if (movementMode == MovementMode.WASD)
            {
                int moveX = 0;
                int moveY = 0;
                if (MoveWait <= 0)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.W))
                    {
                        moveY--;
                        MoveWait = MOVE_WAIT;
                        direction = 1;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S))
                    {
                        moveY++;
                        MoveWait = MOVE_WAIT;
                        direction = 0;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A))
                    {
                        moveX--;
                        MoveWait = MOVE_WAIT;
                        direction = 2;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D))
                    {
                        moveX++;
                        MoveWait = MOVE_WAIT;
                        direction = 3;
                    }

                    if (moveX != 0 || moveY != 0)
                        if (PlayerWorld.GetWalkable(X + moveX, Y + moveY))
                        {
                            X += moveX;
                            Y += moveY;
                            Saturation -= EXAUSTION_PER_STEP;
                            PlayStepSound();
                        }
                }
                MoveWait -= deltaTime;
            }

            if (Health <= 0)
            {
                Program.appMode = Program.AppMode.YouDiedMenu;
            }

            if (Saturation <= 0)
            {
                Saturation += MAX_SATURATION;
                Hunger--;
            }

            if (Thirsting <= 0)
            {
                Thirsting += MAX_THIRSTING;
                Thirst--;
            }

            PlayerInventory.Update(this, mousePosition);

            if (Hunger > 95 && tick % 600 == 0)
            {
                Health++;
            }

            Health = Math.Max(0, Math.Min(MAX_HEALTH, Health));
            Hunger = Math.Max(0, Math.Min(MAX_HUNGER, Hunger));
            Thirst = Math.Max(0, Math.Min(MAX_HUNGER, Thirst));
            Saturation = Math.Min(MAX_SATURATION, Saturation);
            Thirsting = Math.Min(MAX_SATURATION, Thirsting);
        }

        public void Draw(int tileSize, Camera camera)
        {
            Texture2D texture = TextureManager.GetTexture($"player/player_{direction + 1:D4}.png");
            Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle destRec = new Rectangle((int) ((X - camera.X) * tileSize) + Program.ScreenCenterX - tileSize / 2, (int) ((Y - camera.Y) * tileSize) + Program.ScreenCenterY - tileSize / 2, tileSize, tileSize);
            Vector2 origin = Vector2.Zero;
            Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, 0.0f, Color.White);

            if (!PlayerPathfinder.Finished && movementMode == MovementMode.Pathfind)
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