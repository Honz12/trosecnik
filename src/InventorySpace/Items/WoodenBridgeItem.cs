using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class WoodenBridgeItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "woodenBridge";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public string GetTexture()
        {
            return "items/item_0005.png";
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (
                Program.world.GetTileLayer1((int) position.X, (int) position.Y) is WorldSpace.Tiles.WaterTile
                &&
                Program.world.GetTileLayer2((int) position.X, (int) position.Y) == null
            )
            {
                if (Program.world.SetTileLayer2(new WorldSpace.Tiles.WoodenBridgeTile(), (int) position.X, (int) position.Y))
                {
                    Program.player.PlayerInventory.DeleteItem(idx);
                }
            }
        }
    }
}
