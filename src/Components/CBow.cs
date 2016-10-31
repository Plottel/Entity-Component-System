using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Bow Component. This Component indicates that an Entity has the ability to shoot Arrows.
    /// The speed and damage of these Arrows is defined in this Component.
    /// </summary>
    public class CBow : Component
    {
        /// <summary>
        /// The speed of the Arrows to be shot.
        /// </summary>
        private int _arrowSpeed;

        /// <summary>
        /// The damage inflicted by the Arrows to be shot.
        /// </summary>
        private int _arrowDamage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CBow"/> class.
        /// </summary>
        /// <param name="arrowSpeed">The speed of the Arrows to be shot.</param>
        /// <param name="arrowDamage">The damage inflicted by the Arrows to be shot.</param>
        public CBow (int arrowSpeed, int arrowDamage)
        {
            _arrowSpeed = arrowSpeed;
            _arrowDamage = arrowDamage;
        }  

        /// <summary>
        /// Gets the speed of the Arrows to be shot.
        /// </summary>
        /// <value>The arrow speed.</value>
        public int ArrowSpeed
        {
            get {return _arrowSpeed;}
        }

        /// <summary>
        /// Gets the damage inflicted by the Arrows to be shot.
        /// </summary>
        /// <value>The arrow damage.</value>
        public int ArrowDamage
        {
            get {return _arrowDamage;}
        }
    }
}