namespace trosecnik.src.World.Tiles
{
    public class VoidTile : ITile
    {
        public uint GetTextureId()
        {
            return 1;
        }

        public bool GetWalkable()
        {
            return true;
        }
    }
}
