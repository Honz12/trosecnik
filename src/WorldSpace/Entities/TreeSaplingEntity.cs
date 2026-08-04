using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public class TreeSaplingEntity : IEntity
    {
        private static readonly Random Rng = new();

        private Vector2 Position;
        private bool Grown = false;
        private double WaitTime = Rng.NextDouble() * 30.0 + 30.0; // 30 - 60 seconds

        public Vector2 GetPosition(ulong tick)
        {
            return Position;
        }

        public IEntity.EntityRequest GetRequest()
        {
            return Grown ? IEntity.EntityRequest.SelfDelete : IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return "entities/treeSapling/treeSapling.png";
        }

        public string? GetTextureAbove(ulong tick)
        {
            return null;
        }

        public Vector2 GetTextureSize(ulong tick)
        {
            return new(1, 1);
        }

        public void SetPos(Vector2 position)
        {
            Position = position;
            if (Program.world.interactableEntities.ContainsKey(Position))
            {
                Sapling_Break(true);
                return;
            }
            Program.world.interactableEntities.Add(Position, this);
        }

        public void Update(Player player, World world, ulong tick, float deltaTime)
        {
            world.EntityBlockTile((int) Position.X, (int) Position.Y);
            WaitTime -= deltaTime;
            if (WaitTime <= 0.0)
            {
                bool checkPassed = true;

                for (int x = -1; x <= 1 && checkPassed; x++)
                {
                    for (int y = -1; y <= 1 && checkPassed; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (world.interactableEntities.ContainsKey(Position + new Vector2(x, y)))
                        {
                            checkPassed = false;
                        }
                    }
                }

                if (checkPassed)
                {
                    SoundManager.Play($"entity/tree/chop{Rng.Next(6) + 1}.wav");
                    TreeEntity tree = new();
                    tree.SetPos(Position);
                    world.AddEntity(tree);
                    world.interactableEntities.Remove(Position);
                    world.interactableEntities.Add(Position, tree);
                    Grown = true;
                }
                else
                {
                    WaitTime = Rng.NextDouble() * 30.0 + 30.0;
                }
            }
        }

        public void Sapling_Break(bool offset = false)
        {
            ItemDropEntity sapling = new(new InventorySpace.Items.TreeSaplingItem());

            if (offset)
                sapling.SetPos(new (
                    (int) (Position.X + (Rng.Next(2) * 2 - 1)),
                    (int) (Position.Y + (Rng.Next(2) * 2 - 1))
                ));
            else
                sapling.SetPos(Position);

            Program.world.AddEntity(sapling);
            Grown = true;
        }
    }
}