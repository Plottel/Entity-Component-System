using System;
using System.Collections.Generic;

namespace MyGame
{
    public class MovementSystem : System
    {
        public MovementSystem(World world) : base(new List<Type> {typeof(CPosition), typeof(CVelocity)}, new List<Type> {typeof(CNotMoving)}, world)
        {
        }

        public override void Process()
        {
            CPosition posComp;
            CVelocity velComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                posComp = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                velComp = World.GetComponentOfEntity(Entities[i], typeof(CVelocity)) as CVelocity;

                posComp.X += velComp.DX;
                posComp.Y += velComp.DY;
            }
        }
    }
}