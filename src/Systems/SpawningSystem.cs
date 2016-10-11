using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class SpawningSystem : System
    {
        private const int _spawnAtX = 700;
        private int _spawnAtY;
        private Random _random;
        private uint _spawnInterval;
        private uint _lastSpawn;

        public SpawningSystem (World world) : base(new List<Type> {}, new List<Type> {}, world)
        {
            _random = new Random();
            _spawnInterval = 500;
        }

        public override void Process()
        {
            if (World.GameTime - _lastSpawn >= _spawnInterval)
            {
                _lastSpawn = World.GameTime;

                _spawnAtY = _random.Next(50, 550);
                EntityFactory.CreateSwordMan(_spawnAtX, _spawnAtY);
                EntityFactory.CreateBatteringRam(_spawnAtX, _spawnAtY + 50);
                EntityFactory.CreateEnemyArcher(_spawnAtX, _spawnAtY - 50);
            }
        }
    }
}