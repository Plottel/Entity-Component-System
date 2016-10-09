using System;
using SwinGameSDK;

namespace MyGame
{
    public class CAnimation : Component
    {
        private Bitmap _img;
        private Animation _anim;
        AnimationScript _animScript;

        public CAnimation (Bitmap img, Animation anim, AnimationScript animScript)
        {
            _img = img;
            _anim = anim;
            _animScript = animScript;
        }

        public Bitmap Img
        {
            get {return _img;}
        }

        public Animation Anim
        {
            get {return _anim;}
        }

        public AnimationScript AnimScript
        {
            get {return _animScript;}
        }
    }
}