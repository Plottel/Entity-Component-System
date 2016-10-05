using System;
namespace MyGame
{
    public class PoisonSystem : System
    {
        private uint _tickInterval;

        public PoisonSystem (World world) : base((int)ComponentType.Health | (int)ComponentType.Poison, world)
        {
            _tickInterval = 2000;
        }

        private bool StillPoisoned(PoisonComponent poisonComp)
        {
            return !(World.GameTime - poisonComp.TimeApplied >= poisonComp.Duration); 
        }

        public override void Process()
        {
            if (World.GameTime % _tickInterval < 17)
            {
                PoisonComponent poisonComp;
                HealthComponent healthComp;

                for (int i = 0; i < Entities.Count; i++)
                {
                    poisonComp = World.GetComponentOfEntity(Entities[i], typeof(PoisonComponent)) as PoisonComponent;

                    if (StillPoisoned(poisonComp))
                    {
                        healthComp = World.GetComponentOfEntity(Entities[i], typeof(HealthComponent)) as HealthComponent;
                        healthComp.Damage += poisonComp.Strength;
                    }
                    else
                    {
                        World.RemoveComponentFromEntity(Entities[i], typeof(PoisonComponent));
                    }
                }
            }
        }
    }
}

