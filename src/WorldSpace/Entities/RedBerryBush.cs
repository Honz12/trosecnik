using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public class RedBerryBush : IEntity
    {
        private Vector2 Position;
        private static readonly Random Rng = new();
        private double WaitTime = GetRandomWaitTime();
        private bool Broken = false;
        private bool Grown = false;

        private static double GetRandomWaitTime()
        {
            return Rng.NextDouble() * 30 + 60;
        }

        public Vector2 GetPosition(ulong tick)
        {
            return Position;
        }

        public IEntity.EntityRequest GetRequest()
        {
            return Broken ? IEntity.EntityRequest.SelfDelete : IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return Grown ? "entities/redBerryBush/redBerryBush_0002.png" : "entities/redBerryBush/redBerryBush_0001.png";
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
            SoundManager.Play("player/pickupItem/pickupItem1.wav");
            // assets/audio/player/dropItem/dropItem1.wav
            // assets\audio\player\dropItem\dropItem1.wav
            Program.world.interactableEntities.Add(position, this);
        }

        public void Update(Player player, World world, ulong tick, float deltaTime)
        {
            world.EntityBlockTile((int) Position.X, (int) Position.Y);
            WaitTime -= deltaTime;

            if (WaitTime <= 0 && !Grown)
            {
                SoundManager.Play("entity/redBerryBush/grow1.wav");
                Grown = true;
            }
        }

        public void Interact()
        {
            if (Grown)
            {
                SoundManager.Play("player/dropItem/dropItem1.wav");
                Program.DropItem(
                    new (
                        (int) (Position.X + (Rng.Next(2) * 2 - 1)),
                        (int) (Position.Y + (Rng.Next(2) * 2 - 1))
                    ), 
                    new InventorySpace.Items.RedBerriesItem()
                );
                WaitTime = GetRandomWaitTime();
                Grown = false;
            }
        }

        public bool IsInteractable()
        {
            return true;
        }

        public void Bush_Break()
        {
            if (Grown)
            {
                Interact();
                return;
            }
            SoundManager.Play("player/dropItem/dropItem1.wav");
            Program.DropItem(
                Position,
                new InventorySpace.Items.BerryBushItem()
            );
            Broken = true;
            Program.world.interactableEntities.Remove(Position);
        }

        public void Bush_Grow()
        {
            Grown = true;
        }
    }
}
