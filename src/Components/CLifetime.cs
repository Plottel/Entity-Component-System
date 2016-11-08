using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Lifetime Component. Indicates that the Entity will only exist for a
    /// limited time. When the Lifetime expires, the Entity is removed from the World.
    /// </summary>
    public class CLifetime : Component
    {
        /// <summary>
        /// Represents the amount of time the Entity will live for.
        /// </summary>
        private int _lifetime;

        /// <summary>
        /// Represents the time the Component was created.
        /// </summary>
        private uint _createdAt;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CLifetime"/> class.
        /// </summary>
        /// <param name="lifetime">The amount of time the Entity will live for.</param>
        public CLifetime (int lifetime)
        {
            _lifetime = lifetime;
            _createdAt = World.GameTime;
        }

        /// <summary>
        /// Gets the amount of time the Entity will live for.
        /// </summary>
        /// <value>The amount of time the Entity will live for.</value>
        public int Lifetime
        {
            get {return _lifetime;}
        }

        /// <summary>
        /// Gets the time the Entity was created at.
        /// </summary>
        /// <value>The time the Entity was created at.</value>
        public uint CreatedAt
        {
            get {return _createdAt;}
        }
    }
}