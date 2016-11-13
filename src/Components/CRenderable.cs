using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Renderable Component. This Component stores a single Bitmap.
    /// </summary>
    public class CRenderable : Component
    {
        /// <summary>
        /// The Bitmap.
        /// </summary>
        public Bitmap Img {get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CRenderable"/> class.
        /// </summary>
        /// <param name="img">The Bitmap to be stored in the Component.</param>
        public CRenderable (Bitmap img)
        {
            Img = img;
        }
    }
}