using System;
using System.Collections.Generic;

namespace MyGame
{
    public class CStatusAnimations : Component
    {
        private List<CStatusAnimation> _anims;


        public CStatusAnimations ()
        {
            _anims = new List<CStatusAnimation>();
        }

        public List<CStatusAnimation> Anims
        {
            get {return _anims;}
            set {_anims = value;}
        }
    }
}