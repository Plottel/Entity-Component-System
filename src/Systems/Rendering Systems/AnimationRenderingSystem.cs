using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
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
                animComp = World.GetComponentOfEntity(Entities[i], typeof(CAnimation)) as CAnimation;
                animPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

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