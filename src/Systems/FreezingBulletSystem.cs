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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.FreezingBulletSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to .</param>
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
            CProjectile projectile;
            CPosition pos;   

            /// <summary>
            /// This loop represents each Freezing Bullet.
            /// Backwards loop to allow Enities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                projectile = World.GetComponent<CProjectile>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);
                
                if (ReachedTarget(projectile, pos) || !ProjectileOnScreen(pos))
                {
                    EntityFactory.CreateFreezeZone(pos);
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}