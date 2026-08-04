using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class SandTile : ITile
    {
        public Vector2 GetTextureAltlasCoords(ulong tick)
        {
            return new(2, 0);
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
