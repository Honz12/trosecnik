using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.UI
{
    public class HUD
    {
        private const int BAR_HEIGHT = 24;
        private const int BAR_MARGIN = 4;
        private const int LABEL_WIDTH = 58;
        private const int BAR_WIDTH = 128;

        public float Water = 100;
        public float Hunger = 100;
        public float Health = 100;

        public void Draw()
        {
            DrawBar(TransalationServer.GetTransalated("hudBarThirst"), Water, 0, Color.Blue);
            DrawBar(TransalationServer.GetTransalated("hudBarHunger"), Hunger, 1, Color.Orange);
            DrawBar(TransalationServer.GetTransalated("hudBarHealth"), Health, 2, Color.Red);
        }

        private static void DrawBar(string label, float value, int idx, Color color)
        {
            Vector2 origin = new(
                BAR_MARGIN * Program.GameGraphicsScale,
                (BAR_MARGIN + idx * (BAR_HEIGHT + BAR_MARGIN)) * Program.GameGraphicsScale
            );

            Raylib.DrawRectangle(
                (int) origin.X,
                (int) origin.Y,
                LABEL_WIDTH * Program.GameGraphicsScale,
                BAR_HEIGHT * Program.GameGraphicsScale,
                new Color(0, 0, 0, 128)
            );

            Raylib.DrawRectangle(
                (int) origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                (int) origin.Y,
                BAR_WIDTH * Program.GameGraphicsScale,
                BAR_HEIGHT * Program.GameGraphicsScale,
                new Color(64, 64, 64, 255)
            );

            Raylib.DrawRectangleLinesEx(
                new(
                    (int) origin.X,
                    (int) origin.Y,
                    (BAR_WIDTH + LABEL_WIDTH) * Program.GameGraphicsScale,
                    BAR_HEIGHT * Program.GameGraphicsScale
                ),
                Program.GameGraphicsScale,
                new Color(0, 0, 0, 255)
            );

            Raylib.DrawLineEx(
                new(
                    origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                    origin.Y
                ),
                new(
                    origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                    origin.Y + BAR_HEIGHT * Program.GameGraphicsScale
                ),
                Program.GameGraphicsScale,
                new Color(0, 0, 0, 255)
            );
        }
    }
}
