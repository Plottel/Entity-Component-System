using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for dealing with Entities afflicted with a Frozen Component.
    /// If an Entity's Frozen Component has expired, it is removed from the Entity.
    /// </summary>
    public class FrozenSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.FrozenSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public FrozenSystem (World world) : base(new List<Type> {typeof(CFrozen)}, new List<Type>{typeof(CAppliesDebuff)}, world)
        {
        }

        /// <summary>
        /// Fetches the Frozen Component of each Entity. If the effect has expired,
        /// the Frozen Component is removed from the Entity.
        /// </summary>
        public override void Process()
        {
            CFrozen frozen;

            for (int i = 0; i < Entities.Count; i++)
            {
                frozen = World.GetComponent<CFrozen>(Entities[i]);

                if (Utils.EffectHasEnded(World.GameTime, frozen.TimeApplied, frozen.Duration))
                    World.RemoveComponent<CFrozen>(Entities[i]);
            }
        }
    }
}