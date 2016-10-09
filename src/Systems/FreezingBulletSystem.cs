using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class FreezingBulletSystem : ProjectileSystem
    {
        public FreezingBulletSystem (World world) : base(world)
        {
            Include.Add(typeof(CFreezingBullet));
            Exclude.Remove(typeof(CFreezingBullet));
        }

        public override void Process()
        {
            CProjectile bulletProj;
            CPosition bulletPos;   

            List<int> deadFreezingBullets = new List<int>();

            //For each Freezing Bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                bulletProj = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                
                if (ReachedTarget(bulletProj, bulletPos))
                {
                    deadFreezingBullets.Add(Entities[i]);
                    EntityFactory.CreateFreezeZone(bulletPos);
                }
            }
            RemoveDeadProjectiles(deadFreezingBullets);
        }
    }
}