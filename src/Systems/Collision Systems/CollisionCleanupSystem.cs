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

        //Can't loop through Entities and remove them from the world - it removes them from the list
        //Which leads to for loop collection looping problems
        private List<int> PopulateCollisionComponentList(List<int> toRemove)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                toRemove.Add(Entities[i]);
            }
            return toRemove;
        }

        public override void Process()
        {
            List<int> toRemove = new List<int>();
            PopulateCollisionComponentList(toRemove);
            foreach (int entID in toRemove)
            {
                World.RemoveComponent<CCollision>(entID);
            }
        }
    }
}