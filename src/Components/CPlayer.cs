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
        public uint Gold {get; set;} = 5000;

        /// <summary>
        /// The time when Freezing Bullet was last cast.
        /// </summary>
        public uint TimeOfLastFreezingBullet {get; set;}

        /// <summary>
        /// The time when Poison Zone was last cast.
        /// </summary>
        public uint TimeOfLastPoisonZone {get; set;}
    }
}