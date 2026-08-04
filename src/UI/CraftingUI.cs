using Raylib_cs;
using trosecnik.src.InventorySpace;

namespace trosecnik.src.UI
{
    public class CraftingUI
    {
        private Player player;

        private Dictionary<string, int> items = new();

        public CraftingUI(Player player)
        {
            this.player = player;
            Recalculate();
        }

        public void Recalculate()
        {
            items.Clear();

            foreach (IItem item in player.PlayerInventory.GetItems())
            {
                string id = item.GetItemId();

                if (items.ContainsKey(id))
                {
                    items[id]++;
                }
                else
                {
                    items.Add(id, 1);
                }
            }
        }

        public void Draw()
        {
            const int ITEM_ENTRY_MARGIN = 4;

            int menuWidth = 420 * Program.GameGraphicsScale;
            int menuHeight = 220 * Program.GameGraphicsScale;
            int menuOriginX = Program.ScreenCenterX - menuWidth / 2;
            int menuOriginY = Program.ScreenCenterY - menuHeight / 2;
            Raylib.DrawRectangle(
                menuOriginX, menuOriginY, menuWidth, menuHeight,
                new Color(0xb3, 0xc2, 0xca)
            );
            Raylib.DrawRectangleLinesEx(
                new(menuOriginX, menuOriginY, menuWidth, menuHeight),
                Program.GameGraphicsScale,
                Color.Black
            );

            string[] keys = items.Keys.ToArray();

            for (int i = 0; i < items.Count; i++)
            {
                string id = keys[i];
                int count = items[id];

                Program.DrawCustomText(
                    $"{TransalationServer.GetTransalated($"item-{id}"),-20} {count,2}",
                    menuOriginX + ITEM_ENTRY_MARGIN * Program.GameGraphicsScale,
                    menuOriginY + (ITEM_ENTRY_MARGIN + i * (Program.DEFAULT_FONT_SIZE + ITEM_ENTRY_MARGIN)) * Program.GameGraphicsScale,
                    Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                    Color.Black
                );
            }
        }
    }
}