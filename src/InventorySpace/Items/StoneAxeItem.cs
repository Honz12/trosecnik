using System.Net.Security;
using System.Numerics;
using trosecnik.src.WorldSpace;
using trosecnik.src.WorldSpace.Entities;

namespace trosecnik.src.InventorySpace.Items
{
    public class StoneAxeItem : IItem
    {
        public void ConsumeItem(Player player, int idx)
        {
            throw new NotImplementedException();
        }

        public string GetItemId()
        {
            return "stoneAxe";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Placeable;
        }

        public void PlaceItem(Vector2 position, int idx)
        {
            if (Program.world.interactableEntities.TryGetValue(position, out IEntity? entity))
            {
                var tree = entity as TreeEntity;
                var sapling = entity as TreeSaplingEntity;
                var berryBush = entity as RedBerryBush;

                tree?.Tree_Hit();
                sapling?.Sapling_Break();
                berryBush?.Bush_Break();
            }
        }
    }
}
