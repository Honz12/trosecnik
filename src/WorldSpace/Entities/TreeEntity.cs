using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public class TreeEntity : IEntity
    {
        private Vector2 Position;

        public Vector2 GetPosition(ulong tick)
        {
            return new(
                Position.X - 1,
                Position.Y - 1
            );
        }

        public IEntity.EntityRequest GetRequest()
        {
            return IEntity.EntityRequest.None;
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
        }
    }
}