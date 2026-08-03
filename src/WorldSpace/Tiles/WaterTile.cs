namespace trosecnik.src.WorldSpace.Tiles
{
    public class WaterTile : ITile
    {
        public uint GetTextureId(ulong tick)
        {
            return 2u + (uint) (tick / 30 % 8);
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
