namespace trosecnik.src.WorldSpace.Tiles
{
    public class VoidTile : ITile
    {
        public uint GetTextureId(ulong tick)
        {
            return 1;
        }

        public bool GetWalkable()
        {
            return false;
        }
    }
}
