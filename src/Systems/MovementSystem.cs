using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System which handles movement. This System changes the values in Position components
    /// according to the corresponding Velocity component. Any Entity with a Frozen Component will not have
    /// their position changed by the Movement System.
    /// </summary>
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
                entPos = World.GetComponent<CPosition>(Entities[i]);
                entVel = World.GetComponent<CVelocity>(Entities[i]);

                entPos.X += entVel.DX;
                entPos.Y += entVel.DY;
            }
        }
    }
}