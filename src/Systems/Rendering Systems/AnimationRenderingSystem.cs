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

        private void DeleteDeadAnims(List<int> toDelete)
        {
            foreach (int entID in toDelete)
            {
                World.RemoveEntity(entID);
            }
        }

        public override void Process()
        {
            CAnimation animComp;
            CPosition animPos;
            List<int> deadAnims = new List<int>();

            for (int i = 0; i < Entities.Count; i++)
            {
                animComp = World.GetComponent<CAnimation>(Entities[i]);
                animPos = World.GetComponent<CPosition>(Entities[i]);

                /// <summary>
                /// If Entity has no Components other than the finished animation, remove it from the World.
                /// </summary>
                if (SwinGame.AnimationEnded(animComp.Anim))
                {
                    if (World.GetAllComponentsOfEntity(Entities[i]).Count == 1) //If Entity is just an animation
                    {
                        deadAnims.Add(Entities[i]);
                    }
                }
                else
                {
                    SwinGame.DrawAnimation(animComp.Anim, animComp.Img, animPos.X, animPos.Y);
                    SwinGame.UpdateAnimation(animComp.Anim);
                }
            }
            DeleteDeadAnims(deadAnims);
        }
    }
}