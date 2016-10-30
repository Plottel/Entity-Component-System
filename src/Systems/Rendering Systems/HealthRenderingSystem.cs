using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for drawing Health Bars. Draws two rectangles representing
    /// current Health of each Entity.
    /// </summary>
    public class HealthRenderingSystem : System
    {
        public HealthRenderingSystem (World world) : base(new List<Type> {typeof(CHealth), typeof(CPosition)}, new List<Type> {typeof(CPlayer)}, world)
        {
        }

        public override void Process()
        {
            CHealth entHealth;
            CPosition entPos;

            for (int i = 0; i < Entities.Count; i++)
            {
                entHealth = World.GetComponent<CHealth>(Entities[i]);
                entPos = World.GetComponent<CPosition>(Entities[i]);

                int pixelsPerHP = entPos.Width / entHealth.Health;

                SwinGame.FillRectangle(Color.DarkGreen, entPos.X, entPos.Y - 4, entPos.Width, 3); //Draw health bar
                SwinGame.FillRectangle(Color.Red, entPos.X, entPos.Y - 4, (pixelsPerHP * entHealth.Damage), 3); //Draw damage bar
            }
        }
    }
}
