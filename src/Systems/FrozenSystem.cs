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

                if (FreezeExpired(frozen.TimeApplied, frozen.Duration))
                    World.RemoveComponent<CFrozen>(Entities[i]);
            }
        }

        /// <summary>
        /// Specifies whether or not the Freeze Effect has expired.
        /// </summary>
        /// <returns><c>true</c>, if the effect has expired, <c>false</c> otherwise.</returns>
        /// <param name="startTime">The time the Freeze started.</param>
        /// <param name="duration">The duration of the Freeze .</param>
        private bool FreezeExpired(uint startTime, int duration)
        {
            return Utils.DurationReached(startTime, duration);
        }
    }
}