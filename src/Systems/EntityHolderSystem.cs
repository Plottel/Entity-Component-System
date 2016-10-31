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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.EntityHolderSystem"/> class.
        /// </summary>
        /// <param name="include">The Component Types the System will operate on.</param>
        /// <param name="exclude">The Component Types the System will NOT operate on.</param>
        /// <param name="world">The World the System belongs to.</param>
        public EntityHolderSystem (List<Type> include, List<Type> exclude, World world) : base(include, exclude, world)
        {
        }

        public override void Process()
        {
        }
    }
}