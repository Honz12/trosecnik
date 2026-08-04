using System.Numerics;
using trosecnik.src.InventorySpace;

namespace trosecnik.src.WorldSpace.Entities
{
    public class ItemDropEntity : IEntity
    {
        private Vector2 Position;
        private bool PickedUp;
        private IItem Item;

        public ItemDropEntity(IItem item)
        {
            Item = item;
        }

        public Vector2 GetPosition(ulong tick)
        {
            return new(
                Position.X,
                (float) (Position.Y + Math.Sin(tick / 10.0) * 0.1)
            );
        }

        public IEntity.EntityRequest GetRequest()
        {
            return PickedUp ? IEntity.EntityRequest.SelfDelete : IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return ItemData.GetTexture(Item.GetItemId());
        }

        public string? GetTextureAbove(ulong tick)
        {
            return null;
        }

        public Vector2 GetTextureSize(ulong tick)
        {
            return new(1, 1);
        }

        public void SetPos(Vector2 position)
        {
            Position = position;
        }

        public void Update(Player player, World world, ulong tick, float deltaTime)
        {
            if (player.X == Position.X && player.Y == Position.Y && !PickedUp)
            {
                if (!player.PlayerInventory.AddItem(Item))
                {
                    SoundManager.Play("player/pickupItem/pickupItem1.wav");
                    PickedUp = true;
                }
            }
        }
    }
}