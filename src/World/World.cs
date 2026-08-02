namespace trosecnik.src.World
{
    public class World
    {
        ITile[,] tiles;
        public int Width;
        public int Height;

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            tiles = new Tiles.VoidTile[width, height];
        }

        public void Draw()
        {
            // Draw all the tiles with the `assets/tile_{id:0>4}.png` (python formating) filename

            for(int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    
                }
            }
        }
    }
}
