using System;
namespace MyGame
{
    public class CDamage : Component
    {
        private int _damage;

        public CDamage (int damage)
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