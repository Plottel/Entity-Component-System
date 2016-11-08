using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the generic Projectile handler system. Checks if Projectiles have left the boundaries 
    /// of the screen and, if they have, removes them from the World. 
    /// </summary>
    public class ProjectileSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.ProjectileSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to</param>
        public ProjectileSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition), typeof(CVelocity)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Determines if each Entity has left the bounds of the screen. If it has,
        /// then it is removed from the World.
        /// </summary>
        public override void Process()
        {
            CPosition pos;

            /// <summary>
            /// Backwards loop to allow Entities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (!ProjectileOnScreen(pos))
                    World.RemoveEntity(Entities[i]);
            }
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
    }
}