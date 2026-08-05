using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class BowlWithDirtyWaterItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "bowlWithDirtyWater";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.NoInteraction;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            throw new NotImplementedException();
        }
    }
}
