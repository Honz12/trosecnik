using System.Numerics;
using Raylib_cs;
using trosecnik.src.World;

namespace trosecnik.src.Player
{
    public class Player
    {
        public Vector2 Position;
        public float Speed = 120f;
        public float Width = 10f;
        public float Height = 14f;
        public bool FacingRight = true;

        private readonly World.World _world;

        public Player(World.World world)
        {
            _world = world;
            Position = world.GetSpawnPoint();
        }

        public void Update()
        {
            Vector2 dir = Vector2.Zero;

            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) dir.Y -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) dir.Y += 1;
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) dir.X -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) dir.X += 1;

            if (dir.LengthSquared() > 0)
            {
                dir = Vector2.Normalize(dir);
                if (dir.X != 0) FacingRight = dir.X > 0;
            }

            float dt = Raylib.GetFrameTime();

            MoveAxis(dir.X * Speed * dt, true);
            MoveAxis(dir.Y * Speed * dt, false);
        }

        private void MoveAxis(float delta, bool horizontal)
        {
            if (delta == 0) return;

            float newX = horizontal ? Position.X + delta : Position.X;
            float newY = horizontal ? Position.Y : Position.Y + delta;

            if (CanMove(newX, newY))
            {
                Position = new Vector2(newX, newY);
            }
        }

        private bool CanMove(float x, float y)
        {
            float left = x - Width / 2f;
            float right = x + Width / 2f;
            float top = y - Height / 2f;
            float bottom = y + Height / 2f;

            return _world.IsTileWalkable(left, top) &&
                   _world.IsTileWalkable(right, top) &&
                   _world.IsTileWalkable(left, bottom) &&
                   _world.IsTileWalkable(right, bottom);
        }

        public void Draw()
        {
            Rectangle body = new(Position.X - Width / 2f, Position.Y - Height / 2f, Width, Height);

            // body
            Raylib.DrawRectangleRec(body, new Color(46, 32, 24, 255));

            // head
            Raylib.DrawCircle((int)Position.X, (int)(Position.Y - Height / 2f - 4f), 5f, new Color(255, 205, 180, 255));
        }
    }
}
