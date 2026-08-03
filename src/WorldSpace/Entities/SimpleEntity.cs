using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public class SimpleEntity : SimpleEntityBase
    {
        private Pathfinder? pathfinder;

        protected override string GetTexturePath(ulong tick)
        {
            return "items/item_0001.png";
        }

        protected override void SimpleUpdate(Player player, World world, ulong tick)
        {
            if (pathfinder == null)
            {
                pathfinder = new(world);
                pathfinder.SetStart((int) Position.X, (int) Position.Y);
                pathfinder.SetTarget(player.X, player.Y);
            }

            if (tick % 20 == 0)
            {
                if (new Vector2(Position.X - player.X, Position.Y - player.Y).LengthSquared() < 100)
                    if (player.X != pathfinder.GetTargetX() || player.Y != pathfinder.GetTargetY() || pathfinder.Finished)
                    {
                        pathfinder.SetStart((int) Position.X, (int) Position.Y);
                        pathfinder.SetTarget(player.X, player.Y);
                        pathfinder.Recalculate();
                    }

                if (!pathfinder.Finished)
                {
                    (int X, int Y)? pos = pathfinder.GetNextStep();

                    if (pos != null)
                    {
                        Vector2 PrevPos = Position;
                        Position.X = pos.Value.X;
                        Position.Y = pos.Value.Y;
                        if (Position.X == player.X && Position.Y == player.Y)
                        {
                            Position = PrevPos;
                            player.Health -= 2;
                        }
                    }
                }
            }

            world.EntityBlockTile((int) Position.X, (int) Position.Y);
        }
    }
}