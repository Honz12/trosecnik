using System.Numerics;
using trosecnik.src.WorldSpace.Tiles;

namespace trosecnik.src.InventorySpace.Items
{
    public class BowlItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "bowl";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (Program.world.GetTileLayer1((int) position.X, (int) position.Y) is WaterTile)
            {
                Program.player.PlayerInventory.RemoveItem(idx);
                Program.player.PlayerInventory.AddItem(new BowlWithDirtyWaterItem());
            }
        }
    }
}
