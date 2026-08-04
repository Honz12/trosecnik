using System.Numerics;
using Raylib_cs;
using trosecnik.src.UI;
using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public static class Program
    {
        public enum AppMode
        {
            Playing, YouDiedMenu
        }

        public const int BASE_TILE_SIZE = 16;
        public const int MINI_TILE_SIZE = 4;
        public const int DEFAULT_FONT_SIZE = 16;

        public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, Math.Max(0, count)));

        public const string VER_STRING = "0.2";

        public static int ScreenWidth = 640;
        public static int ScreenHeight = 360;
        public static int ScreenTilesHor = ScreenWidth / BASE_TILE_SIZE;
        public static int ScreenTilesVer = ScreenHeight / BASE_TILE_SIZE;
        public static int ScreenCenterX = ScreenWidth / 2;
        public static int ScreenCenterY = ScreenHeight / 2;
        public static int GameGraphicsScale = 1;

        public static World world = new(512, 512, 0);
        public static Player player = new(world.Width / 2, world.Height / 2, world);
        public static Camera camera = new()
        {
            X = player.X,
            Y = player.Y,
        };
        public static HUD hud = new();

        public static ulong Tick;
        private static int debugMenuEntries = 0;
        private static bool miniTiles = false;
        private static bool DebugShown = false;
        private static bool ShouldExit = false;

        public static AppMode appMode = AppMode.Playing;

        public static Font font;
        public static float fontSpacing = 0;

        public static void AddDebugMenuEntry(string entry)
        {
            int fontSize = DEFAULT_FONT_SIZE * GameGraphicsScale;
            int padding = 2 * GameGraphicsScale;
            for (int ox = -1; ox <= 1; ox++)
            {
                for (int oy = -1; oy <= 1; oy++)
                {
                    DrawCustomText(entry, padding + ox * GameGraphicsScale, padding + (padding + fontSize) * debugMenuEntries + oy * GameGraphicsScale, fontSize, Color.Black);
                }
            }
            DrawCustomText(entry, padding, padding + (padding + fontSize) * debugMenuEntries, fontSize, Color.Magenta);
            debugMenuEntries++;
        }

        public static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.ResizableWindow | ConfigFlags.FullscreenMode);

            Raylib.InitWindow(ScreenWidth, ScreenHeight, $"{TransalationServer.GetTransalated("gameName")} {VER_STRING}");
            Raylib.InitAudioDevice();
            
            Raylib.SetWindowMinSize(640, 360);
            Raylib.SetExitKey(KeyboardKey.Null);
            Raylib.SetTargetFPS(60);

            // Special characters
            string specialChars = "áčďéěíňóřšťúůýžÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ";

            // Create an array containing ASCII (32-126) + Czech characters
            int[] codepoints = Enumerable.Range(32, 95)
                .Concat(specialChars.Select(c => (int)c))
                .Distinct()
                .ToArray();

            // Raylib-cs accepts the array directly
            font = Raylib.LoadFontEx("assets/PXPLUS_IBM_VGA8.TTF", 32, codepoints, codepoints.Length);

            while (!Raylib.WindowShouldClose() && !ShouldExit)
            {
                float deltaTime = Raylib.GetFrameTime();

                ScreenWidth = Raylib.GetScreenWidth();
                ScreenHeight = Raylib.GetScreenHeight();

                GameGraphicsScale = Math.Max(1, Math.Min(ScreenWidth / 640, ScreenHeight / 360));
                world.TileSize = miniTiles ? (GameGraphicsScale * MINI_TILE_SIZE) : (GameGraphicsScale * BASE_TILE_SIZE);

                ScreenTilesHor = ScreenWidth / world.TileSize;
                ScreenTilesVer = ScreenHeight / world.TileSize;

                ScreenCenterX = ScreenWidth / 2;
                ScreenCenterY = ScreenHeight / 2;

                // --- UPDATE LOGIC ---

                Vector2 mousePosition = Raylib.GetMousePosition();
                Vector2 mouseWorldPosition = new((int) Math.Floor((mousePosition.X - ScreenCenterX) / (double) world.TileSize + camera.X + 0.5), (int) Math.Floor((mousePosition.Y - ScreenCenterY) / (double) world.TileSize + camera.Y + 0.5));

                if (Raylib.IsKeyPressed(KeyboardKey.F11))
                {
                    Raylib.ToggleFullscreen();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F1))
                {
                    if (TransalationServer.GetLanguage() == "cz")
                    {
                        TransalationServer.SetLanguage("eng");
                    }
                    else if (TransalationServer.GetLanguage() == "eng")
                    {
                        TransalationServer.SetLanguage("cz");
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F2))
                {
                    miniTiles = !miniTiles;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F3))
                {
                    DebugShown = !DebugShown;
                }

                AppUpdate(mouseWorldPosition, deltaTime);
                SoundManager.Update();

                debugMenuEntries = 0;

                // Render

                Raylib.BeginDrawing();

                AppRender(mouseWorldPosition);

                // Debug

                if (DebugShown)
                {
                    AddDebugMenuEntry($"FPS: {Raylib.GetFPS()}");
                    AddDebugMenuEntry($"Tick: {Tick}");
                    AddDebugMenuEntry($"Language: {TransalationServer.GetLanguage().ToUpper()}");
                    string mtString = miniTiles ? " [MT]" : "";

                    AddDebugMenuEntry($"Position - X:{player.X} Y:{player.Y} MoveWaitTime:{player.MoveWait}");
                    AddDebugMenuEntry($"Viewport - TilesHorizonzaly:{ScreenTilesHor} TilesVerticaly:{ScreenTilesVer}{mtString} TileSizeInPixels:{world.TileSize}");
                    AddDebugMenuEntry($"Screen - Width:{ScreenWidth} Height:{ScreenHeight} GameGraphicsScale:{GameGraphicsScale}");
                    AddDebugMenuEntry($"Cursor - WorldX:{mouseWorldPosition.X} WorldY:{mouseWorldPosition.Y}");
                    AddDebugMenuEntry($"Health:{player.Health} Hunger:{player.Hunger} Saturation:{player.Saturation:F2} Thirst:{player.Thirst} Thirsting:{player.Thirsting:F2}");
                    AddDebugMenuEntry($"World - Width:{world.Width} Height:{world.Height}");
                    AddDebugMenuEntry($"EntityCount:{world.GetEntityCount()} ClearingEntityTileBlocksColumns:{world.ClearingEntityTileBlocksColumns}");
                }

                Raylib.EndDrawing();

                Tick++;
            }

            TextureManager.UnloadAll();
            SoundManager.UnloadAll();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        private static void AppUpdate(Vector2 mouseWorldPosition, float deltaTime)
        {
            switch (appMode)
            {
                case AppMode.Playing:
                    {
                        if (Raylib.IsMouseButtonPressed(MouseButton.Right) && player.movementMode == Player.MovementMode.Pathfind)
                        {
                            player.PlayerPathfinder.SetStart(player.X, player.Y);
                            player.PlayerPathfinder.SetTarget((int) mouseWorldPosition.X, (int) mouseWorldPosition.Y);
                            player.PlayerPathfinder.Recalculate();
                        }

                        player.Update(Tick, mouseWorldPosition, deltaTime);
                        camera.X += (player.X - camera.X) * 0.1;
                        camera.Y += (player.Y - camera.Y) * 0.1;

                        world.UpdateEntites(player, Tick, deltaTime);
                    }
                    break;
                case AppMode.YouDiedMenu:
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Space))
                        {
                            ShouldExit = true;
                        }
                    }
                    break;
            }
        }

        private static void DrawScenePlaying(Vector2 mouseWorldPosition)
        {
            Raylib.ClearBackground(Color.Black);

            world.Draw(player, camera, Tick);

            for (int ox = -2; ox <= 2; ox++)
            {
                for (int oy = -2; oy <= 2; oy++)
                {
                    Raylib.DrawRectangleLinesEx(
                        new Rectangle(
                            (float) ((mouseWorldPosition.X - camera.X + ox) * world.TileSize) + ScreenCenterX - world.TileSize / 2,
                            (float) ((mouseWorldPosition.Y - camera.Y + oy) * world.TileSize) + ScreenCenterY - world.TileSize / 2,
                            world.TileSize, world.TileSize
                        ),
                        GameGraphicsScale,
                        (ox == 0 && oy == 0) ? new Color(255, 255, 255, 128) : ((Math.Abs(ox) == 2 || Math.Abs(oy) == 2) ? new Color(255, 255, 255, 16) : new Color(255, 255, 255, 64)));
                }
            }

            // HUD

            hud.Health = (int) ((double) player.Health * (100 / Player.MAX_HEALTH));
            hud.Hunger = (int) ((double) player.Hunger * (100 / Player.MAX_HUNGER));
            hud.Draw();
        }

        public static void DrawCustomText(string text, float x, float y, float fontSize, Color color)
        {
            Raylib.DrawTextEx(
                font, text, new Vector2(x, y), fontSize, fontSpacing, color
            );
        }

        private static void AppRender(Vector2 mouseWorldPosition)
        {
            switch (appMode)
            {
                case AppMode.Playing:
                    {
                        DrawScenePlaying(mouseWorldPosition);
                        player.PlayerInventory.Draw();
                    }
                    break;
                case AppMode.YouDiedMenu:
                    {
                        DrawScenePlaying(mouseWorldPosition);

                        Raylib.DrawRectangle(0, 0, ScreenWidth, ScreenHeight, new Color(0, 0, 0, 256 - 64));
                        
                        string text1 = TransalationServer.GetTransalated("deathMsg1");
                        string text2 = TransalationServer.GetTransalated("deathMsg2");

                        DrawCustomText(
                            text1,
                            ScreenCenterX - Raylib.MeasureTextEx(font, text1, 20 * GameGraphicsScale, fontSpacing).X / 2,
                            ScreenCenterY - 5 * GameGraphicsScale - 30 * GameGraphicsScale,
                            20 * GameGraphicsScale,
                            Color.Red
                        );
                        DrawCustomText(
                            text2,
                            ScreenCenterX - Raylib.MeasureTextEx(font, text2, 20 * GameGraphicsScale, fontSpacing).X / 2,
                            ScreenCenterY - 5 * GameGraphicsScale + 30 * GameGraphicsScale,
                            20 * GameGraphicsScale,
                            Color.Red
                        );
                    }
                    break;
            }
        }
    }
}
