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
            /*if (entTarget.Target.X > 100) //it's a player dude targetting an enemy dude
            {
                float x, y, width, height;
                x = entTarget.Target.X;
                y = entTarget.Target.Y;
                width = entTarget.Target.Width;
                height = entTarget.Target.Height;

                Rectangle newRect = SwinGame.CreateRectangle(x, y, width, height);
                return SwinGame.RectanglesIntersect(newRect, entPos.Rect);
            }*/
                
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
        /// Uses the projectile and position components of each Entity to determine whether 
        /// or not the Entity has reached its target. If it has, the Entity is removed from the world.
        /// </summary>
        public override void Process()
        {
            List<int> deadProjectiles = new List<int>();
            CProjectile projectile;
            CPosition pos;

            /// <summary>
            /// Backwards loop to allow Entities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                projectile = World.GetComponent<CProjectile>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (ReachedTarget(projectile, pos) || !ProjectileOnScreen(pos))
                    World.RemoveEntity(Entities[i]);
            }
        }
    }
}