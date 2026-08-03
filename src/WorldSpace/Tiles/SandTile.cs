namespace trosecnik.src.WorldSpace.Tiles
{
    public class SandTile : ITile
    {
        public uint GetTextureId(ulong tick)
        {
            return 11;
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
