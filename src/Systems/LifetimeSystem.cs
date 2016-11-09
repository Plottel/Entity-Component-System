using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for dealing with Entities with a Lifetime Component.
    /// This System is responsible for removing Entities from the World if their Lifetime has passed.
    /// </summary>
    public class LifetimeSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.LifetimeSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public LifetimeSystem (World world) : base (new List<Type> {typeof(CLifetime)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Checks the Lifetime of each Entity. If their Lifetime has passed,
        /// the Entity is removed from the World.
        /// </summary>
        public override void Process()
        {
            CLifetime lifetime;

            /// <summary>
            /// This loop represents each Entity with a Lifetime.
            /// Backwards loop to allow Entities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                lifetime = World.GetComponent<CLifetime>(Entities[i]);

                if (TimeLimitReached(lifetime.CreatedAt, lifetime.Lifetime))
                    World.RemoveEntity(Entities[i]);
            }
        }

        /// <summary>
        /// Specifies whether or not an Entity's Lifetime is over.
        /// </summary>
        /// <returns><c>true</c>, if Lifetime is over, <c>false</c> otherwise.</returns>
        /// <param name="creationTime">Time the Entity was created.</param>
        /// <param name="lifetime">Lifetime of the Entity.</param>
        private bool TimeLimitReached(uint creationTime, int lifetime)
        {
            return Utils.DurationReached(creationTime, lifetime);
        }
    }
}