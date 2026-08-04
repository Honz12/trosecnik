using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class WaterTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick)
        {
            return new(tick / 10 % 8, 1);
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
