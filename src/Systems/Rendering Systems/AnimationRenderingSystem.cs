using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling all Animations in the program.
    /// Updates each Animation and, if it has finished and has no other components, removes the Entity from the World.
    /// </summary>
    public class AnimationRenderingSystem : System
    {
        public AnimationRenderingSystem (World world) : base(new List<Type> {typeof(CAnimation), typeof(CPosition)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Loops through each animation, updating and drawing them.
        /// If an animation has ended, the Animation Component is removed from the Entity.
        /// If the Entity has no other Components, it is removed from the World.
        /// </summary>
        public override void Process()
        {
            CAnimation anim;
            CPosition pos;

            /// <summary>
            /// This loop represents each Entity with an Animation Component.
            /// Backwards loop to allow Entities to be removed from the World while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                anim = World.GetComponent<CAnimation>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (SwinGame.AnimationEnded(anim.Anim))
                {
                    if (World.GetAllComponentsOfEntity(Entities[i]).Count == 1)
                        World.RemoveEntity(Entities[i]);
                }
                else
                {
                    SwinGame.DrawAnimation(anim.Anim, anim.Img, pos.X, pos.Y);
                    SwinGame.UpdateAnimation(anim.Anim);
                }
            }
        }
    }
}