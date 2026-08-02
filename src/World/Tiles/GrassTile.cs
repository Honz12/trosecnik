namespace trosecnik.src.World.Tiles
{
    public class GrassTile : ITile
    {
        public uint GetTextureId()
        {
            return 10;
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
