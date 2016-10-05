using System;
namespace MyGame
{
    public class CHealth : Component
    {
        private int _health;
        private int _damage;

        public CHealth (int health)
        {
            _health = health;
            _damage = 0;
        }

        public int Health
        {
            get {return _health;}
            set {_health = value;}
        }

        public int Damage
        {
            get {return _damage;}
            set {_damage = value;}
        }

        public bool OutOfHealth
        {
            get {return _damage >= _health;}
        }
    }
}