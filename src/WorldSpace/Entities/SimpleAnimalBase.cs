using System.Numerics;

namespace trosecnik.src.WorldSpace.Entities
{
    public abstract class SimpleAnimalBase : IEntity
    {
        protected Vector2 Position;
        protected int Health;
        protected bool Died;
        protected double StepWaitTime;

        public Vector2 GetPosition(ulong tick)
        {
            return Position;
        }

        public IEntity.EntityRequest GetRequest()
        {
            return Died ? IEntity.EntityRequest.SelfDelete : IEntity.EntityRequest.None;
        }

        public string GetTexture(ulong tick)
        {
            return GetAnimalTexture();
        }

        protected abstract string GetAnimalTexture();

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
            throw new NotImplementedException();
        }

        public bool IsInteractable()
        {
            return false;
        }

        public void SetPos(Vector2 position)
        {
            Position = position;
            Animal_SetProperties();
        }

        public void Update(Player player, World world, ulong tick, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public abstract void Animal_SetProperties();
        public abstract void Animal_DropLoot();

        public void SimpleAnimal_Hit(int damage)
        {
            Health -= damage;
            SoundManager.Play("player/damage/damage1.wav");
            if (Health <= 0)
            {
                Animal_DropLoot();
            }
        }
    }
}
