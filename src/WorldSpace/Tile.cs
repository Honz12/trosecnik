using System.Numerics;

namespace trosecnik.src.WorldSpace
{
    public interface ITile
    {
        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick);
        public bool GetWalkable();
    }
}
