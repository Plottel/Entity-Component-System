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
            /// The Freezing Bullet System's Component Masks are:
            /// Include - CProjectile, CPosition, CVelocity, CFreezingBullet
            /// Exclude - null
            /// </summary>
            Include.Add(typeof(CFreezingBullet));
            Exclude.Remove(typeof(CFreezingBullet));
        }

        /// <summary>
        /// Loops through all Freezing Bullet entities and checks if they've reached their destination.
        /// If they have, the Bullet is removed and a Freeze Zone is created at the bullet's location.
        /// </summary>
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
                
                if (ReachedTarget(bulletProj, bulletPos) || !ProjectileOnScreen(bulletPos))
                {
                    deadFreezingBullets.Add(Entities[i]);
                    EntityFactory.CreateFreezeZone(bulletPos);
                }
            }
            RemoveDeadProjectiles(deadFreezingBullets);
        }
    }
}