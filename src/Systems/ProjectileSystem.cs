using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the generic Projectile handler system. Checks if Projectiles have reached their
    /// target and, if they have, removes them from the world. This System only processes generic
    /// projectiles - all specific projectiles will be handled in Systems which inherit from this.
    /// </summary>
    public class ProjectileSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.ProjectileSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to</param>
        public ProjectileSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition), typeof(CVelocity)}, 
                                                     new List<Type> {typeof(CFreezingBullet)}, 
                                                     world)
        {
        }

        /// <summary>
        /// Specifies whether or not the projectile has reached its target.
        /// </summary>
        /// <returns><c>true</c> if the projectile has reached its target, <c>false</c> otherwise.</returns>
        /// <param name="entTarget">Ent projectile.</param>
        /// <param name="entPos">Ent position.</param>
        protected bool ReachedTarget(CProjectile entTarget, CPosition entPos)
        {
            return SwinGame.RectanglesIntersect(entTarget.Target.Rect, entPos.Rect);
        }

        /// <summary>
        /// Specifies whether or not the projectile is within the boundaries of the screen.
        /// </summary>
        /// <returns><c>true</c>, if the projectile is on the screen, <c>false</c> otherwise.</returns>
        /// <param name="pos">The projectile's position.</param>
        protected bool ProjectileOnScreen(CPosition pos)
        {
            return SwinGame.RectOnScreen(pos.Rect);
        }

        /// <summary>
        /// Removes all projectiles which have reached their target from the World.
        /// This is done after the Process method in a separate list in order to prevent issues
        /// that arise when modifying a list while looping through it.
        /// </summary>
        /// <param name="toRemove">The list of projectiles to remove.</param>
        protected void RemoveDeadProjectiles(List<int> toRemove)
        {
            foreach (int projectile in toRemove)
            {
                World.RemoveEntity(projectile);
            }
        }

        /// <summary>
        /// Uses the projectile and position components of each Entity to determine whether 
        /// or not the Entity has reached its target. If it has, the Entity is removed from the world.
        /// </summary>
        public override void Process()
        {
            List<int> deadProjectiles = new List<int>();
            CProjectile projectile;
            CPosition pos;

            for (int i = 0; i < Entities.Count; i++)
            {
                projectile = World.GetComponent<CProjectile>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (ReachedTarget(projectile, pos) || !ProjectileOnScreen(pos))
                {
                    deadProjectiles.Add(Entities[i]);
                }
            }
            RemoveDeadProjectiles(deadProjectiles);
        }
    }
}