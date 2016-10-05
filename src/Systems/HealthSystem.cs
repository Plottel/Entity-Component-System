using System;
using System.Collections.Generic;

namespace MyGame
{
    public class HealthSystem : System
    {
        public HealthSystem (World world) : base (new List<Type> {typeof(CHealth)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            CHealth healthComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                healthComp = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;

                if (healthComp.OutOfHealth)
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}