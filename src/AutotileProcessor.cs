using System.Numerics;

namespace trosecnik.src
{
    public class AutotileTemplate
    {
        public enum Direction
        {
            // 0
            None,

            // 1
            Up, Down, Left, Right,

            // 2
            UpDown, LeftRight,
            UpLeft, UpRight, DownLeft, DownRight,

            // 3
            UpDownLeft, UpDownRight,
            UpLeftRight, DownLeftRight,

            // 4
            UpDownLeftRight
        }

        public AutotileTemplate() {}
        public AutotileTemplate(Vector2 originCoord)
        {
            SetCoords(Direction.DownRight, originCoord + new Vector2(0, 0));
            SetCoords(Direction.DownLeftRight, originCoord + new Vector2(1, 0));
            SetCoords(Direction.DownLeft, originCoord + new Vector2(2, 0));
            SetCoords(Direction.Down, originCoord + new Vector2(3, 0));

            SetCoords(Direction.UpDownRight, originCoord + new Vector2(0, 1));
            SetCoords(Direction.UpDownLeftRight, originCoord + new Vector2(1, 1));
            SetCoords(Direction.UpDownLeft, originCoord + new Vector2(2, 1));
            SetCoords(Direction.UpDown, originCoord + new Vector2(3, 1));

            SetCoords(Direction.UpRight, originCoord + new Vector2(0, 2));
            SetCoords(Direction.UpLeftRight, originCoord + new Vector2(1, 2));
            SetCoords(Direction.UpLeft, originCoord + new Vector2(2, 2));
            SetCoords(Direction.Up, originCoord + new Vector2(3, 2));

            SetCoords(Direction.Right, originCoord + new Vector2(0, 3));
            SetCoords(Direction.LeftRight, originCoord + new Vector2(1, 3));
            SetCoords(Direction.Left, originCoord + new Vector2(2, 3));
            SetCoords(Direction.None, originCoord + new Vector2(3, 3));
        }

        private Vector2[] Coords = new Vector2[16];

        public void SetCoords(Direction direction, Vector2 atlasCoords)
        {
            Coords[(int) direction] = atlasCoords;
        }

        public Vector2 GetCoords(Direction direction)
        {
            return Coords[(int) direction];
        }
    }

    public static class AutotileProcessor
    {
        public enum WorldTilemapLayer { Layer1, Layer2 }

        public static Vector2 GetAtlasCoords(AutotileTemplate template, WorldTilemapLayer layer, Vector2 position, Type matchType)
        {
            int x = (int)position.X;
            int y = (int)position.Y;

            bool up = IsMatchingNeighbour(layer, x, y - 1, matchType);
            bool down = IsMatchingNeighbour(layer, x, y + 1, matchType);
            bool left = IsMatchingNeighbour(layer, x - 1, y, matchType);
            bool right = IsMatchingNeighbour(layer, x + 1, y, matchType);

            AutotileTemplate.Direction direction = GetDirection(up, down, left, right);
            return template.GetCoords(direction);
        }

        private static bool IsMatchingNeighbour(WorldTilemapLayer layer, int x, int y, Type matchType)
        {
            if (matchType == null)
                return false;

            object? neighbour = layer == WorldTilemapLayer.Layer1
                ? Program.world.GetTileLayer1(x, y)
                : Program.world.GetTileLayer2(x, y);

            return neighbour != null && matchType.IsInstanceOfType(neighbour);
        }

        private static AutotileTemplate.Direction GetDirection(bool up, bool down, bool left, bool right)
        {
            if (up)
            {
                if (down)
                {
                    if (left)
                    {
                        return right ? AutotileTemplate.Direction.UpDownLeftRight : AutotileTemplate.Direction.UpDownLeft;
                    }

                    return right ? AutotileTemplate.Direction.UpDownRight : AutotileTemplate.Direction.UpDown;
                }

                if (left)
                {
                    return right ? AutotileTemplate.Direction.UpLeftRight : AutotileTemplate.Direction.UpLeft;
                }

                return right ? AutotileTemplate.Direction.UpRight : AutotileTemplate.Direction.Up;
            }

            if (down)
            {
                if (left)
                {
                    return right ? AutotileTemplate.Direction.DownLeftRight : AutotileTemplate.Direction.DownLeft;
                }

                return right ? AutotileTemplate.Direction.DownRight : AutotileTemplate.Direction.Down;
            }

            if (left)
            {
                return right ? AutotileTemplate.Direction.LeftRight : AutotileTemplate.Direction.Left;
            }

            return right ? AutotileTemplate.Direction.Right : AutotileTemplate.Direction.None;
        }
    }
}