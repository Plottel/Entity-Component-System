using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Component which inflicts damage. This Component is used 
    /// by all Entities which deal damage to determine how much damage is dealt.
    /// </summary>
    public class CDamage : Component
    {
        /// <summary>
        /// The amount of damage to be dealt.
        /// </summary>
        private int _damage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CDamage"/> class.
        /// </summary>
        /// <param name="damage">The amount of damage to be dealt.</param>
        public CDamage (int damage)
        {
            _damage = damage;
        }

        /// <summary>
        /// Gets the amount of damage to be dealt.
        /// </summary>
        /// <value>The damage.</value>
        public int Damage
        {
            get {return _damage;}
        }
    }
}