using System.Numerics;

namespace trosecnik.src.WorldSpace
{
    public interface IEntity
    {
        public enum EntityRequest
        {
            None, SelfDelete
        }

        public string GetTexture(ulong tick);
        public Vector2 GetPosition(ulong tick);
        public void Update(Player player, World world, ulong tick);
        public EntityRequest GetRequest();
        public void SetPos(Vector2 position);
    }
}
