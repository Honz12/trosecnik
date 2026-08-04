using System.Numerics;
using Raylib_cs;
using trosecnik.src.Data;
using trosecnik.src.InventorySpace;

namespace trosecnik.src.UI
{
    public class CraftingUI
    {
        public enum WorkstationPrivilige
        {
            HandCrafting,
            Workbench,
            Furnace,
            Anvil,
        }

        private Player player;

        private Dictionary<string, int> items = new();
        private int Selected = 0;

        private bool FailedBefore = false;

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

        public void Update()
        {
            if (Raylib.IsKeyPressedRepeat(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.Up))
            {
                Selected--;
            }
            if (Raylib.IsKeyPressedRepeat(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.Down))
            {
                Selected++;
            }
            if (
                Raylib.IsKeyPressedRepeat(KeyboardKey.Space) || Raylib.IsKeyPressed(KeyboardKey.Space) ||
                Raylib.IsKeyPressedRepeat(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Enter)
            )
            {
                TryToCraft();
            }
            Selected += CraftingRecipes.GetRecipes().Length;
            Selected %= CraftingRecipes.GetRecipes().Length;
        }

        public void TryToCraft()
        {
            CraftingRecipes.Recipe recipe = CraftingRecipes.GetRecipes()[Selected];

            bool canCraft = true;

            foreach (CraftingRecipes.ItemEntry ingredient in recipe.Ingredients)
            {
                items.TryGetValue(ingredient.ItemId, out int hasCount);

                if (hasCount < ingredient.Count)
                {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft)
            {
                SoundManager.Play("player/craft/success1.wav");

                Dictionary<string, int> remainingToRemove = new();
                foreach (CraftingRecipes.ItemEntry ingredient in recipe.Ingredients)
                {
                    if (remainingToRemove.ContainsKey(ingredient.ItemId))
                    {
                        remainingToRemove[ingredient.ItemId] += ingredient.Count;
                    }
                    else
                    {
                        remainingToRemove.Add(ingredient.ItemId, ingredient.Count);
                    }
                }

                List<IItem> inventoryItems = new(player.PlayerInventory.GetItems());
                for (int idx = inventoryItems.Count - 1; idx >= 0; idx--)
                {
                    string itemId = inventoryItems[idx].GetItemId();

                    if (remainingToRemove.TryGetValue(itemId, out int remaining) && remaining > 0)
                    {
                        player.PlayerInventory.RemoveItem(idx);
                        remainingToRemove[itemId] = remaining - 1;
                    }
                }

                for (int count = 0; count < recipe.Result.Count; count++)
                {
                    IItem resultItem = ItemData.GetItemEntryData(recipe.Result.ItemId).CreateInstance();
                    player.PlayerInventory.AddItem(resultItem);
                }

                Recalculate();
                FailedBefore = false;
            }
            else
            {
                if (!FailedBefore) SoundManager.Play("player/craft/fail1.wav");
                FailedBefore = true;
            }
        }

        public void Draw()
        {
            const int ITEM_ENTRY_MARGIN = 4;
            const int ITEM_COUNT_PANEL_WIDTH = 120;
            const int MENU_WIDTH = 480;
            const int MENU_HEIGHT = 340;

            int menuWidth = MENU_WIDTH * Program.GameGraphicsScale;
            int menuHeight = MENU_HEIGHT * Program.GameGraphicsScale;
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

                Texture2D texture = TextureManager.GetTexture(ItemData.GetTexture(id));

                Raylib.DrawTexturePro(
                    texture,
                    new(0, 0, texture.Width, texture.Height),
                    new(
                        menuOriginX + ITEM_ENTRY_MARGIN * Program.GameGraphicsScale,
                        menuOriginY + (ITEM_ENTRY_MARGIN + i * (Program.DEFAULT_FONT_SIZE + ITEM_ENTRY_MARGIN)) * Program.GameGraphicsScale,
                        Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale,
                        Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale
                    ),
                    Vector2.Zero,
                    0.0f,
                    Color.White
                );

                Program.DrawCustomText(
                    TransalationServer.GetTransalated($"{count}"),
                    menuOriginX + Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale + ITEM_ENTRY_MARGIN * 2 * Program.GameGraphicsScale,
                    menuOriginY + (ITEM_ENTRY_MARGIN + i * (Program.DEFAULT_FONT_SIZE + ITEM_ENTRY_MARGIN)) * Program.GameGraphicsScale + Program.GameGraphicsScale,
                    Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                    Color.Black
                );

                Raylib.DrawLineEx(
                    new(
                        menuOriginX + ITEM_COUNT_PANEL_WIDTH * Program.GameGraphicsScale,
                        menuOriginY
                    ),
                    new(
                        menuOriginX + ITEM_COUNT_PANEL_WIDTH * Program.GameGraphicsScale,
                        menuOriginY + MENU_HEIGHT * Program.GameGraphicsScale
                    ),
                    Program.GameGraphicsScale,
                    Color.Black
                );

                CraftingRecipes.Recipe[] recipes = CraftingRecipes.GetRecipes();

                for (int idx = 0; idx < recipes.Length; idx++)
                {
                    CraftingRecipes.Recipe recipe = recipes[idx];

                    Vector2 startPos = new(
                        menuOriginX + (ITEM_COUNT_PANEL_WIDTH + ITEM_ENTRY_MARGIN) * Program.GameGraphicsScale,
                        menuOriginY + ITEM_ENTRY_MARGIN * Program.GameGraphicsScale
                    );

                    if (idx == Selected)
                    {
                        Raylib.DrawRectangle(
                            (int) startPos.X - Program.GameGraphicsScale,
                            (int) startPos.Y - Program.GameGraphicsScale + (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * idx * Program.GameGraphicsScale,
                            menuWidth - (ITEM_COUNT_PANEL_WIDTH + ITEM_ENTRY_MARGIN * 2) * Program.GameGraphicsScale + 2 * Program.GameGraphicsScale,
                            Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale + 2 * Program.GameGraphicsScale,
                            new Color(0xda, 0xe1, 0xe5)
                        );
                    }

                    for (int ingIdx = 0; ingIdx < recipe.Ingredients.Length; ingIdx++)
                    {
                        CraftingRecipes.ItemEntry ingredient = recipe.Ingredients[ingIdx];

                        Texture2D itemTexture = TextureManager.GetTexture(ItemData.GetTexture(ingredient.ItemId));

                        Raylib.DrawTexturePro(
                            itemTexture,
                            new(0, 0, itemTexture.Width, itemTexture.Height),
                            new(
                                startPos + new Vector2(
                                    (Inventory.ITEM_SIZE_PX * 2 + ITEM_ENTRY_MARGIN) * ingIdx * Program.GameGraphicsScale,
                                    (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * idx * Program.GameGraphicsScale
                                ),
                                new(
                                    Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale,
                                    Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale
                                )
                            ),
                            Vector2.Zero,
                            0.0f,
                            Color.White
                        );
                        Program.DrawCustomText(
                            $"{ingredient.Count}",
                            startPos.X + ((Inventory.ITEM_SIZE_PX * 2 + ITEM_ENTRY_MARGIN) * ingIdx + Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * Program.GameGraphicsScale,
                            startPos.Y + ((Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * idx + 1) * Program.GameGraphicsScale,
                            Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                            Color.Black
                        );
                    }

                    Texture2D resultTexture = TextureManager.GetTexture(ItemData.GetTexture(recipe.Result.ItemId));

                    Raylib.DrawTexturePro(
                        resultTexture,
                        new(0, 0, resultTexture.Width, resultTexture.Height),
                        new(
                            new(
                                menuOriginX + menuWidth - (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * 2 * Program.GameGraphicsScale,
                                menuOriginY + (ITEM_ENTRY_MARGIN + (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * idx) * Program.GameGraphicsScale
                            ),
                            new(
                                Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale,
                                Inventory.ITEM_SIZE_PX * Program.GameGraphicsScale
                            )
                        ),
                        Vector2.Zero,
                        0.0f,
                        Color.White
                    );
                    Program.DrawCustomText(
                        $"{recipe.Result.Count}",
                        menuOriginX + menuWidth - (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * Program.GameGraphicsScale,
                        menuOriginY + (ITEM_ENTRY_MARGIN + 1 + (Inventory.ITEM_SIZE_PX + ITEM_ENTRY_MARGIN) * idx) * Program.GameGraphicsScale,
                        Program.DEFAULT_FONT_SIZE * Program.GameGraphicsScale,
                        Color.Black
                    );
                }
            }
        }
    }
}