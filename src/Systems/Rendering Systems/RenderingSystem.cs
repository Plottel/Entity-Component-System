using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for Rendering static Bitmaps. This System only handles 
    /// Bitmaps and will not deal with Animations.
    /// </summary>
    public class RenderingSystem: System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.RenderingSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public RenderingSystem(World world) : base(new List<Type> {typeof(CRenderable), typeof(CPosition)}, new List<Type> {typeof(CAnimation)}, world)
        {
        }

        /// <summary>
        /// Renders the Bitmap in each Entity's Renderable Component at the x and y coordinates
        /// in the Entity's Position Component.
        /// </summary>
        public override void Process()
        {
            CRenderable render;
            CPosition pos;

            for (int i = 0; i < Entities.Count; i++)
            {
                render = World.GetComponent<CRenderable>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                SwinGame.DrawBitmap(render.Img, pos.X, pos.Y);
            }
        }
    }
}