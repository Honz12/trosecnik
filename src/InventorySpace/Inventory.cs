using System.Numerics;
using Raylib_cs;

namespace trosecnik.src.InventorySpace
{
    public class Inventory
    {
        public const int INVENTORY_SIZE = 30;
        private const int SLOTS_PER_ROW = 5;
        private const int SLOT_SIZE_PX = 22;

        private int Selected = 0;

        private List<IItem> Items = new();
        
        public Inventory()
        {
            
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns>TRUE if the inventory is full and the addition has failed.</returns>
        public bool AddItem(IItem item)
        {
            if (Items.Count < INVENTORY_SIZE)
            {
                Items.Add(item);
                return false;
            }
            return true;
        }

        public void Draw()
        {
            Texture2D slot = TextureManager.GetTexture("ui/inv_slot_0001.png");
            Texture2D slotSelected = TextureManager.GetTexture("ui/inv_slot_0002.png");

            int menuWidth = SLOTS_PER_ROW * SLOT_SIZE_PX * Program.GameGraphicsScale;
            int menuHeight = INVENTORY_SIZE / SLOTS_PER_ROW * SLOT_SIZE_PX * Program.GameGraphicsScale;

            int menuOriginX = 8 * Program.GameGraphicsScale;
            int menuOriginY = Program.ScreenHeight - 8 * Program.GameGraphicsScale - menuHeight;

            Raylib.DrawRectangleLinesEx(
                new(
                    menuOriginX - Program.GameGraphicsScale, menuOriginY - Program.GameGraphicsScale,
                    menuWidth + Program.GameGraphicsScale * 2, menuHeight + Program.GameGraphicsScale * 2
                ),
                Program.GameGraphicsScale,
                Color.Black
            );

            for (int i = 0; i < INVENTORY_SIZE; i++)
            {
                IItem? item = null;
                if (i < Items.Count)
                {
                    item = Items[i];
                }

                int x = i % SLOTS_PER_ROW;
                int y = i / SLOTS_PER_ROW;

                if (i == Selected)
                {
                    Raylib.DrawTexturePro(
                        slotSelected,
                        new(
                            0, 0,
                            SLOT_SIZE_PX, SLOT_SIZE_PX
                        ),
                        new(
                            x * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginX, y * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginY,
                            SLOT_SIZE_PX * Program.GameGraphicsScale, SLOT_SIZE_PX * Program.GameGraphicsScale
                        ),
                        Vector2.Zero,
                        0.0f,
                        Color.White
                    );
                }
                else
                {
                    Raylib.DrawTexturePro(
                        slot,
                        new(
                            0, 0,
                            SLOT_SIZE_PX, SLOT_SIZE_PX
                        ),
                        new(
                            x * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginX, y * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginY,
                            SLOT_SIZE_PX * Program.GameGraphicsScale, SLOT_SIZE_PX * Program.GameGraphicsScale
                        ),
                        Vector2.Zero,
                        0.0f,
                        Color.White
                    );
                }
            }
        }

        public void Update()
        {
            float scroll = Raylib.GetMouseWheelMove();

            if (scroll > 0)
            {
                Selected--;
            }
            else if (scroll < 0)
            {
                Selected++;
            }

            Selected += INVENTORY_SIZE;
            Selected %= INVENTORY_SIZE;
        }
    }
}
