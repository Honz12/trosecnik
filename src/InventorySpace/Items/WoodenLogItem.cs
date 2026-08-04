using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class WoodenLogItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            player.PlayerInventory.RemoveItem(idx);
            player.PlayerInventory.AddItem(new WoodenBridgeItem());
        }

        public string GetItemId()
        {
            return "woodenLog";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Consumable;
        }

        public string GetTexture()
        {
            return "items/item_0006.png";
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            throw new NotImplementedException();
        }
    }
}