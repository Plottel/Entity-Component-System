using System;
using System.Collections.Generic;

namespace MyGame
{
    public class UnfreezingSystem : System
    {
        public UnfreezingSystem (World world) : base(new List<Type> {typeof(CNotMoving)}, new List<Type>{}, world)
        {
        }

        private bool StillActive(CNotMoving notMoving)
        {
            return !(World.GameTime - notMoving.TimeApplied >= notMoving.Duration);
        }

        public override void Process()
        {
            CNotMoving notMoving;

            for (int i = 0; i < Entities.Count; i++)
            {
                notMoving = World.GetComponentOfEntity(Entities[i], typeof(CNotMoving)) as CNotMoving;

                if (!StillActive(notMoving))
                {
                    World.RemoveComponentFromEntity(Entities[i], typeof(CNotMoving));
                }
            }
        }
    }
}