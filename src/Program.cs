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

        public static World.World world = new(8, 8);

        public static void Main()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, $"Trosečník {VER_STRING}");

            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                // --- UPDATE LOGIC HERE ---
                // Example: Handle input or move objects

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
