using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public abstract class SimpleEntityBase : IEntity
    {
        public bool Killed = false;

        protected Vector2 Position;

        public Vector2 GetPosition(ulong tick)
        {
            return Position;
        }

        public IEntity.EntityRequest GetRequest()
        {
            if (Killed)
            {
                return IEntity.EntityRequest.SelfDelete;
            }
            return IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return GetTexturePath(tick);
        }

        abstract protected string GetTexturePath(ulong tick);

        public void SetPos(Vector2 position)
        {
            Position = position;
        }

        public void Update(Player player, World world, ulong tick)
        {
            SimpleUpdate(player, world, tick);
        }

        abstract protected void SimpleUpdate(Player player, World world, ulong tick);

        public Vector2 GetTextureSize(ulong tick)
        {
            return new(1, 1);
        }
    }
}