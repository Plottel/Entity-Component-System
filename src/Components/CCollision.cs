using System;
using System.Collections.Generic;

namespace MyGame
{
    public class CCollision : Component
    {
        private List<int> _collidedWith;

        public CCollision (int collidedWith)
        {
            _collidedWith = new List<int>();
            _collidedWith.Add(collidedWith);
        }

        public List<int> CollidedWith
        {
            get {return _collidedWith;}
            set {_collidedWith = value;}
        }
    }
}