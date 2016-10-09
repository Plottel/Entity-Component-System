using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class ProjectileSystem : System
    {
        //This system only processes generic projectiles - all specific projectiles are excluded
        public ProjectileSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition), typeof(CVelocity)}, new List<Type> {typeof(CFreezingBullet)}, world)
        {
        }

        protected bool ReachedTarget(CProjectile entProjectile, CPosition entPos)
        {
            return SwinGame.RectanglesIntersect(entProjectile.Target.Rect, entPos.Rect);
        }

        protected void RemoveDeadProjectiles(List<int> toRemove)
        {
            foreach (int projectile in toRemove)
            {
                World.RemoveEntity(projectile);
            }
        }

        public override void Process()
        {
            List<int> deadProjectiles = new List<int>();
            CProjectile entProjectile;
            CPosition entPos;

            for (int i = 0; i < Entities.Count; i++)
            {
                entProjectile = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                entPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (ReachedTarget(entProjectile, entPos))
                {
                    deadProjectiles.Add(Entities[i]);
                }
            }
            RemoveDeadProjectiles(deadProjectiles);
        }
    }
}