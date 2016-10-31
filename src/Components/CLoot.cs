using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Loot Component. This is held by Enemy Entities and its contents is given 
    /// to the Player when the Entity dies.
    /// </summary>
    public class CLoot : Component
    {
        /// <summary>
        /// The value of the loot.
        /// </summary>
        private uint _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CLoot"/> class.
        /// </summary>
        /// <param name="value">The value of the loot.</param>
        public CLoot (uint value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value of the loot.
        /// </summary>
        /// <value>The value of the loot.</value>
        public uint Value
        {
            get {return _value;}
        }
    }
}