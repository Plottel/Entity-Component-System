using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents a Tag Component indicating that the Entity has just been afflicted with one or more Status Effects.
    /// Presence of this Component triggers some Systems to operate.
    /// </summary>
    public class CGotStatusEffect : Component
    {
        /// <summary>
        /// Represents the status effects the Entity has been afflicted with.
        /// </summary>
        private readonly List<Type> _effects = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CGotStatusEffect"/> class.
        /// </summary>
        /// <param name="effect">The Effect Type the Entity has been afflicted with.</param>
        public CGotStatusEffect(Type effect)
        {
            _effects.Add(effect);
        }

        /// <summary>
        /// Gets the <see cref="T:MyGame.CGotStatusEffect"/> at the specified index.
        /// Allows Effects to be accessed directly from the class, rather than CGotStatusEffect.Effects[i]
        /// </summary>
        /// <param name="index">Index.</param>
        public Type this[int index]
        {
            get {return _effects[index];}
        }

        /// <summary>
        /// Adds the passed in Status Effect to the list of Effects.
        /// </summary>
        /// <param name="effect">The Status Effect type to be added.</param>
        public void AddEffect(Type effect)
        {
            _effects.Add(effect);
        }

        /// <summary>
        /// Specifies if the Component contains the specified Effect.
        /// </summary>
        /// <returns><c>true</c> if the Component contains the Effect, <c>false</c> otherwise.</returns>
        /// <param name="effect">The Effect to check.</param>
        public bool Contains(Type effect)
        {
            return _effects.Contains(effect);
        }

        /// <summary>
        /// Gets the number of Status Effects in the Component.
        /// </summary>
        /// <value>The number of Status Effects.</value>
        public int Count
        {
            get {return _effects.Count;}
        }
    }
}