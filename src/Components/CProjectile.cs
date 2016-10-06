using System;
using SwinGameSDK;

namespace MyGame
{
    public class CProjectile : Component
    {
        private CPosition _target;

        public CProjectile (CPosition target)
        {
            _target = target;
        }

        public CPosition Target
        {
            get {return _target;}
            set {_target = value;}
        }
    }
}