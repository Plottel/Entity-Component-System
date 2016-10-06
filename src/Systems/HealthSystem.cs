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
            CHealth entHealth;

            for (int i = 0; i < Entities.Count; i++)
            {
                entHealth = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;

                if (entHealth.OutOfHealth)
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}