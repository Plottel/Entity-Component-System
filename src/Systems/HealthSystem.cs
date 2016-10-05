using System;
namespace MyGame
{
    public class HealthSystem : System
    {
        public HealthSystem (World world) : base ((int)ComponentType.Health, world)
        {
        }

        private bool OutOfHealth(HealthComponent healthComp)
        {
            return healthComp.Damage >= healthComp.Health;
        }

        public override void Process()
        {
            HealthComponent healthComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                healthComp = World.GetComponentOfEntity(Entities[i], typeof(HealthComponent)) as HealthComponent;

                if (OutOfHealth(healthComp))
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}