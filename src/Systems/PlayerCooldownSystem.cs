using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that manages the Player's cooldowns. It takes the passed in ability details
    /// and indicates whether or not the ability is ready to be used.
    /// </summary>
    public class PlayerCooldownSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PlayerCooldownSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PlayerCooldownSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Specifies whether or not an ability is ready to be used from the passed in details.
        /// </summary>
        /// <param name="gameTime">The current Game Time according to the World.</param>
        /// <param name="lastUsedAt">Then the ability was last used.</param>
        /// <param name="cooldown">How often the ability can be used.</param>
        public static bool AbilityIsReady(uint gameTime, uint lastUsedAt, uint cooldown)
        {
            return gameTime - lastUsedAt >= cooldown;
        }

        public override void Process()
        {
        }
    }
}