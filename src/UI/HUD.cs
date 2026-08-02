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
            DrawBar("Voda", Water, new Vector2(10, 10), Color.Blue);
            DrawBar("Hlad", Hunger, new Vector2(10, 34), Color.Orange);
            DrawBar("Zdraví", Health, new Vector2(10, 58), Color.Red);
        }

        private static void DrawBar(string label, float value, Vector2 position, Color color)
        {
            const int barWidth = 140;
            const int barHeight = 16;
            const int labelWidth = 60;

            Raylib.DrawRectangle((int)position.X, (int)position.Y, labelWidth, barHeight, new Color(0, 0, 0, 160));
            Raylib.DrawText(label, (int)position.X + 4, (int)position.Y + 3, 10, Color.White);

            int x = (int)position.X + labelWidth + 6;
            Raylib.DrawRectangle(x, (int)position.Y, barWidth + 2, barHeight + 2, Color.Black);
            Raylib.DrawRectangle(x + 1, (int)position.Y + 1, barWidth, barHeight, new Color(40, 40, 40, 255));

            float ratio = Math.Clamp(value / 100f, 0f, 1f);
            if (ratio > 0)
            {
                Raylib.DrawRectangle(x + 1, (int)position.Y + 1, (int)(barWidth * ratio), barHeight, color);
            }

            Raylib.DrawText($"{Math.Max(0, (int)value)}%", x + barWidth + 8, (int)position.Y + 3, 10, Color.White);
        }
    }
}
