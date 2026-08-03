using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public abstract class ConsumableItemBase : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            ConsumeAction(player);
            player.PlayerInventory.DeleteItem(idx);
        }
        public abstract void ConsumeAction(Player player);

        public abstract void DropItem(Vector2 position, int idx);

        public abstract string GetDisplayName();

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Consumable;
        }

        public abstract string GetTexture();

        public void PlaceItem(Vector2 position, int idx)
        {
            throw new NotImplementedException();
        }
    }
}
