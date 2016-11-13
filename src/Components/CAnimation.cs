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
        /// Gets or sets the Bitmap used for the Animation.
        /// </summary>
        public Bitmap Img {get; set;}

        /// <summary>
        /// Gets or sets the current Animation for the Entity.
        /// </summary>
        public Animation Anim {get; set;}

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
            Img = img;
            Anim = anim;
            _animScript = animScript;
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