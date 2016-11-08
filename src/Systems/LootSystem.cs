using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that awards Loot to the Player. When an Entity is removed from the World,
    /// this System overrides the Remove method to extract the Loot component from the Entity and award 
    /// it to the Player.
    /// </summary>
    public class LootSystem: System
    {
        public LootSystem(World world) : base(new List<Type> {typeof(CLoot), typeof(CEnemyTeam)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
        }

        /// <summary>
        /// Checks if the Entity is within the System. If it is, its loot component is extracted
        /// and its contents sent to the fetched Player Gold System.
        /// </summary>
        /// <param name="entID">The Entity to remove.</param>
        public override void Remove(ulong entID)
        {
            if (HasEntity(entID))
            {
                CLoot lootToGive = World.GetComponent<CLoot>(entID);
                PlayerSystem playerSystem = World.GetSystem<PlayerSystem>();
                playerSystem.GiveLoot(lootToGive);
            }
            base.Remove(entID);
        }      
    }
}