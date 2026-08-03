namespace trosecnik.src.WorldSpace
{
    public interface ITile
    {
        public uint GetTextureId(ulong tick);
        public bool GetWalkable();
    }
}
