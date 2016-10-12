using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that manages the Player's resources and handles all purchases. This System
    /// can be thought of like a "shop". This System also receives Loot components and gives its contents
    /// to the Player.
    /// </summary>
    public class PlayerGoldSystem : System
    {
        private uint _archerCost;
        private uint _wizardCost;

        /// <summary>
        /// Buying Wizards reduces the cooldowns of spells, down to these specified values for each spell.
        /// </summary>
        private int _minimumFreezingBulletCooldown = 200;
        private int _minimumPoisonZoneCooldown = 2000;

        /// <summary>
        /// Represents the seed used to randomise y coordinate spawn locations for Player units.
        /// </summary>
        private Random _spawnAt = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PlayerGoldSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PlayerGoldSystem(World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {   
            _archerCost = 10;
            _wizardCost = 20;
        }

        /// <summary>
        /// Takes a Loot Component; extracts its contents and gives it to the Player
        /// </summary>
        /// <param name="lootComp">The Loot to be given to the Player.</param>
        public void GiveLoot(CLoot lootComp)
        {
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            player.Gold += lootComp.Value;
        }

        /// <summary>
        /// Purchases a Wizard for the Player if they have enough gold. Lowers the Player's gold, increments the
        /// Wizard Count and lowers Player spell cooldowns if they have not reached the minimum values.
        /// </summary>
        /// <param name="player">The Player.</param>
        public void BuyWizard(CPlayer player)
        {
            if (player.Gold > _wizardCost)
            {
                player.WizardCount += 1;
                player.Gold -= _wizardCost;

                if (player.FreezingBulletCooldown > _minimumFreezingBulletCooldown)
                {
                    player.FreezingBulletCooldown -= 20;
                }

                if (player.PoisonZoneCooldown > _minimumPoisonZoneCooldown)
                {
                    player.PoisonZoneCooldown -= 20;
                }
            }
        }

        /// <summary>
        /// Purchases an Archer for the Player if they have enough gold. Lowers the Player's gold, increments the 
        /// Archer count and spawns an Archer at a random y coordinate inside the Castle.
        /// </summary>
        /// <returns>The archer.</returns>
        /// <param name="player">Player.</param>
        public void BuyArcher(CPlayer player)
        {
            if (player.Gold >= _archerCost)
            {
                EntityFactory.CreatePlayerArcher(50, _spawnAt.Next(20, 550));
                player.Gold -= _archerCost;
                player.ArcherCount += 1;
            }
        }

        public override void Process()
        {
        }
    }
}