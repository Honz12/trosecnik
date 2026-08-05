namespace trosecnik.src.InventorySpace.Items
{
    public class RedBerriesItem : ConsumableItemBase
    {
        public override void ConsumeAction(Player player)
        {
            player.Food += 5;
            player.Saturation = Player.MAX_SATURATION;
        }

        public override string GetConsumableItemId()
        {
            return "redBerries";
        }
    }
}
