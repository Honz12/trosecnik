using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class WoodenWallTile : ITile
    {
        private static readonly AutotileTemplate autotileTemplate = new(new(4, 2));

        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick)
        {
            return AutotileProcessor.GetAtlasCoords(autotileTemplate, AutotileProcessor.WorldTilemapLayer.Layer2, position, typeof(WoodenWallTile));
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
