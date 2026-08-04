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
        public float Food = 100;
        public float Health = 100;

        public void Draw()
        {
            DrawBar(TransalationServer.GetTransalated("hudBarHealth"), Health, 0, Color.Red);
            DrawBar(TransalationServer.GetTransalated("hudBarHunger"), Food, 1, Color.Orange);
            DrawBar(TransalationServer.GetTransalated("hudBarThirst"), Water, 2, Color.Blue);
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

            Program.DrawCustomText(
                label,
                origin.X + (BAR_HEIGHT - Program.DEFAULT_FONT_SIZE) / 2 * Program.GameGraphicsScale,
                origin.Y + (BAR_HEIGHT - Program.DEFAULT_FONT_SIZE) / 2 * Program.GameGraphicsScale,
                Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                Color.White
            );

            Raylib.DrawRectangle(
                (int) origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                (int) origin.Y,
                BAR_WIDTH * Program.GameGraphicsScale,
                BAR_HEIGHT * Program.GameGraphicsScale,
                new Color(64, 64, 64, 255)
            );

            Raylib.DrawRectangle(
                (int) origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                (int) origin.Y,
                (int) (BAR_WIDTH * Program.GameGraphicsScale * (value / 100)),
                BAR_HEIGHT * Program.GameGraphicsScale,
                color
            );

            Raylib.DrawRectangle(
                (int) origin.X + LABEL_WIDTH * Program.GameGraphicsScale,
                (int) origin.Y + BAR_HEIGHT * Program.GameGraphicsScale / 2,
                BAR_WIDTH * Program.GameGraphicsScale,
                BAR_HEIGHT * Program.GameGraphicsScale / 2,
                new Color(0, 0, 0, 64)
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

            Program.DrawCustomText(
                $"{(int) value}%",
                origin.X + LABEL_WIDTH * Program.GameGraphicsScale + (BAR_HEIGHT - Program.DEFAULT_FONT_SIZE) / 2 * Program.GameGraphicsScale,
                origin.Y + (BAR_HEIGHT - Program.DEFAULT_FONT_SIZE) / 2 * Program.GameGraphicsScale,
                Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                Color.White
            );
        }
    }
}
