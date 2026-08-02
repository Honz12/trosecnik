namespace trosecnik.src.World
{
    public interface ITile
    {
        public uint GetTextureId(ulong tick);
        public bool GetWalkable();
    }
}
