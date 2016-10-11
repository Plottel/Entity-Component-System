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
            if (HasEntity(entID))
            {
                PlayerGoldSystem goldSystem = World.GetSystem(typeof(PlayerGoldSystem)) as PlayerGoldSystem;
                goldSystem.GiveLoot(World.GetComponentOfEntity(entID, typeof(CLoot)) as CLoot);
            }
            base.Remove(entID);
        }

        public override void Process()
        {
        }
    }
}