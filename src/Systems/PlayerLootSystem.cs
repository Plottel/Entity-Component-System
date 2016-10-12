using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that awards Loot to the Player. When an Entity is removed from the World,
    /// this System overrides the Remove method to extract the Loot component from the Entity and award 
    /// it to the Player.
    /// </summary>
    public class PlayerLootSystem : System
    {
        public PlayerLootSystem (World world) : base(new List<Type> {typeof(CLoot), typeof(CEnemyTeam)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Checks if the Entity is within the System. If it is, its loot component is extracted
        /// and its contents sent to the fetched Player Gold System.
        /// </summary>
        /// <param name="entID">The Entity to remove.</param>
        public override void Remove(int entID)
        {
            if (HasEntity(entID))
            {
                //Player Gold System is responsible for managing the Player's resources.
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