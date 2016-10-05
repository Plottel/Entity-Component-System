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

        private void RenderHealthBar(CHealth healthComp, CPosition posComp)
        {
            int pixelsPerHP = posComp.Width / healthComp.Health;
            SwinGame.FillRectangle(Color.DarkGreen, posComp.X, posComp.Y - 6, posComp.Width, 5); //Draw health bar
            SwinGame.FillRectangle(Color.Red, posComp.X, posComp.Y - 6, (pixelsPerHP * healthComp.Damage), 5); //Draw damage bar
        }

        public override void Process()
        {
            CRenderable rendComp;
            CPosition posComp;
            CHealth healthComp;

            //Use components of each entity to draw their bitmaps
            for (int i = 0; i < Entities.Count; i++)
            {
                rendComp = World.GetComponentOfEntity(Entities[i], typeof(CRenderable)) as CRenderable;
                posComp = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                SwinGame.FillRectangle(rendComp.Color, posComp.X, posComp.Y, posComp.Width, posComp.Height);

                if (World.EntityHasComponent(Entities[i], typeof(CHealth)))
                {
                    healthComp = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;
                    RenderHealthBar(healthComp, posComp);
                }
            }
        }
    }
}