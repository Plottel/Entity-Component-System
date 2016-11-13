using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Health Component. This stores Health and Damage details.
    /// When an Entity is out of Health, it is removed from the World.
    /// </summary>
    public class CHealth : Component
    {
        /// <summary>
        /// The maximum health.
        /// </summary>
        private int _health;

        /// <summary>
        /// The current amount of damage the Entity has taken.
        /// </summary>
        public int Damage {get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CHealth"/> class.
        /// </summary>
        /// <param name="health">The maximum health.</param>
        public CHealth (int health)
        {
            _health = health;
            Damage = 0;
        }

        /// <summary>
        /// Gets the health.
        /// </summary>
        /// <value>The health.</value>
        public int Health
        {
            get {return _health;}
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MyGame.CHealth"/> out of health.
        /// </summary>
        /// <returns><c>true</c> if damage is greater than max health, <c>false</c> otherwise.</returns>
        public bool OutOfHealth
        {
            get {return Damage >= _health;}
        }
    }
}