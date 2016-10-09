using System;
using System.Collections.Generic;

namespace MyGame
{
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