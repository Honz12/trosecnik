using System.Numerics;
using trosecnik.src.WorldSpace.Tiles;

namespace trosecnik.src.InventorySpace.Items
{
    public class CampfireItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "campfire";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (Program.world.interactableEntities.ContainsKey(position)) return;
            if (Program.player.X == position.X && Program.player.Y == position.Y) return;
            if (!Program.world.GetWalkable((int) position.X, (int) position.Y)) return;
            WorldSpace.Entities.CampfireEntity campfire = new();
            campfire.SetPos(position);
            Program.world.AddEntity(campfire);
            Program.player.PlayerInventory.RemoveItem(idx);
        }
    }
}