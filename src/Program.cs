using System.Numerics;
using Raylib_cs;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public static class Program
    {
        public const int BASE_TILE_SIZE = 16;

        public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, Math.Max(0, count)));

        public const string VER_STRING = "0.1";

        public static int ScreenWidth = 640;
        public static int ScreenHeight = 360;
        public static int ScreenTilesHor = ScreenWidth / BASE_TILE_SIZE;
        public static int ScreenTilesVer = ScreenHeight / BASE_TILE_SIZE;
        public static int ScreenCenterX = ScreenWidth / 2;
        public static int ScreenCenterY = ScreenHeight / 2;
        public static int GameGraphicsScale = 1;

        public static World world = new(512, 512, 0);
        public static Player player = new(world)
        {
            X = world.Width / 2,
            Y = world.Height / 2,
        };

        public static ulong Tick;
        private static int debugMenuEntries = 0;
        private static bool miniTiles = false;

        public static void AddDebugMenuEntry(string entry)
        {
            int fontSize = 10 * GameGraphicsScale;
            int padding = 2 * GameGraphicsScale;
            for (int ox = -1; ox <= 1; ox++)
            {
                for (int oy = -1; oy <= 1; oy++)
                {
                    Raylib.DrawText(entry, padding + ox * GameGraphicsScale, padding + (padding + fontSize) * debugMenuEntries + oy * GameGraphicsScale, fontSize, Color.Black);
                }
            }
            Raylib.DrawText(entry, padding, padding + (padding + fontSize) * debugMenuEntries, fontSize, Color.Magenta);
            debugMenuEntries++;
        }

        public static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.ResizableWindow | ConfigFlags.FullscreenMode);

            Raylib.InitWindow(ScreenWidth, ScreenHeight, $"Trosečník {VER_STRING}");

            Raylib.SetExitKey(KeyboardKey.Null);

            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                ScreenWidth = Raylib.GetScreenWidth();
                ScreenHeight = Raylib.GetScreenHeight();

                GameGraphicsScale = Math.Max(1, Math.Min(ScreenWidth / 640, ScreenHeight / 360));
                world.TileSize = miniTiles ? 1 : (GameGraphicsScale * BASE_TILE_SIZE);

                ScreenTilesHor = ScreenWidth / world.TileSize;
                ScreenTilesVer = ScreenHeight / world.TileSize;

                ScreenCenterX = ScreenWidth / 2;
                ScreenCenterY = ScreenHeight / 2;

                // --- UPDATE LOGIC ---

                Vector2 mousePosition = Raylib.GetMousePosition();
                Vector2 mouseWorldPosition = new((float) Math.Round((mousePosition.X - ScreenCenterX) / world.TileSize) + player.X, (float) Math.Round((mousePosition.Y - ScreenCenterY) / world.TileSize) + player.Y);

                if (Raylib.IsKeyPressed(KeyboardKey.F11))
                {
                    Raylib.ToggleFullscreen();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F2))
                {
                    miniTiles = !miniTiles;
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    player.PlayerPathfinder.SetStart(player.X, player.Y);
                    player.PlayerPathfinder.SetTarget((int) mouseWorldPosition.X, (int) mouseWorldPosition.Y);
                    player.PlayerPathfinder.Recalculate();
                }

                player.Update(Tick);

                // --- DRAW LOGIC HERE ---
                debugMenuEntries = 0;

                Raylib.BeginDrawing();
                
                Raylib.ClearBackground(Color.Black);

                world.Draw(player.X, player.Y, Tick);

                player.Draw(world.TileSize);

                AddDebugMenuEntry($"FPS: {Raylib.GetFPS()}");
                string mtString = miniTiles ? " [MT]" : "";

                Raylib.DrawRectangleLinesEx(new Rectangle(((mouseWorldPosition.X - player.X) * world.TileSize) + ScreenCenterX - world.TileSize / 2, ((mouseWorldPosition.Y - player.Y) * world.TileSize) + ScreenCenterY - world.TileSize / 2, world.TileSize, world.TileSize), GameGraphicsScale, Color.RayWhite);

                AddDebugMenuEntry($"<PLAYER> X:{player.X} Y:{player.Y}");
                AddDebugMenuEntry($"<WORLD> W:{world.Width} H:{world.Height}");
                AddDebugMenuEntry($"<VIEWPORT> TW:{ScreenTilesHor} TH:{ScreenTilesVer}{mtString}");
                AddDebugMenuEntry($"<SCREEN> W:{ScreenWidth} H:{ScreenHeight} GGS:{GameGraphicsScale}");
                AddDebugMenuEntry($"<TILE> S-PX:{world.TileSize}");
                AddDebugMenuEntry($"<MOUSE> X:{mouseWorldPosition.X} Y:{mouseWorldPosition.Y}");

                Raylib.EndDrawing();

                Tick++;
            }

            Raylib.CloseWindow();
        }
    }
}
