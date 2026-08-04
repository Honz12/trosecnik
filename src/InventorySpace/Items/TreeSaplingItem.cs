using System.Numerics;
using trosecnik.src.WorldSpace.Tiles;

namespace trosecnik.src.InventorySpace.Items
{
    public class TreeSaplingItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "treeSapling";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public string GetTexture()
        {
            return "items/item_0007.png";
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (Program.world.interactableEntities.ContainsKey(position)) return;
            if (Program.player.X == position.X && Program.player.Y == position.Y) return;
            if (Program.world.GetTileLayer1((int) position.X, (int) position.Y) is not GrassTile) return;
            if (!Program.world.GetWalkable((int) position.X, (int) position.Y)) return;
            WorldSpace.Entities.TreeSaplingEntity sapling = new();
            sapling.SetPos(position);
            Program.world.AddEntity(sapling);
            Program.player.PlayerInventory.RemoveItem(idx);
        }
    }
}