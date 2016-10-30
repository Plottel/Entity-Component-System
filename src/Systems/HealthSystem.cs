using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for determing if an Entity is out of health.
    /// Any entity which has run out of health is removed from the World.
    /// </summary>
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
                entHealth = World.GetComponent<CHealth>(Entities[i]);

                if (entHealth.OutOfHealth)
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}