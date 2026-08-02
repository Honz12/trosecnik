using System.Numerics;
using Raylib_cs;
using trosecnik.src.World;

namespace trosecnik.src
{
    public static class Program
    {
        public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, Math.Max(0, count)));

        public const string VER_STRING = "0.2";

        public static int ScreenWidth = 640;
        public static int ScreenHeight = 360;

        public static World.World world = new(512, 512);
        public static Player.Player player = null!;
        public static UI.HUD hud = new();
        public static Camera2D camera;

        public static void Main()
        {
            world.GenerateIsland();
            player = new Player.Player(world);

            camera = new Camera2D
            {
                Zoom = 1.0f
            };

            Raylib.InitWindow(ScreenWidth, ScreenHeight, $"Trosečník {VER_STRING}");

            Raylib.SetExitKey(KeyboardKey.Null);

            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                ScreenWidth = Raylib.GetScreenWidth();
                ScreenHeight = Raylib.GetScreenHeight();

                float tileScale = MathF.Min(ScreenWidth / 640f, ScreenHeight / 360f);
                camera.Zoom = tileScale;

                if (Raylib.IsKeyPressed(KeyboardKey.F11))
                {
                    Raylib.ToggleFullscreen();
                }

                // --- UPDATE LOGIC ---
                player.Update();
                UpdateCamera();

                // --- DRAW LOGIC ---
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.Black);

                Raylib.BeginMode2D(camera);

                world.Draw(CameraViewLeft(), CameraViewTop(), CameraViewWidth(), CameraViewHeight());

                player.Draw();

                Raylib.EndMode2D();

                hud.Draw();

                Raylib.EndDrawing();
            }

            TextureManager.UnloadAll();
            Raylib.CloseWindow();
        }

        private static void UpdateCamera()
        {
            float halfW = CameraViewWidth() / 2f;
            float halfH = CameraViewHeight() / 2f;

            float worldW = world.Width * world.TileSize;
            float worldH = world.Height * world.TileSize;

            float tx = Math.Clamp(player.Position.X, halfW, worldW - halfW);
            float ty = Math.Clamp(player.Position.Y, halfH, worldH - halfH);

            camera.Target = new Vector2(tx, ty);
            camera.Offset = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);
        }

        private static float CameraViewWidth() => ScreenWidth / camera.Zoom;
        private static float CameraViewHeight() => ScreenHeight / camera.Zoom;
        private static float CameraViewLeft() => camera.Target.X - CameraViewWidth() / 2f;
        private static float CameraViewTop() => camera.Target.Y - CameraViewHeight() / 2f;
    }
}
