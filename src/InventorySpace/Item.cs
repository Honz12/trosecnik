using System.Numerics;

namespace trosecnik.src.InventorySpace
{
    public interface IItem
    {
        public enum ItemType
        {
            NoInteraction, Consumable, Placeable
        }

        public string GetTexture();

        public string GetDisplayName();

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
        public void DropItem(Vector2 position, int idx);

        /// <summary>
        /// Drops the item on the ground.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully dropped</returns>
        public void PlaceItem(Vector2 position, int idx);
    }
}
