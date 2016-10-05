using System;
namespace MyGame
{
    public class DamageComponent : Component
    {
        private int _damage;

        public DamageComponent (int damage)
        {
            _damage = damage;
        }

        public int Damage
        {
            get {return _damage;}
            set {_damage = value;}
        }
    }
}