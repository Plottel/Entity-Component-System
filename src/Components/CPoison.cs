using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Poison Component. This is used for applying poison damage to afflicted Entities,
    /// as well as defining the Poison Component to be applied to Entities by Poison Zones.
    /// </summary>
    public class CPoison : Component
    {
        /// <summary>
        /// The strength of the poison. This is how much damage it will do each tick.
        /// </summary>
        private int _strength;

        /// <summary>
        /// How long the poison will last.
        /// </summary>
        private int _duration;

        /// <summary>
        /// When the poison was applied. Used to determine if it is still active.
        /// </summary>
        private uint _timeApplied;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CPoison"/> class.
        /// </summary>
        /// <param name="strength">The strength of the poison.</param>
        /// <param name="duration">The duration of the poison.</param>
        /// <param name="timeApplied">When the poison was applied.</param>
        public CPoison (int strength, int duration, uint timeApplied)
        {
            _strength = strength;
            _duration = duration;
            _timeApplied = timeApplied;
        }

        /// <summary>
        /// Gets the strength of the poison.
        /// </summary>
        /// <value>The strength of the poison.</value>
        public int Strength
        {
            get {return _strength;}
        }

        /// <summary>
        /// Gets the duration of the poison.
        /// </summary>
        /// <value>The duration of the poison.</value>
        public int Duration
        {
            get {return _duration;}
        }

        /// <summary>
        /// Gets or sets the time the poison was applied.
        /// </summary>
        /// <value>The time the poison was applied.</value>
        public uint TimeApplied
        {
            get {return _timeApplied;}
            set {_timeApplied = value;}
        }
    }
}