using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the class which purely stores Entities with specified Component Masks.
    /// This is primarily used where Systems need to operate on multiple, distinct Entity types.
    /// This enables Entities to be sorted and quickly iterated over, rather than asking the World
    /// to fetch a large number of Entities every frame.
    /// </summary>
    public class EntityHolderSystem : System
    {
        public EntityHolderSystem (List<Type> include, List<Type> exclude, World world) : base(include, exclude, world)
        {
        }

        public override void Process()
        {
        }
    }
}