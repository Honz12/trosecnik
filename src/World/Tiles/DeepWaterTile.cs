namespace trosecnik.src.World.Tiles
{
    public class DeepWaterTile : ITile
    {
        public uint GetTextureId(ulong tick)
        {
            return 12u + (uint) (tick / 30 % 8);
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
