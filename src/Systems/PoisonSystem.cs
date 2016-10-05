using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PoisonSystem : System
    {
        private uint _tickInterval;

        public PoisonSystem (World world) : base(new List<Type> {typeof(CPoison), typeof(CHealth)}, new List<Type> {}, world)
        {
            _tickInterval = 2000;
        }

        private bool StillPoisoned(CPoison poisonComp)
        {
            return !(World.GameTime - poisonComp.TimeApplied >= poisonComp.Duration); 
        }

        public override void Process()
        {
            if (World.GameTime % _tickInterval < 17)
            {
                CPoison poisonComp;
                CHealth healthComp;

                for (int i = 0; i < Entities.Count; i++)
                {
                    poisonComp = World.GetComponentOfEntity(Entities[i], typeof(CPoison)) as CPoison;

                    if (StillPoisoned(poisonComp))
                    {
                        healthComp = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;
                        healthComp.Damage += poisonComp.Strength;
                    }
                    else
                    {
                        World.RemoveComponentFromEntity(Entities[i], typeof(CPoison));
                    }
                }
            }
        }
    }
}

