using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class GotStatusEffectSystem : System
    {
        public GotStatusEffectSystem (World world) : base (new List<Type> {typeof(CGotStatusEffect), typeof(CStatusAnimations), typeof(CPosition)}, 
                                                           new List<Type> {}, 
                                                           world)
        {
        }

        public override void Process()
        {
            CStatusAnimations statusAnims;
            CPosition pos;

            for (int i = 0; i < Entities.Count; i++)
            {
                statusAnims = World.GetComponent<CStatusAnimations>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (World.EntityHasComponent(Entities[i], typeof(CFrozen)))
                {
                    CStatusAnimation newStatusAnim;
                    Bitmap bmp;
                    Animation anim;
                    AnimationScript animScript;
                    float xOffset;
                    float yOffset;

                    if (pos.Width <= 21)
                        bmp = SwinGame.BitmapNamed("SmallIceSpike");
                    else
                        bmp = SwinGame.BitmapNamed("BigIceSpike");

                    xOffset = (pos.Width / 2) - (bmp.CellWidth / 2);
                    yOffset = pos.Height - bmp.CellHeight;

                    anim = SwinGame.CreateAnimation("Freeze", SwinGame.AnimationScriptNamed("IceSpikeAnim"));
                    animScript = SwinGame.AnimationScriptNamed("IceSpikeAnim");

                    newStatusAnim = new CStatusAnimation(typeof(CFrozen), xOffset, yOffset, bmp, anim, animScript);

                    statusAnims.Anims.Add(newStatusAnim);
                }

                if (World.EntityHasComponent(Entities[i], typeof(CPoison)))
                {
                    CStatusAnimation newStatusAnim;
                    Bitmap bmp;
                    Animation anim;
                    AnimationScript animScript;
                    float xOffset;
                    float yOffset;

                    if (pos.Width <= 21)
                        bmp = SwinGame.BitmapNamed("SmallPoisonCloud");
                    else
                        bmp = SwinGame.BitmapNamed("BigPoisonCloud");

                    xOffset = (pos.Width / 2) - (bmp.CellWidth / 2);
                    yOffset = 0;

                    anim = SwinGame.CreateAnimation("Poison", SwinGame.AnimationScriptNamed("PoisonZoneAnim"));
                    animScript = SwinGame.AnimationScriptNamed("PoisonZoneAnim");

                    newStatusAnim = new CStatusAnimation(typeof(CPoison), xOffset, yOffset, bmp, anim, animScript);

                    statusAnims.Anims.Add(newStatusAnim);
                }
            }

            for (int i = Entities.Count - 1; i >= 0; i--)
                World.RemoveComponent<CGotStatusEffect>(Entities[i]);
        }
    }
}
