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
        private const int _spawnAtX = 1200;

        /// <summary>
        /// The y coordinate where units will be spawned.
        /// </summary>
        private int _spawnAtY;

        /// <summary>
        /// Seed used to randomise y coordinate spawn locations.
        /// </summary>
        private Random _seed = new Random();

        /// <summary>
        /// How often the System will spawn units.
        /// </summary>
        private uint _spawnInterval;

        /// <summary>
        /// When the System last spawned. Used to determine when the next spawn will occur.
        /// </summary>
        private uint _lastSpawn;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.SpawningSystem"/> class.
        /// This System does not contain any Entities, it simply spawns units.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public SpawningSystem (World world) : base(new List<Type> {}, new List<Type> {}, world)
        {
            _spawnInterval = 500;
        }

        /// <summary>
        /// Specifies whether or not the System is ready to spawn units.
        /// This uses the Game Time to see if enough time has passed since the last spawn.
        /// </summary>
        private bool ReadyToSpawn()
        {
            return World.GameTime - _lastSpawn >= _spawnInterval;
        }

        /// <summary>
        /// Spawns one of each unit type at random Y locations if the System is Ready To Spawn
        /// </summary>
        public override void Process()
        {
            if (ReadyToSpawn())
            {
                _lastSpawn = World.GameTime;

                _spawnAtY = _seed.Next(100, 500);
                EntityFactory.CreateSwordMan(_spawnAtX, _spawnAtY);
                EntityFactory.CreateBatteringRam(_spawnAtX, _spawnAtY + 50);
                EntityFactory.CreateEnemyArcher(_spawnAtX, _spawnAtY - 50);
            }
        }
    }
}