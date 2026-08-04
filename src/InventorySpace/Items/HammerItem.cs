using System.Numerics;
using trosecnik.src.WorldSpace;
using trosecnik.src.WorldSpace.Tiles;

namespace trosecnik.src.InventorySpace.Items
{
    public class HammerItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "hammer";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            ITile? target = Program.world.GetTileLayer2((int) position.X, (int) position.Y);

            if (target != null && (position.X != Program.player.X || position.Y != Program.player.Y))
            {
                if (target is WoodenBridgeTile)
                {
                    Program.DropItem(new(Program.player.X, Program.player.Y), new WoodenBridgeItem());
                    Program.world.SetTileLayer2(null, (int) position.X, (int) position.Y);
                }
                if (target is WoodenWallTile)
                {
                    Program.DropItem(new(Program.player.X, Program.player.Y), new WoodenWallItem());
                    Program.world.SetTileLayer2(null, (int) position.X, (int) position.Y);
                }
            }
        }
    }
}
