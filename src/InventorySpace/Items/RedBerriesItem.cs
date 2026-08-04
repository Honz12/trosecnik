using System.Numerics;

namespace trosecnik.src.InventorySpace.Items
{
    public class RedBerriesItem : ConsumableItemBase
    {
        public override void ConsumeAction(Player player)
        {
            player.Hunger += 20;
            player.Saturation = Player.MAX_SATURATION;
            player.Health += 5;
        }

        public override string GetConsumableItemId()
        {
            return "woodenLog";
        }

        public override string GetTexture()
        {
            return "items/item_0003.png";
        }
    }
}
