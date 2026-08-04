using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class WoodenWallItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "woodenWall";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (
                Program.world.GetWalkable((int) position.X, (int) position.Y)
                &&
                Program.world.GetTileLayer2((int) position.X, (int) position.Y) == null
            )
            {
                if (Program.world.SetTileLayer2(new WorldSpace.Tiles.WoodenWallTile(), (int) position.X, (int) position.Y))
                {
                    Program.player.PlayerInventory.RemoveItem(idx);
                }
            }
        }
    }
}
