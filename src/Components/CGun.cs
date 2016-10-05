using System;
namespace MyGame
{
    public class CGun : Component
    {
        private int _bulletSpeed;
        private int _bulletDamage;

        public CGun (int bulletSpeed, int bulletDamage)
        {
            _bulletSpeed = bulletSpeed;
            _bulletDamage = bulletDamage;
        }

        public int BulletSpeed
        {
            get {return _bulletSpeed;}
        }

        public int BulletDamage
        {
            get {return _bulletDamage;}
        }
    }
}