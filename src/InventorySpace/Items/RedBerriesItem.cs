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

        public override void DropItem(Vector2 position, int idx)
        {
            throw new NotImplementedException();
        }

        public override string GetDisplayName()
        {
            return TransalationServer.GetTransalated("itemRedBerries");
        }

        public override string GetTexture()
        {
            return "items/item_0003.png";
        }
    }
}
