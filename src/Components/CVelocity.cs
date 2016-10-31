using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Velocity Component. This Component gives Entities a Speed and an X / Y velocity.
    /// </summary>
    public class CVelocity : Component
    {
        /// <summary>
        /// The x-axis velocity.
        /// </summary>
        private float _dx;

        /// <summary>
        /// The y-axis velocity.
        /// </summary>
        private float _dy;

        /// <summary>
        /// The speed. Used for vector calculations.
        /// </summary>
        private float _speed;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CVelocity"/> class.
        /// </summary>
        /// <param name="dx">The x velocity.</param>
        /// <param name="dy">The y velocity.</param>
        /// <param name="speed">The speed.</param>
        public CVelocity (float dx, float dy, float speed)
        {
            _dx = dx;
            _dy = dy;
            _speed = speed;
        }

        /// <summary>
        /// Gets or sets the x velocity.
        /// </summary>
        /// <value>The x velocity.</value>
        public float DX
        {
            get {return _dx;}
            set {_dx = value;}
        }

        /// <summary>
        /// Gets or sets the y velocity.
        /// </summary>
        /// <value>The y velocity.</value>
        public float DY
        {
            get {return _dy;}
            set {_dy = value;}
        }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed
        {
            get {return _speed;}
            set {_speed = value;}
        }
    }
}