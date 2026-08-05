using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class BowlWithWaterItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            player.Water += 10;
            player.Thirsting = Player.MAX_THIRSTING;
            player.PlayerInventory.RemoveItem(idx);
            player.PlayerInventory.AddItem(new BowlItem());
        }

        public string GetItemId()
        {
            return "bowlWithWater";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Consumable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            throw new NotImplementedException();
        }
    }
}
