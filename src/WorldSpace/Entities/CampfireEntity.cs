using System.Numerics;
using trosecnik.src.InventorySpace;
using trosecnik.src.InventorySpace.Items;

namespace trosecnik.src.WorldSpace.Entities
{
    public class CampfireEntity : IEntity
    {
        private Vector2 Position;
        private bool Destroyed;
        private double LitTimeRemaining;

        public Vector2 GetPosition(ulong tick)
        {
            return Position;
        }

        public IEntity.EntityRequest GetRequest()
        {
            return Destroyed ? IEntity.EntityRequest.SelfDelete : IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return LitTimeRemaining <= 0 ? "entities/campfire/campfire_0001.png" : $"entities/campfire/campfire_{tick / 10 % 3 + 2:D4}.png";
        }

        public string? GetTextureAbove(ulong tick)
        {
            return null;
        }

        public Vector2 GetTextureSize(ulong tick)
        {
            return new(1, 1);
        }

        public void Interact()
        {
            IItem selected = Program.player.PlayerInventory.GetItems()[Program.player.PlayerInventory.Selected];

            if (selected is WoodenLogItem)
            {
                if (LitTimeRemaining < 0) LitTimeRemaining = 0;
                LitTimeRemaining += 30;
                Program.player.PlayerInventory.RemoveItem(Program.player.PlayerInventory.Selected);
            }

            if (LitTimeRemaining > 0)
            {
                if (selected is BowlWithDirtyWaterItem)
                {
                    Program.player.PlayerInventory.RemoveItem(Program.player.PlayerInventory.Selected);
                    Program.player.PlayerInventory.AddItem(new BowlWithWaterItem());
                }
            }
        }

        public bool IsInteractable()
        {
            return true;
        }

        public void SetPos(Vector2 position)
        {
            Position = position;
            Program.world.interactableEntities.Add(position, this);
        }

        public void Update(Player player, World world, ulong tick, float deltaTime)
        {
            world.EntityBlockTile((int) Position.X, (int) Position.Y);
            LitTimeRemaining -= deltaTime;
        }

        public void Campfire_Destroy()
        {
            Destroyed = true;
            Program.DropItem(new(Program.player.X, Program.player.Y), new CampfireItem());
            Program.world.interactableEntities.Remove(Position);
        }
    }
}
