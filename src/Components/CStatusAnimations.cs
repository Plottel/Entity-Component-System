using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the Component which holds the Status Animations of an Entity.
    /// An Entity can only have one of each Component Type, so this "container" Component
    /// allows an Entity to have multiple Status Animations.
    /// </summary>
    public class CStatusAnimations : Component
    {
        /// <summary>
        /// The List of Status Animations.
        /// </summary>
        private readonly List<CStatusAnimation> _anims = new List<CStatusAnimation>();

        /// <summary>
        /// Gets the List of Status Animations.
        /// </summary>
        /// <value>The List of Status Animations.</value>
        public List<CStatusAnimation> Anims
        {
            get {return _anims;}
        }
    }
}