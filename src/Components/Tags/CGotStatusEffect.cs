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
        /// Adds the passed in Status Effect to the list of Effects.
        /// </summary>
        /// <param name="effect">The Status Effect type to be added.</param>
        public void AddEffect(Type effect)
        {
            _effects.Add(effect);
        }

        /// <summary>
        /// Gets the list of Status Effect types.
        /// </summary>
        /// <value>The Status Effects.</value>
        public List<Type> Effects
        {
            get
            {
                return _effects;
            }
        }
    }
}