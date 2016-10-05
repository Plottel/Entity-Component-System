using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class RenderingSystem: System
    {
        public RenderingSystem(World world) : base((int)ComponentType.Renderable | (int)ComponentType.Position, world)
        {
        }

        private void RenderHealthBar(HealthComponent healthComp, PositionComponent posComp)
        {
            int pixelsPerHP = posComp.Width / healthComp.Health;
            SwinGame.FillRectangle(Color.DarkGreen, posComp.X, posComp.Y - 6, posComp.Width, 5); //Draw health bar
            SwinGame.FillRectangle(Color.Red, posComp.X, posComp.Y - 6, (pixelsPerHP * healthComp.Damage), 5); //Draw damage bar
        }

        public override void Process()
        {
            RenderableComponent rendComp;
            PositionComponent posComp;
            HealthComponent healthComp;

            //Use components of each entity to draw their bitmaps
            for (int i = 0; i < Entities.Count; i++)
            {
                rendComp = World.GetComponentOfEntity(Entities[i], typeof(RenderableComponent)) as RenderableComponent;
                posComp = World.GetComponentOfEntity(Entities[i], typeof(PositionComponent)) as PositionComponent;

                SwinGame.FillRectangle(rendComp.Color, posComp.X, posComp.Y, posComp.Width, posComp.Height);

                if (World.EntityHasComponent(Entities[i], typeof(HealthComponent)))
                {
                    healthComp = World.GetComponentOfEntity(Entities[i], typeof(HealthComponent)) as HealthComponent;
                    RenderHealthBar(healthComp, posComp);
                }
            }
        }
    }
}