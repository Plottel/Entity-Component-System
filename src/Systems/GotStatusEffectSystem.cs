using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for dealing with Entities which have just received a 
    /// status effect. This System will create a Status Effect Animation corresponding to the
    /// type of status effect the Entity received. In future development, this System would also 
    /// be responsible for other applications such as playing sounds.
    /// </summary>
    public class GotStatusEffectSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.GotStatusEffectSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public GotStatusEffectSystem (World world) : base (new List<Type> {typeof(CGotStatusEffect), typeof(CStatusAnimations), typeof(CPosition)}, 
                                                           new List<Type> {}, 
                                                           world)
        {
        }

        /// <summary>
        /// Evaluates the GotStatusEffect Components of each Entity to determine which type of status effect it has received.
        /// The corresponding Status Animation is then created and the GotStatusEffect Component is removed.
        /// </summary>
        public override void Process()
        {
            CGotStatusEffect statusEffects;
            CStatusAnimations statusAnims;
            CPosition pos;

            for (int i = 0; i < Entities.Count; i++)
            {
                statusEffects = World.GetComponent<CGotStatusEffect>(Entities[i]);
                statusAnims = World.GetComponent<CStatusAnimations>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (statusEffects.Effects.Contains(typeof(CFrozen)))
                    HandleFreezeEffect(statusAnims, pos);

                if (statusEffects.Effects.Contains(typeof(CPoison)))
                    HandlePoisonEffect(statusAnims, pos);
            }

            /// <summary>
            /// Remove GotStatusEffect Components from each Entity.
            /// Backwards looping is used to avoid errors caused when modifying a collection while looping over it.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
                World.RemoveComponent<CGotStatusEffect>(Entities[i]);
        }

        /// <summary>
        /// Adds a Poison Cloud status animation linked to the Entity's position.
        /// </summary>
        /// <param name="statusAnims">The Entity's list of Status Animations.</param>
        /// <param name="pos">The Entity's position.</param>
        private void HandlePoisonEffect(CStatusAnimations statusAnims, CPosition pos)
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

        /// <summary>
        /// Adds an Ice Spike status animation linked to the Entity's position.
        /// </summary>
        /// <param name="statusAnims">The Entity's list of Status Animations.</param>
        /// <param name="pos">The Entity's position.</param>
        private void HandleFreezeEffect(CStatusAnimations statusAnims, CPosition pos)
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
    }
}