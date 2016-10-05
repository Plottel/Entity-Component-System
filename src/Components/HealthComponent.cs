using System;
namespace MyGame
{
    public class HealthComponent : Component
    {
        private int _health;
        private int _damage;

        public HealthComponent (int health)
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
    }
}