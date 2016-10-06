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

        protected bool ReachedTarget(CProjectile entProjectile, CPosition entPos)
        {
            return Utils.AreColliding(entProjectile.Target, entPos);
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
        }
    }
}