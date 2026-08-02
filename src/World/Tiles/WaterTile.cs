namespace trosecnik.src.World.Tiles
{
    public class WaterTile : ITile
    {
        byte anim = 0;
        byte timer = 0;

        public uint GetTextureId()
        {
            return 2u + anim;
        }

        public bool GetWalkable()
        {
            return false;
        }

        public void UpdateRenderState()
        {
            timer++;
            if (timer % 15 != 0) return;
            anim = (byte) ((anim + 1) % 8);
        }
    }
}
