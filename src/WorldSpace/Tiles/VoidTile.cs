using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class VoidTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick)
        {
            return new(0, 0);
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
