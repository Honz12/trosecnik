using System.Numerics;

namespace trosecnik.src.InventorySpace
{
    public interface IItem
    {
        public enum ItemType
        {
            NoInteraction, Consumable, Placeable
        }

        public string GetItemId();

        public ItemType GetItemType();
        
        /// <summary>
        /// Consumes an item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully been consumed</returns>
        public void ConsumeItem(Player player, int idx);

        /// <summary>
        /// Drops the item on the ground.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully dropped</returns>
        public void PlaceItem(Vector2 position, int idx);
    }

    public static class ItemData
    {
        private static Dictionary<string, string> textures = new()
        {
            { "redBerries", "items/item_0003.png" },
            { "stoneAxe", "items/item_0004.png" },
            { "woodenBridge", "items/item_0005.png" },
            { "woodenLog", "items/item_0006.png" },
            { "treeSapling", "items/item_0007.png" },
        };
        
        public static string GetTexture(string id)
        {
            return textures[id];
        }
    }
}
