using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for drawing Health Bars. Health Bars are drawn above Entity positions as
    /// indicated by their Position Components. First draws a green rectangle representing the total
    /// health of the Entity, then draws a red rectangle on top to represent the damage the Entity has taken.
    /// </summary>
    public class HealthRenderingSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.HealthRenderingSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public HealthRenderingSystem (World world) : base(new List<Type> {typeof(CHealth), typeof(CPosition)}, new List<Type> {typeof(CPlayer)}, world)
        {
        }

        /// <summary>
        /// Process this instance.
        /// </summary>
        public override void Process()
        {
            /// <summary>
            /// Represents how many pixels wide the Health Bar should be for each point of HP the Entity has.
            /// </summary>
            int barWidthPerHP;

            CHealth health;
            CPosition pos;


            for (int i = 0; i < Entities.Count; i++)
            {
                health = World.GetComponent<CHealth>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                barWidthPerHP = pos.Width / health.Health;

                /// <summary>
                /// Renders the Green Health Bar.
                /// </summary>
                SwinGame.FillRectangle(Color.DarkGreen, pos.X, pos.Y - 4, pos.Width, 3);

                /// <summary>
                /// Renders the Red Damage Bar.
                /// </summary>
                SwinGame.FillRectangle(Color.Red, pos.X, pos.Y - 4, (barWidthPerHP * health.Damage), 3); //Draw damage bar
            }
        }
    }
}