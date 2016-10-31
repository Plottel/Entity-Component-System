using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for removing all collision components at the end of each frame.
    /// This prevents buildup of entries from unhandled collisions and ensures each frame is processing
    /// a new set of collisions. This System is the last to be processed in each frame.
    /// </summary>
    public class CollisionCleanupSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CollisionCleanupSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public CollisionCleanupSystem (World world) : base(new List<Type> {typeof(CCollision)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            /// <summary>
            /// This loop represents each Entity with a Collision Component.
            /// Backwards loop to allow Entities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
                World.RemoveComponent<CCollision>(Entities[i]);
        }
    }
}