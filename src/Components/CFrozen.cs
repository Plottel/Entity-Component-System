using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Component which indicates if an Entity is Frozen.
    /// The Movement System will not operate on Frozen Entities.
    /// </summary>
    public class CFrozen : Component
    {
        /// <summary>
        /// How long the Frozen Component will last.
        /// </summary>
        private int _duration;

        /// <summary>
        /// When the Frozen Component was applied. Used to determine if the effect is still active.
        /// </summary>
        private uint _timeApplied;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CFrozen"/> class.
        /// </summary>
        /// <param name="duration">How long the freeze will last.</param>
        /// <param name="timeApplied">When the freeze was applied.</param>
        public CFrozen (int duration, uint timeApplied)
        {
            _duration = duration;
            _timeApplied = timeApplied;
        }

        /// <summary>
        /// Gets the amount of time the Frozen Component will last for.
        /// </summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get {return _duration;}
        }

        /// <summary>
        /// Gets or sets the time the Frozen Component was applied.
        /// </summary>
        /// <value>The time the Frozen Component was applied.</value>
        public uint TimeApplied
        {
            get {return _timeApplied;}
            set {_timeApplied = value;}
        }
    }
}