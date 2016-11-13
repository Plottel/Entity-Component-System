using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System that spawns enemy units in the game. It spawns units at every interval,
    /// determined by the World's Game Time.
    /// </summary>
    public class SpawningSystem : System
    {
        /// <summary>
        /// The x coordinate where units will be spawned.
        /// </summary>
        private const int SPAWN_AT_X = 1130;

        /// <summary>
        /// The minimum y coordinate where units can be spawned.
        /// </summary>
        private const int MIN_Y_SPAWN = 100;

        /// <summary>
        /// The maximum y coordinate where units can be spawned.
        /// </summary>
        private const int MAX_Y_SPAWN = 500;

        /// <summary>
        /// Seed used to randomise y coordinate spawn locations.
        /// </summary>
        private Random _seed = new Random();

        /// <summary>
        /// How often the System will spawn units.
        /// </summary>
        private uint _spawnInterval = 5000;

        /// <summary>
        /// When the System last spawned. Used to determine when the next spawn will occur.
        /// </summary>
        private uint _lastSpawn;

        /// <summary>
        /// How many archers will be created for each spawn.
        /// </summary>
        private const int ARCHERS_PER_SPAWN = 10;

        /// <summary>
        /// How many sword men will be created for each spawn.
        /// </summary>
        private const int SWORD_MEN_PER_SPAWN = 5;

        /// <summary>
        /// How many battering rams will be created for each spawn.
        /// </summary>
        private const int BATTERING_RAMS_PER_SPAWN = 3;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.SpawningSystem"/> class.
        /// This System does not contain any Entities, it simply spawns Entities.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public SpawningSystem (World world) : base(new List<Type> {}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Spawns one of each unit type at random Y locations if the System is Ready To Spawn
        /// </summary>
        public override void Process()
        {
            if (ReadyToSpawn())
            {
                _lastSpawn = World.GameTime;

                //Spawn archers
                for (int i = 0; i < ARCHERS_PER_SPAWN; i++)
                    EntityFactory.CreateEnemyArcher(SPAWN_AT_X, _seed.Next(MIN_Y_SPAWN, MAX_Y_SPAWN));

                //Spawn sword men
                for (int i = 0; i < SWORD_MEN_PER_SPAWN; i++)
                    EntityFactory.CreateSwordMan(SPAWN_AT_X, _seed.Next(MIN_Y_SPAWN, MAX_Y_SPAWN));

                //Spawn battering rams
                for (int i = 0; i < BATTERING_RAMS_PER_SPAWN; i++)
                    EntityFactory.CreateBatteringRam(SPAWN_AT_X, _seed.Next(MIN_Y_SPAWN, MAX_Y_SPAWN));
            }
        }

        /// <summary>
        /// Specifies whether or not the System is ready to spawn units.
        /// This uses the Game Time to see if enough time has passed since the last spawn.
        /// </summary>
        /// <returns><c>true</c> if the System is ready to spawn, <c>false</c> otherwise.</returns>
        private bool ReadyToSpawn()
        {
            return World.GameTime - _lastSpawn >= _spawnInterval;
        }
    }
}