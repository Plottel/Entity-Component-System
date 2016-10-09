using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PlayerLootSystem : System
    {
        public PlayerLootSystem (World world) : base(new List<Type> {typeof(CLoot), typeof(CEnemyTeam)}, new List<Type> {}, world)
        {
        }

        public override void Remove(int entID)
        {
            PlayerSystem playerSystem = World.GetSystem(typeof(PlayerSystem)) as PlayerSystem;
            playerSystem.GiveLoot(World.GetComponentOfEntity(entID, typeof(CLoot)) as CLoot);
            base.Remove(entID);
        }

        public override void Process()
        {
        }
    }
}