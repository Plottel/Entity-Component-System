using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PlayerCooldownSystem : System
    {
        public PlayerCooldownSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {
        }

        public static bool AbilityIsReady(uint gameTime, uint lastUsedAt, uint cooldown)
        {
            return gameTime - lastUsedAt >= cooldown;
        }

        public override void Process()
        {
        }
    }
}

