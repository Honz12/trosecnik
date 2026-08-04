using System.Numerics;
using Raylib_cs;
using trosecnik.src.Data;

namespace trosecnik.src.InventorySpace
{
    public class Inventory
    {
        public const int INVENTORY_SIZE = 30;
        public const int ITEM_SIZE_PX = 16;
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
                Items.Sort((a, b) => a.GetItemId().CompareTo(b.GetItemId()));
                return false;
            }
            return true;
        }

        public void RemoveItem(int idx)
        {
            Items.RemoveAt(idx);
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

                if (item != null)
                {
                    Texture2D itemTexture = TextureManager.GetTexture(ItemData.GetTexture(item.GetItemId()));

                    Raylib.DrawTexturePro(
                        itemTexture,
                        new(
                            0, 0,
                            ITEM_SIZE_PX, ITEM_SIZE_PX
                        ),
                        new(
                            x * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginX + (SLOT_SIZE_PX - ITEM_SIZE_PX) / 2 * Program.GameGraphicsScale,
                            y * SLOT_SIZE_PX * Program.GameGraphicsScale + menuOriginY + (SLOT_SIZE_PX - ITEM_SIZE_PX) / 2 * Program.GameGraphicsScale,
                            ITEM_SIZE_PX * Program.GameGraphicsScale,
                            ITEM_SIZE_PX * Program.GameGraphicsScale
                        ),
                        Vector2.Zero,
                        0.0f,
                        Color.White
                    );
                }
            }

            if (Selected < Items.Count)
            {
                IItem item = Items[Selected];

                string itemId = item.GetItemId();
                string displayName = TransalationServer.GetTransalated($"item-{itemId}");
                string intDesc = TransalationServer.GetTransalated($"itemIntDesc-{itemId}");
                
                int fontSize = Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale;
                Program.DrawCustomText(
                    displayName,
                    menuOriginX, menuOriginY - fontSize - Program.GameGraphicsScale,
                    fontSize,
                    Color.Black
                );

                if (item.GetItemType() != IItem.ItemType.NoInteraction)
                {
                    Program.DrawCustomText(
                        intDesc,
                        Program.ScreenCenterX - Raylib.MeasureTextEx(Program.font, intDesc, fontSize, Program.fontSpacing).X / 2,
                        Program.ScreenHeight - 8 * Program.GameGraphicsScale - fontSize - Program.GameGraphicsScale,
                        fontSize,
                        Color.Black
                    );
                }
            }
        }

        public void Update(Player player, Vector2 interactionPosition)
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
            if (Selected < Items.Count)
            {
                IItem item = Items[Selected];
                
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    switch (item.GetItemType())
                    {
                        case IItem.ItemType.Consumable:
                            item.ConsumeItem(player, Selected);
                            break;
                        case IItem.ItemType.Placeable:
                            if (
                                new Vector2(
                                    interactionPosition.X - player.X,
                                    interactionPosition.Y - player.Y
                                ).LengthSquared() < 4
                            ) item.PlaceItem(interactionPosition, Selected);
                            break;
                        default:
                            break;
                    }
                }
                if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                {
                    if (
                        new Vector2(
                            interactionPosition.X - player.X,
                            interactionPosition.Y - player.Y
                        ).LengthSquared() < 9
                        && Program.world.GetWalkable((int) interactionPosition.X, (int) interactionPosition.Y)
                        && (interactionPosition.X != player.X || interactionPosition.Y != player.Y)
                    )
                    {
                        WorldSpace.Entities.ItemDropEntity itemDrop = new(item);

                        itemDrop.SetPos(interactionPosition);

                        Program.world.AddEntity(itemDrop);

                        player.PlayerInventory.RemoveItem(Selected);

                        SoundManager.Play("player/dropItem/dropItem1.wav");
                    }
                }
            }
        }

        public List<IItem> GetItems()
        {
            return Items;
        }
    }
}
