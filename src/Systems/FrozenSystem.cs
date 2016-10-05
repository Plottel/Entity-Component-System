using System;
using System.Collections.Generic;

namespace MyGame
{
    public class FrozenSystem : System
    {
        public FrozenSystem (World world) : base(new List<Type> {typeof(CFrozen)}, new List<Type>{}, world)
        {
        }

        public override void Process()
        {
            CFrozen notMoving;

            for (int i = 0; i < Entities.Count; i++)
            {
                notMoving = World.GetComponentOfEntity(Entities[i], typeof(CFrozen)) as CFrozen;

                if (Utils.EffectHasEnded(World.GameTime, notMoving.TimeApplied, notMoving.Duration))
                {
                    World.RemoveComponentFromEntity(Entities[i], typeof(CFrozen));
                }
            }
        }
    }
}