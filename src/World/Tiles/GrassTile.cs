namespace trosecnik.src.World.Tiles
{
    public class GrassTile : ITile
    {
        public uint GetTextureId(ulong tick)
        {
            return 10;
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
