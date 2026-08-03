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
        public string? GetTextureAbove(ulong tick);
        public Vector2 GetPosition(ulong tick);

        /// <summary>
        /// Gets the tilesize multiplier of the entity, new(3, 3) would be a 3x3 tile entity.
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public Vector2 GetTextureSize(ulong tick);
        public void Update(Player player, World world, ulong tick);
        public EntityRequest GetRequest();
        public void SetPos(Vector2 position);
    }
}
