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
                pathfinder.Recalculate();
            }

            if (player.X != pathfinder.GetTargetX() || player.Y != pathfinder.GetTargetY())
            {
                pathfinder.SetStart((int) Position.X, (int) Position.Y);
                pathfinder.SetTarget(player.X, player.Y);
                pathfinder.Recalculate();
            }

            if (tick % 20 == 0)
            {
                (int X, int Y)? pos = pathfinder.GetNextStep();

                if (pos != null)
                {
                    Position.X = pos.Value.X;
                    Position.Y = pos.Value.Y;
                }
                else
                {
                    player.Health -= 5;
                }
            }
        }
    }
}