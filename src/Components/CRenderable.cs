using System;
using SwinGameSDK;

namespace MyGame
{
    public class CRenderable : Component
    {
        private Bitmap _img;
        
        public CRenderable (Bitmap img)
        {
            _img = img;
        }

        public Bitmap Img
        {
            get {return _img;}
            set {_img = value;}
        }
    }
}