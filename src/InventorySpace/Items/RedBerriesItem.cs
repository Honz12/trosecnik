using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    class RedBerriesItem : IItem
    {
        public void ConsumeItem(Player player)
        {
            player.Hunger += 20;
            player.Saturation += 100;
        }

        public void DropItem(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName()
        {
            return "Cervené bobule";
        }

        public IItem.ItemType GetItemType()
        {
            return IItem.ItemType.Consumable;
        }

        public string GetTexture()
        {
            return "items/item_0003.png";
        }

        public void PlaceItem(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}