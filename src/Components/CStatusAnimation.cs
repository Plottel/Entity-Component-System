using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Component which contains an Animation linked to a particular Component Type.
    /// This animation will exist for as long as the Entity it belongs to has the required Component Type.
    /// Status animations are drawn at an offset relative to the Position Component of the Entity they belong to.
    /// </summary>
    public class CStatusAnimation : CAnimation
    {
        /// <summary>
        /// The Component Type dictating if the Animation remains active.
        /// </summary>
        private Type _linkedComponent;

        /// <summary>
        /// The x-axis offset where the Animation will be drawn.
        /// </summary>
        private float _xOffset;

        /// <summary>
        /// The y-axis offset where the Animation will be drawn.
        /// </summary>
        private float _yOffset;

        public CStatusAnimation (Type linkedComponent, float xOffset, float yOffset, Bitmap img, Animation anim, AnimationScript animScript) : base(img, anim, animScript)
        {
            _linkedComponent = linkedComponent;
            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        public Type LinkedComponent
        {
            get {return _linkedComponent;}
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
