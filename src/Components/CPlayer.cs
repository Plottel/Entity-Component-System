using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the Component which stores "stats" for the Player.
    /// This includes Gold and the times when abilities were last used. 
    /// </summary>
    public class CPlayer: Component
    {
        /// <summary>
        /// The amount of Gold the Player has. This is ued to purhcase units.
        /// </summary>
        private uint _gold = 5000;

        /// <summary>
        /// The time when Freezing Bullet was last cast.
        /// </summary>
        private uint _timeOfLastFreezingBullet;

        /// <summary>
        /// The time when Poison Zone was last cast.
        /// </summary>
        private uint _timeOfLastPoisonZone;

        /// <summary>
        /// Gets or sets the gold.
        /// </summary>
        /// <value>The gold.</value>
        public uint Gold
        {
            get {return _gold;}
            set {_gold = value;}
        }

        /// <summary>
        /// Gets or sets the time of the last freezing bullet.
        /// </summary>
        /// <value>The time of the last freezing bullet.</value>
        public uint TimeOfLastFreezingBullet
        {
            get {return _timeOfLastFreezingBullet;}
            set {_timeOfLastFreezingBullet = value;}
        }

        /// <summary>
        /// Gets or sets the time of last poison zone.
        /// </summary>
        /// <value>The time of the last poison zone.</value>
        public uint TimeOfLastPoisonZone
        {
            get {return _timeOfLastPoisonZone;}
            set {_timeOfLastPoisonZone = value;}
        }
    }
}