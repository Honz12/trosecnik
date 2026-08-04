using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class GrassTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick)
        {
            return new(1, 0);
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
