using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for rendering Status Animations. A Status Animation is linked to a 
    /// Component Type and will exist so long as the Entity the Animation belongs to has this Component Type.
    /// This System checks if each Status Animation is still valid and, if it's not, it is removed.
    /// </summary>
    public class StatusAnimationRenderingSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.StatusAnimationRenderingSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public StatusAnimationRenderingSystem (World world) : base (new List<Type> {typeof(CStatusAnimations), typeof(CPosition)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Updates and Draws each Animation in each Entity's StatusAnimation Component.
        /// If an Entity no longer has the required Component for the StatusAnimation, it
        /// is removed from the StatusAnimations Component.
        /// </summary>
        public override void Process ()
        {
            CStatusAnimations statusAnims;
            CStatusAnimation anim;
            CPosition pos;

            /// <summary>
            /// This loop represents each Entity with a Status Animations Component.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                statusAnims = World.GetComponent<CStatusAnimations>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                /// <summary>
                /// This loop represents each Status Animation inside the Status Animations Component.
                /// Backwards loop to allow Status Animations to be removed while looping.
                /// </summary>
                for (int j = statusAnims.Anims.Count - 1; j >= 0; j--)
                {
                    anim = statusAnims[j];

                    if (World.EntityHasComponent(Entities[i], anim.LinkedComponent))
                    {
                        float x = pos.X + anim.XOffset;
                        float y = pos.Y + anim.YOffset;

                        SwinGame.DrawAnimation(anim.Anim, anim.Img, x, y);
                        SwinGame.UpdateAnimation(anim.Anim);
                    }
                    else
                    {
                        statusAnims.Anims.RemoveAt(j);
                    }
                }
            }
        }
    }
}