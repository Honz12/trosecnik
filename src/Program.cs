using Raylib_cs;

namespace trosecnik.src
{
    public static class Program
    {
        public const string VER_STRING = "0.1";

        public static int ScreenWidth = 640;
        public static int ScreenHeight = 360;

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
                
                Raylib.ClearBackground(Color.RayWhite);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
