using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class StatusAnimationRenderingSystem : System
    {
        private List<CStatusAnimation> _deadAnims;

        public StatusAnimationRenderingSystem (World world) : base (new List<Type> {typeof(CStatusAnimations), typeof(CPosition)}, new List<Type> {}, world)
        {
            _deadAnims = new List<CStatusAnimation>();
        }

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