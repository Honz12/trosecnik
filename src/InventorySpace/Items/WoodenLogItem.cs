using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class WoodenLogItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "woodenLog";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.NoInteraction;
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