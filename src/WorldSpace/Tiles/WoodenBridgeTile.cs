using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class WoodenBridgeTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(ulong tick)
        {
            return new(0, 2);
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
