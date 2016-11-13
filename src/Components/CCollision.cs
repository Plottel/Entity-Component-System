using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the Component which indicates that a collision has occurred.
    /// When a collision occurs, the two colliding Entities are each given a 
    /// Collision Component with a reference to the other Entity inside it.
    /// </summary>
    public class CCollision : Component
    {
        /// <summary>
        /// An Entity can collide with more than one Entity per frame.
        /// This list represents all the Entities an Entity has collided with.
        /// </summary>
        private readonly List<ulong> _collidedWith;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CCollision"/> class.
        /// </summary>
        /// <param name="collidedWith">The Entity the Entity has collided with.</param>
        public CCollision (ulong collidedWith)
        {
            _collidedWith = new List<ulong>();
            _collidedWith.Add(collidedWith);
        }

        /// <summary>
        /// Gets the <see cref="T:MyGame.CCollision"/> at the specified index.
        /// Allows collisions to be accessed directly from the class, rather than CCollision.CollidedWith[i]
        /// </summary>
        /// <param name="index">The list index.</param>
        public ulong this[int index]
        {
            get {return _collidedWith[index];}
        }

        /// <summary>
        /// Adds an Entity to the list of collisions.
        /// </summary>
        /// <param name="collider">The Entity that has collided.</param>
        public void AddCollision(ulong collider)
        {
            _collidedWith.Add(collider);
        }

        /// <summary>
        /// Gets the number of Entities the Entity collided with.
        /// </summary>
        /// <value>The list of collisions.</value>
        public int Count
        {
            get {return _collidedWith.Count;}
        }  
    }
}