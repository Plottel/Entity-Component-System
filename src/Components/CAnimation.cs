using System;
using SwinGameSDK;

namespace MyGame
{
    public class CAnimation : Component
    {
        private Bitmap _img;
        private Animation _anim;

        public CAnimation (Bitmap img, Animation anim)
        {
            _img = img;
            _anim = anim;
        }

        public Bitmap Img
        {
            get {return _img;}
        }

        public Animation Anim
        {
            get {return _anim;}
        }
    }
}