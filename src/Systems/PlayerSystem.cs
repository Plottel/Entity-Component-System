using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class PlayerSystem : System
    {
        /// <summary>
        /// Represents the ID for the Castle. This is used to assign Enemy Entities
        /// a target without needing to fetch IDs from the world. 
        /// </summary>
        public static ulong PLAYER_ENTITY_ID;

        /// <summary>
        /// Represents the number of Player Archers currently on the field.
        /// </summary>
        public static int ARCHER_COUNT;

        /// <summary>
        /// Represents the Gold costs for units the Player can purchase.
        /// </summary>
        public const uint ARCHER_COST = 10;
        public const uint WIZARD_COST = 20;
        public const int EXPLOSION_MAN_COST = 50;

        /// <summary>
        /// Represents the cooldowns of Abilities the Player can use.
        /// </summary>
        public const int FREEZING_BULLET_COOLDOWN = 2000;
        public const int POISON_ZONE_COOLDOWN = 2000;

        /// <summary>
        /// Represents the seed used to randomise y coordinate spawn locations for Player units.
        /// </summary>
        private Random _spawnAtY = new Random();

        /// <summary>
        /// Represents the x coordinate spawn location for Player units.
        /// </summary>
        private float _spawnAtX = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PlayerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PlayerSystem (World world) : base (new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {
        }


        public override void Process()
        {
        }

        /// <summary>
        /// Represents the end of the game. If the Player entity is removed from the World, the game is over.
        /// </summary>
        /// <param name="entID">Ent identifier.</param>
        public override void Remove(ulong entID)
        {
            if (HasEntity(entID))
            {
                //Congratulations you lost LOL
            }
        }

        /// <summary>
        /// Takes a Loot Component; extracts its contents and gives it to the Player
        /// </summary>
        /// <param name="loot">The Loot to be given to the Player.</param>
        public void GiveLoot(CLoot loot)
        {
            CPlayer player = World.GetComponent<CPlayer>(PLAYER_ENTITY_ID);
            player.Gold += loot.Value;
        }

        /// <summary>
        /// Purchases an Archer for the Player if they have enough gold. Lowers the Player's gold, increments the 
        /// Archer count and spawns an Archer at a random y coordinate inside the Castle.
        /// </summary>
        public void BuyArcher()
        {
            CPlayer player = World.GetComponent<CPlayer>(PLAYER_ENTITY_ID);

            if (player.Gold >= ARCHER_COST)
            {
                EntityFactory.CreatePlayerArcher(_spawnAtX, _spawnAtY.Next(20, 550));
                player.Gold -= ARCHER_COST;
                ARCHER_COUNT++;
            }
        }

        /// <summary>
        /// Purchases an Explosion Man for the Player if they have enough gold. Lowers the Player's gold 
        /// and spawns an Explosion Man at the centre of the most populated Hash cell on the field.
        /// </summary>
        public void BuyExplosionMan()
        {
            CPlayer player = World.GetComponent<CPlayer>(PLAYER_ENTITY_ID);

            if (player.Gold >= EXPLOSION_MAN_COST)
            {
                EntityFactory.CreateExplosionMan();
                player.Gold -= EXPLOSION_MAN_COST;
            }
        }

        /// <summary>
        /// Specifies whether or not an ability is ready to be used from the passed in details.
        /// </summary>
        /// <param name="lastUsedAt">Then the ability was last used.</param>
        /// <param name="cooldown">How often the ability can be used.</param>
        public static bool AbilityIsReady(uint lastUsedAt, int cooldown)
        {
            return Utils.DurationReached(lastUsedAt, cooldown);
        }

        /// <summary>
        /// Attemps to create a Freezing Bullet heading towards the passed in point.
        /// This will only occur if the Ability is off cooldown.
        /// </summary>
        /// <param name="pt">The point the Freezing Bullet will head towards.</param>
        public void CastFreezingBullet(Point2D pt)
        {
            CPlayer player = World.GetComponent<CPlayer>(PLAYER_ENTITY_ID);

            if (AbilityIsReady(player.TimeOfLastFreezingBullet, FREEZING_BULLET_COOLDOWN))
            {
                CPosition playerPos = World.GetComponent<CPosition>(PLAYER_ENTITY_ID);
                EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, pt.X, pt.Y);
                player.TimeOfLastFreezingBullet = World.GameTime;
            }
        }

        /// <summary>
        /// Attempts to create a Poison Zone at the passed in point.
        /// This will only occur if the Ability is off cooldown.
        /// </summary>
        /// <param name="pt">Point.</param>
        public void CastPoisonZone(Point2D pt)
        {
            CPlayer player = World.GetComponent<CPlayer>(PLAYER_ENTITY_ID);

            if (AbilityIsReady(player.TimeOfLastPoisonZone, POISON_ZONE_COOLDOWN))
            {
                EntityFactory.CreatePoisonZone(pt.X, pt.Y);
                player.TimeOfLastPoisonZone = World.GameTime;
            }
        }
    }
}