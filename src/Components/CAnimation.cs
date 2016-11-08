using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Animation Component. This Component defines stand-alone animations, not
    /// animations which are linked to a Component or Entity.
    /// </summary>
    public class CAnimation : Component
    {
        /// <summary>
        /// The Bitmap used for the Animation.
        /// </summary>
        private Bitmap _img;

        /// <summary>
        /// The current Animation.
        /// </summary>
        private Animation _anim;

        /// <summary>
        /// The Animation Script containing Animation details.
        /// </summary>
        private AnimationScript _animScript;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CAnimation"/> class.
        /// </summary>
        /// <param name="img">The Animation image.</param>
        /// <param name="anim">The Animation.</param>
        /// <param name="animScript">The Animation Script containing Animation details.</param>
        public CAnimation (Bitmap img, Animation anim, AnimationScript animScript)
        {
            _img = img;
            _anim = anim;
            _animScript = animScript;
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Bitmap Img
        {
            get {return _img;}
            set {_img = value;}
        }

        /// <summary>
        /// Gets or sets the animation.
        /// </summary>
        /// <value>The animation.</value>
        public Animation Anim
        {
            get {return _anim;}
            set {_anim = value;}
        }

        /// <summary>
        /// Gets the animation script.
        /// </summary>
        /// <value>The animation script.</value>
        public AnimationScript AnimScript
        {
            get {return _animScript;}
        }
    }
}