using System;
using System.Collections.Generic;

namespace MyGame
{
    public class MovementSystem : System
    {
        public MovementSystem(World world) : base(new List<Type> {typeof(CPosition), typeof(CVelocity)}, new List<Type> {typeof(CFrozen)}, world)
        {
        }

        public override void Process()
        {
            CPosition entPos;
            CVelocity entVel;

            for (int i = 0; i < Entities.Count; i++)
            {
                entPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                entVel = World.GetComponentOfEntity(Entities[i], typeof(CVelocity)) as CVelocity;

                entPos.X += entVel.DX;
                entPos.Y += entVel.DY;
            }
        }
    }
}