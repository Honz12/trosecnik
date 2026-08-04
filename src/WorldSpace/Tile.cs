using System.Numerics;

namespace trosecnik.src.WorldSpace
{
    public interface ITile
    {
        public Vector2 GetTextureAltlasCoords(ulong tick);
        public bool GetWalkable();
    }
}
