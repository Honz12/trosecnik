using System.Numerics;
using trosecnik.src.InventorySpace.Items;

namespace trosecnik.src.WorldSpace.Entities
{
    public class TreeEntity : IEntity
    {
        private static readonly Random Rng = new();

        private Vector2 Position;
        private int Health = 10;
        private bool Broken = false;

        Vector2 shake = new();

        public Vector2 GetPosition(ulong tick)
        {
            return new(
                Position.X - 1 + shake.X,
                Position.Y - 1 + shake.Y
            );
        }

        public IEntity.EntityRequest GetRequest()
        {
            if (Broken)
            {
                Program.world.interactableEntities.Remove(Position);
                return IEntity.EntityRequest.SelfDelete;
            }
            else
            {
                return IEntity.EntityRequest.None;
            }
        }

        public string GetTexture(ulong tick)
        {
            return "entities/tree_0001.png";
        }

        public string? GetTextureAbove(ulong tick)
        {
            return "entities/tree_0002.png";
        }

        public Vector2 GetTextureSize(ulong tick)
        {
            return new(3, 3);
        }

        public void SetPos(Vector2 position)
        {
            Position = position;
        }

        public void Update(Player player, World world, ulong tick)
        {
            world.EntityBlockTile((int) Position.X, (int) Position.Y);
            shake.X *= -0.3f;
            shake.Y *= -0.3f;
        }

        public void Tree_Drop()
        {
            ItemDropEntity log = new(new WoodenLogItem());

            log.SetPos(new (
                (int) (Position.X + (Rng.Next(2) * 2 - 1)),
                (int) (Position.Y + (Rng.Next(2) * 2 - 1))
            ));

            Program.world.AddEntity(log);
        }

        public void Tree_Hit()
        {
            SoundManager.Play($"entity/tree/chop{Rng.Next(6) + 1}.wav");

            Health--;

            if (Rng.Next(5) == 0) Tree_Drop();

            if (Health <= 0)
            {
                Tree_Drop();
                Broken = true;
            }

            shake.X = (float) Rng.NextDouble() * 0.6f - 0.3f;
            shake.Y = (float) Rng.NextDouble() * 0.6f - 0.3f;
        }
    }
}