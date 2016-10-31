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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.HealthSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public HealthSystem (World world) : base (new List<Type> {typeof(CHealth)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Fetches the Health Component of each Entity. If the Entity is out of health,
        /// it is removed from the World.
        /// </summary>
        public override void Process()
        {
            CHealth health;

            for (int i = 0; i < Entities.Count; i++)
            {
                health = World.GetComponent<CHealth>(Entities[i]);

                if (health.OutOfHealth)
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}