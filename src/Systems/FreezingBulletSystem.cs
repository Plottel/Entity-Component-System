using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the class responsible for handling Freezing Bullets. Checks if each Freezing Bullet 
    /// has reached its destination. If it has, it is removed from the World and a Freeze Zone is created.
    /// </summary>
    public class FreezingBulletSystem : ProjectileSystem
    {
        public FreezingBulletSystem (World world) : base(world)
        {
            /// <summary>
            /// The F
            /// </summary>
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