namespace trosecnik.src.World.Tiles
{
    public class SandTile : ITile
    {
        public uint GetTextureId()
        {
            return 11;
        }

        public bool GetWalkable()
        {
            return true;
        }

        public void UpdateRenderState()
        {
            return;
        }
    }
}
