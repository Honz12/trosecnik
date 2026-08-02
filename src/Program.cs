using Raylib_cs;
using trosecnik.src.World;

namespace trosecnik.src
{
    public static class Program
    {
        public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, Math.Max(0, count)));

        public const string VER_STRING = "0.1";

        public static int ScreenWidth = 640;
        public static int ScreenHeight = 360;

        public static World.World world = new(512, 512);

        public static void Main()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, $"Trosečník {VER_STRING}");

            Raylib.SetExitKey(KeyboardKey.Null);

            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                ScreenWidth = Raylib.GetScreenWidth();
                ScreenHeight = Raylib.GetScreenHeight();

                int tileScale = Math.Min(ScreenWidth / 640, ScreenHeight / 360);
                world.TileSize = tileScale * 16;

                // --- UPDATE LOGIC HERE ---
                // Example: Handle input or move objects

                if (Raylib.IsKeyPressed(KeyboardKey.F11))
                {
                    Raylib.ToggleFullscreen();
                }

                // --- DRAW LOGIC HERE ---
                Raylib.BeginDrawing();
                
                Raylib.ClearBackground(Color.Black);

                world.Draw();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
