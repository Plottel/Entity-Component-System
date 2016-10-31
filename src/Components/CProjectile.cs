using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Projectile Component. This Component stores a Position Component
    /// as a target and is used by Projectile Systems to determine if Projectiles have 
    /// reached their target.
    /// </summary>
    public class CProjectile : Component
    {
        /// <summary>
        /// The target of the projectile.
        /// </summary>
        private CPosition _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CProjectile"/> class.
        /// </summary>
        /// <param name="target">The target of the projectile.</param>
        public CProjectile (CPosition target)
        {
            _target = target;
        }

        /// <summary>
        /// Gets or sets the target of the projectile.
        /// </summary>
        /// <value>The target of the projectile.</value>
        public CPosition Target
        {
            get {return _target;}
            set {_target = value;}
        }
    }
}