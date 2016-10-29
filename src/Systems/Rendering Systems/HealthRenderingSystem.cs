using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
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
                entHealth = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;
                entPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                int pixelsPerHP = entPos.Width / entHealth.Health;

                SwinGame.FillRectangle(Color.DarkGreen, entPos.X, entPos.Y - 4, entPos.Width, 3); //Draw health bar
                SwinGame.FillRectangle(Color.Red, entPos.X, entPos.Y - 4, (pixelsPerHP * entHealth.Damage), 3); //Draw damage bar
            }
        }
    }
}
