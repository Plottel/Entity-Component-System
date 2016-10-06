using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class RenderingSystem: System
    {
        public RenderingSystem(World world) : base(new List<Type> {typeof(CRenderable), typeof(CPosition)}, new List<Type> {}, world)
        {
        }

        private void RenderHealthBar(CHealth entHealth, CPosition entPos)
        {
            int pixelsPerHP = entPos.Width / entHealth.Health;
            SwinGame.FillRectangle(Color.DarkGreen, entPos.X, entPos.Y - 6, entPos.Width, 5); //Draw health bar
            SwinGame.FillRectangle(Color.Red, entPos.X, entPos.Y - 6, (pixelsPerHP * entHealth.Damage), 5); //Draw damage bar
        }

        public override void Process()
        {
            CRenderable entRend;
            CPosition entPos;
            CHealth entHealth;

            //Use components of each entity to draw their bitmaps
            for (int i = 0; i < Entities.Count; i++)
            {
                entRend = World.GetComponentOfEntity(Entities[i], typeof(CRenderable)) as CRenderable;
                entPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                SwinGame.DrawBitmap(entRend.Img, entPos.X, entPos.Y);

                if (World.EntityHasComponent(Entities[i], typeof(CHealth)))
                {
                    entHealth = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;
                    RenderHealthBar(entHealth, entPos);
                }
            }
        }
    }
}