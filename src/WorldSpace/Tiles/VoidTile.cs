using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class VoidTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(ulong tick)
        {
            return new(0, 0);
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
