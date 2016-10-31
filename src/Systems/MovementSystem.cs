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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.MovementSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public MovementSystem(World world) : base(new List<Type> {typeof(CPosition), typeof(CVelocity)}, new List<Type> {typeof(CFrozen)}, world)
        {
        }

        /// <summary>
        /// Updates the Position Components of each Entity according to the speeds in their Velocity Components.
        /// </summary>
        public override void Process()
        {
            CPosition pos;
            CVelocity vel;

            for (int i = 0; i < Entities.Count; i++)
            {
                pos = World.GetComponent<CPosition>(Entities[i]);
                vel = World.GetComponent<CVelocity>(Entities[i]);

                pos.X += vel.DX;
                pos.Y += vel.DY;
            }
        }
    }
}