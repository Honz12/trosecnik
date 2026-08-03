using System.Numerics;

namespace trosecnik.src.InventorySpace
{
    public interface IItem
    {
        public string GetTexture();

        public bool CanItemBeDropped();
        public bool CanItemBeConsumed();
        public bool CanItemBePlaced();
        
        /// <summary>
        /// Consumes an item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully been consumed</returns>
        public bool ConsumeItem(Player player);

        /// <summary>
        /// Drops the item on the ground.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully dropped</returns>
        public bool DropItem(Vector2 position);

        /// <summary>
        /// Drops the item on the ground.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>If the item has successfully dropped</returns>
        public bool PlaceItem(Vector2 position);
    }
}
