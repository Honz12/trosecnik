using System.Numerics;

namespace trosecnik.src.WorldSpace.Tiles
{
    public class WoodenBridgeTile : ITile
    {
        private static readonly AutotileTemplate autotileTemplate = new(new(0, 2));

        public Vector2 GetTextureAltlasCoords(Vector2 position, ulong tick)
        {
            return AutotileProcessor.GetAtlasCoords(autotileTemplate, AutotileProcessor.WorldTilemapLayer.Layer2, position, typeof(WoodenBridgeTile));
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
