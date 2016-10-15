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
        public FrozenSystem (World world) : base(new List<Type> {typeof(CFrozen)}, new List<Type>{typeof(CAppliesDebuff)}, world)
        {
        }

        public override void Process()
        {
            CFrozen entFrozen;

            for (int i = 0; i < Entities.Count; i++)
            {
                entFrozen = World.GetComponentOfEntity(Entities[i], typeof(CFrozen)) as CFrozen;

                if (Utils.EffectHasEnded(World.GameTime, entFrozen.TimeApplied, entFrozen.Duration))
                {
                    World.RemoveComponentFromEntity(Entities[i], typeof(CFrozen));
                }
            }
        }
    }
}