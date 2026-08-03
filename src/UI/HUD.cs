using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.UI
{
    public class HUD
    {
        public float Water = 100;
        public float Hunger = 100;
        public float Health = 100;

        public void Draw()
        {
            DrawBar("Voda", Water, new Vector2(8 * Program.GameGraphicsScale, 8 * Program.GameGraphicsScale), Color.Blue);
            DrawBar("Hlad", Hunger, new Vector2(8 * Program.GameGraphicsScale, 32 * Program.GameGraphicsScale), Color.Orange);
            DrawBar("Zdraví", Health, new Vector2(8 * Program.GameGraphicsScale, 56 * Program.GameGraphicsScale), Color.Red);
        }

        private static void DrawBar(string label, float value, Vector2 position, Color color)
        {
            const int barWidth = 140;
            const int barHeight = 16;
            const int labelWidth = 60;

            Raylib.DrawRectangle((int)position.X, (int)position.Y, labelWidth * Program.GameGraphicsScale, (barHeight + 2) * Program.GameGraphicsScale, new Color(0, 0, 0, 160));
            Raylib.DrawText(label, (int)position.X + 4 * Program.GameGraphicsScale, (int)position.Y + 4 * Program.GameGraphicsScale, 10 * Program.GameGraphicsScale, Color.White);

            int x = (int)position.X + (labelWidth + 6) * Program.GameGraphicsScale;
            Raylib.DrawRectangle(x, (int)position.Y, (barWidth + 2) * Program.GameGraphicsScale, (barHeight + 2) * Program.GameGraphicsScale, Color.Black);
            Raylib.DrawRectangle(x + Program.GameGraphicsScale, (int)position.Y + Program.GameGraphicsScale, barWidth * Program.GameGraphicsScale, barHeight * Program.GameGraphicsScale, new Color(40, 40, 40, 255));

            float ratio = Math.Clamp(value / 100f, 0f, 1f);
            if (ratio > 0)
            {
                Raylib.DrawRectangle(x + Program.GameGraphicsScale, (int)position.Y + Program.GameGraphicsScale, (int)(barWidth * ratio) * Program.GameGraphicsScale, barHeight * Program.GameGraphicsScale, color);
            }

            Raylib.DrawText($"{Math.Max(0, (int)value)}%", x + (barWidth + 8) * Program.GameGraphicsScale, (int)position.Y + 4 * Program.GameGraphicsScale, 10 * Program.GameGraphicsScale, Color.White);
        }
    }
}
