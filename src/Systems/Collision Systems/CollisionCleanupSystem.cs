using System;
using System.Collections.Generic;

namespace MyGame
{
    //This class removes all collision components at the end of each frame to ensure they've been processed
    //It is the last System to be processed so that all collisions are handled before components are removed
    public class CollisionCleanupSystem : System
    {
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
                World.RemoveComponentFromEntity(entID, typeof(CCollision));
            }
        }
    }
}