using trosecnik.src.WorldSpace;

namespace trosecnik.src
{
    public class Pathfinder
    {
        // This pathfinder finds the closest path to a destination while stepping only on walkable tiles

        public enum MovementStyle
        {
            FourDirectional,
            EightDirectional
        }

        public enum AllowedTiles
        {
            Walkable,
            All
        }

        private World World;

        private int StartX;
        private int StartY;
        private int TargetX;
        private int TargetY;

        private int MaxRangeFromStart = 100; // in tiles how much the search can travel in X and Y dimensions, 100 means a maximum area of 201x201 is searched

        private MovementStyle movementStyle = MovementStyle.FourDirectional;
        private AllowedTiles allowedTiles = AllowedTiles.Walkable;

        private List<(int X, int Y)> path = new();
        private int nextStepIndex = 0;

        public bool FoundPath { get; private set; }
        public bool Finished { get; private set; }

        public Pathfinder(World world)
        {
            World = world;
        }

        public void SetMovementStyle(MovementStyle style)
        {
            movementStyle = style;
        }

        public void SetAllowedTiles(AllowedTiles allowed)
        {
            allowedTiles = allowed;
        }

        public void SetStart(int x, int y)
        {
            StartX = x;
            StartY = y;
        }

        public void SetTarget(int x, int y)
        {
            TargetX = x;
            TargetY = y;
        }

        public List<(int X, int Y)> GetPath()
        {
            return path;
        }

        public bool HasPath => FoundPath && nextStepIndex < path.Count;

        public (int X, int Y)? GetNextStep()
        {
            if (!HasPath)
                return null;

            (int X, int Y) step = path[nextStepIndex];
            nextStepIndex++;
            if (nextStepIndex >= path.Count)
                Finished = true;
            return step;
        }

        public void Recalculate()
        {
            path.Clear();
            nextStepIndex = 0;
            FoundPath = false;
            Finished = true;

            int minX = Math.Max(0, StartX - MaxRangeFromStart);
            int maxX = Math.Min(World.Width - 1, StartX + MaxRangeFromStart);
            int minY = Math.Max(0, StartY - MaxRangeFromStart);
            int maxY = Math.Min(World.Height - 1, StartY + MaxRangeFromStart);

            int gridWidth = maxX - minX + 1;
            int gridHeight = maxY - minY + 1;

            int ToIndex(int x, int y) => (x - minX) + (y - minY) * gridWidth;

            int Heuristic(int x1, int y1, int x2, int y2)
            {
                int dx = Math.Abs(x1 - x2);
                int dy = Math.Abs(y1 - y2);
                return movementStyle == MovementStyle.FourDirectional ? dx + dy : Math.Max(dx, dy);
            }

            IEnumerable<(int X, int Y)> GetNeighbors(int x, int y)
            {
                yield return (x + 1, y);
                yield return (x - 1, y);
                yield return (x, y + 1);
                yield return (x, y - 1);

                if (movementStyle == MovementStyle.EightDirectional)
                {
                    yield return (x + 1, y + 1);
                    yield return (x + 1, y - 1);
                    yield return (x - 1, y + 1);
                    yield return (x - 1, y - 1);
                }
            }

            void ReconstructPath(int[] cameFromArray, int endIndex)
            {
                List<int> chain = new();
                int current = endIndex;
                while (current != -1)
                {
                    chain.Add(current);
                    current = cameFromArray[current];
                }
                chain.Reverse();

                for (int i = 1; i < chain.Count; i++)
                {
                    int index = chain[i];
                    path.Add((minX + index % gridWidth, minY + index / gridWidth));
                }
            }

            bool IsInsideWorld(int x, int y) => x >= 0 && x < World.Width && y >= 0 && y < World.Height;

            if (!IsInsideWorld(StartX, StartY) || !IsInsideWorld(TargetX, TargetY))
                return;

            if (allowedTiles == AllowedTiles.Walkable && !World.GetWalkable(StartX, StartY))
                return;

            if (allowedTiles == AllowedTiles.Walkable && !World.GetWalkable(TargetX, TargetY))
                return;

            if (Math.Abs(TargetX - StartX) > MaxRangeFromStart || Math.Abs(TargetY - StartY) > MaxRangeFromStart)
                return;

            if (StartX == TargetX && StartY == TargetY)
            {
                FoundPath = true;
                return;
            }

            int[] gScore = new int[gridWidth * gridHeight];
            Array.Fill(gScore, int.MaxValue);
            int[] cameFrom = new int[gridWidth * gridHeight];
            Array.Fill(cameFrom, -1);
            bool[] closed = new bool[gridWidth * gridHeight];

            int startIndex = ToIndex(StartX, StartY);
            int targetIndex = ToIndex(TargetX, TargetY);

            gScore[startIndex] = 0;

            PriorityQueue<int, int> open = new();
            open.Enqueue(startIndex, Heuristic(StartX, StartY, TargetX, TargetY));

            while (open.Count > 0)
            {
                int current = open.Dequeue();

                if (closed[current])
                    continue;

                closed[current] = true;

                if (current == targetIndex)
                {
                    ReconstructPath(cameFrom, targetIndex);
                    FoundPath = true;
                    Finished = false;
                    return;
                }

                int currentX = minX + current % gridWidth;
                int currentY = minY + current / gridWidth;

                foreach ((int X, int Y) neighbor in GetNeighbors(currentX, currentY))
                {
                    int nx = neighbor.X;
                    int ny = neighbor.Y;

                    if (nx < minX || nx > maxX || ny < minY || ny > maxY)
                        continue;

                    if (allowedTiles == AllowedTiles.Walkable && !World.GetWalkable(nx, ny))
                        continue;

                    int neighborIndex = ToIndex(nx, ny);

                    if (closed[neighborIndex])
                        continue;

                    int tentativeG = gScore[current] + 1;
                    if (tentativeG < gScore[neighborIndex])
                    {
                        gScore[neighborIndex] = tentativeG;
                        cameFrom[neighborIndex] = current;
                        open.Enqueue(neighborIndex, tentativeG + Heuristic(nx, ny, TargetX, TargetY));
                    }
                }
            }
        }

        public int GetTargetX()
        {
            return TargetX;
        }

        public int GetTargetY()
        {
            return TargetY;
        }
    }
}
