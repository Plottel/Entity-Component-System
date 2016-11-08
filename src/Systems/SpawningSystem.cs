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
        private const int _spawnAtX = 1130;

        /// <summary>
        /// Seed used to randomise y coordinate spawn locations.
        /// </summary>
        private Random _seed = new Random();

        /// <summary>
        /// How often the System will spawn units.
        /// </summary>
        private uint _spawnInterval = 500;

        /// <summary>
        /// When the System last spawned. Used to determine when the next spawn will occur.
        /// </summary>
        private uint _lastSpawn;

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

                //EntityFactory.CreateSwordMan(_spawnAtX, _seed.Next(100, 500));
                //EntityFactory.CreateBatteringRam(_spawnAtX, _seed.Next(100, 500));
                //EntityFactory.CreateEnemyArcher(_spawnAtX, _seed.Next(100, 500));

                EntityFactory.CreateEnemyArcher(_spawnAtX, 200);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 205);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 210);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 215);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 220);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 225);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 230);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 235);

                EntityFactory.CreateEnemyArcher(_spawnAtX, 50);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 150);
                EntityFactory.CreateEnemyArcher(_spawnAtX, 500);
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