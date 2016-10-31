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
        private List<CStatusAnimation> _deadAnims;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.StatusAnimationRenderingSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public StatusAnimationRenderingSystem (World world) : base (new List<Type> {typeof(CStatusAnimations), typeof(CPosition)}, new List<Type> {}, world)
        {
            _deadAnims = new List<CStatusAnimation>();
        }

        /// <summary>
        /// Updates and Draws each Animation in each Entity's StatusAnimation Component.
        /// If an Entity no longer has the required Component for the StatusAnimation, it
        /// is removed from the StatusAnimations Component.
        /// </summary>
        public override void Process ()
        {
            _deadAnims.Clear();

            CStatusAnimations statusAnims;
            CPosition pos;

            for (int i = 0; i < Entities.Count; i++)
            {
                statusAnims = World.GetComponent<CStatusAnimations>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                foreach (CStatusAnimation statusAnim in statusAnims.Anims)
                {
                    //Whole point is that these animatinos will loop until status is over
                    if (World.EntityHasComponent(Entities[i], statusAnim.Key))
                    {
                        SwinGame.DrawAnimation(statusAnim.Anim, statusAnim.Img, pos.X + statusAnim.XOffset, pos.Y + statusAnim.YOffset);
                        SwinGame.UpdateAnimation(statusAnim.Anim);
                    }
                    else
                    {
                        _deadAnims.Add(statusAnim);
                    }
                }

                foreach (CStatusAnimation anim in _deadAnims)
                    statusAnims.Anims.Remove(anim);
            }
        }
    }
}