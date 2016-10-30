using System;
using SwinGameSDK;

namespace MyGame
{
    public class CStatusAnimation : CAnimation
    {
        private Type _key;
        private float _xOffset;
        private float _yOffset;

        public CStatusAnimation (Type key, float xOffset, float yOffset, Bitmap img, Animation anim, AnimationScript animScript) : base(img, anim, animScript)
        {
            _key = key;
            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        public Type Key
        {
            get {return _key;}
        }

        public float XOffset
        {
            get {return _xOffset;}
        }

        public float YOffset
        {
            get {return _yOffset;}
        }
    }
}
