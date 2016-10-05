using System;
using SwinGameSDK;

namespace MyGame
{
    public class SpawningSystem : System
    {
        private const int _spawnAtX = 700;
        private int _spawnAtY;
        private Random _random;
        private uint _spawningInterval;

        public SpawningSystem (World world) : base((int)ComponentType.None, world)
        {
            _random = new Random();
            _spawningInterval = 2000;
        }

        public override void Process()
        {
            if (World.GameTime % _spawningInterval < 17)
            {
                _spawnAtY = _random.Next(50, 550);
                EntityFactory.CreateWalker(_spawnAtX, _spawnAtY);
                EntityFactory.CreateShooter(_spawnAtX, _spawnAtY);
            }
        }
    }
}