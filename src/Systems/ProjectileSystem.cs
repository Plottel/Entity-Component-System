using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class ProjectileSystem : System
    {
        public ProjectileSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition)}, new List<Type> {}, world)
        {
        }

        protected bool ReachedTarget(CProjectile projComp, CPosition posComp)
        {
            return SwinGame.RectanglesIntersect(projComp.Target, SwinGame.CreateRectangle(posComp.X, posComp.Y, posComp.Width, posComp.Height));
        }

        public override void Process()
        {
        }
    }
}