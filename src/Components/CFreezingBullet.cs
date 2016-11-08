using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Freezing Bullet Component. Each Freezing Bullet has a target
    /// which, when reached, will trigger the Freezing Bullet System to create a Freeze Zone.
    /// </summary>
    public class CFreezingBullet : Component
    {
        /// <summary>
        /// The X coordinate of the Freezing Bullet's target.
        /// </summary>
        private float _targetX;

        /// <summary>
        /// The Y coordinate of the Freezing Bullet's target.
        /// </summary>
        private float _targetY;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CFreezingBullet"/> class.
        /// </summary>
        /// <param name="targetX">The X coordinate of the Freezing Bullet's target.</param>
        /// <param name="targetY">The Y coordinate of the Freezing Bullet's target.</param>
        public CFreezingBullet(float targetX, float targetY)
        {
            _targetX = targetX;
            _targetY = targetY;
        }

        /// <summary>
        /// Gets the X coordinate of the Freezing Bullet's target.
        /// </summary>
        /// <value>The X coordinate.</value>
        public float TargetX
        {
            get {return _targetX;}
        }

        /// <summary>
        /// Gets the Y coordinate of the Freezing Bullet's target.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public float TargetY
        {
            get {return _targetY;}
        }
    }  
}