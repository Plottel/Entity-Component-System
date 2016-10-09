using System;
namespace MyGame
{
    public class CPlayer: Component
    {
        private uint _gold;

        public CPlayer()
        {
            _gold = 5000;
        }

        public uint Gold
        {
            get {return _gold;}
            set {_gold = value;}
        }
    }
}