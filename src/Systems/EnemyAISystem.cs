using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System which handles AI for the Enemy team. Enemy AI have their target automatically assigned to
    /// be the Player's Castle. The flow chart for AI behaviour is Get Target -> Check Range -> Check Cooldown -> Attack.
    /// When the AI Attacks, they begin their Attack animation. When the animation concludes, the attack is carried out.
    /// </summary>
    public class EnemyAISystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.EnemyAISystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public EnemyAISystem (World world) : base (new List<Type> {typeof(CAI), typeof(CEnemyTeam)}, new List<Type> {}, world)
        {
        }      

        public override void Process()
        {
            CAI AI;
            CPosition pos;
            CAnimation anim;

            /// <summary>
            /// For each Enemy AI Entity.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                AI = World.GetComponent<CAI>(Entities[i]);
                pos = World.GetComponent<CPosition>(Entities[i]);

                if (!AI.IsInRange)
                {
                    CheckRange(Entities[i], AI, pos);

                    /// <summary>
                    /// If the AI is now in range, stop moving and begin attacking.
                    /// </summary>
                    if (AI.IsInRange)
                        World.RemoveComponent<CVelocity>(Entities[i]);
                }
                else if (!AI.AttackIsReady)
                {
                    CheckCooldown(Entities[i]);
                }
                else
                {
                    anim = World.GetComponent<CAnimation>(Entities[i]);

                    /// <summary>
                    /// Attack will be carried out when the Attack animation has ended.
                    /// </summary>
                    if (SwinGame.AnimationEnded(anim.Anim))
                    {
                        /// <summary>
                        /// Carry out the attack.
                        /// </summary>
                        Attack(Entities[i], "Enemy");

                        /// <summary>
                        /// If Attacking, stand still during the cooldown period.
                        /// </summary>
                        SwinGame.AssignAnimation(anim.Anim, "Still", anim.AnimScript);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a Circle to represent the attack radius of the AI. If the Position Component of the
        /// AI's target is within this Circle, then the AI is in range.
        /// </summary>
        /// <param name="entID">The Entity to check range for.</param>
        /// <param name="AI">AI component of the Entity to check range for.</param>
        /// <param name="pos">Position component of the Entity to check range for.</param>
        protected void CheckRange(int entID, CAI AI, CPosition pos)
        {
            Circle attackRadius = SwinGame.CreateCircle(pos.Centre.X, pos.Centre.Y, AI.Range + (pos.Width / 2));
            CPosition targetPos = World.GetComponent<CPosition>(AI.TargetID);

            AI.IsInRange = SwinGame.CircleRectCollision(attackRadius, targetPos.Rect);
        }

        /// <summary>
        /// Checks the Game Time against the last time the AI attacked. If the time difference is greater
        /// than the attack cooldown of the AI, then the AI is ready to attack and their Attack animation begins.
        /// </summary>
        /// <param name="entID">Ent identifier.</param>
        protected void CheckCooldown(int entID)
        {
            CAI AI = World.GetComponent<CAI>(entID);

            AI.AttackIsReady = World.GameTime - AI.LastAttackTime >= AI.Cooldown;

            if (AI.AttackIsReady)
            {
                /// <summary>
                /// Begin the Attack animation for the AI
                /// </summary>
                CAnimation anim = World.GetComponent<CAnimation>(entID);
                SwinGame.AssignAnimation(anim.Anim, "Attack", anim.AnimScript);
            }
        }

        protected void Attack(int entID, string team)
        {
            CAI AI = World.GetComponent<CAI>(entID);
            CDamage damage; 
            CBow bow;
            CPosition pos;

            CHealth targetHealth = World.GetComponent<CHealth>(AI.TargetID);
            CPosition targetPos;

            /// <summary>
            /// The AI has just attacked, so its cooldown is started.
            /// </summary>
            AI.LastAttackTime = World.GameTime;
            AI.AttackIsReady = false;

            /// <summary>
            /// If the Attack Type is Melee, directly apply damage to the Target.
            /// If the Attack Type is Gun, create a projectile Entity to travel towards the Target.
            /// </summary>
            switch (AI.AttackType)
            {
                case AttackType.Melee:
                {
                    damage = World.GetComponent<CDamage>(entID);

                    targetHealth.Damage += damage.Damage;
                    break;
                }

                case AttackType.Bow:
                {
                    pos = World.GetComponent<CPosition>(entID);
                    bow = World.GetComponent<CBow>(entID);

                    targetPos = World.GetComponent<CPosition>(AI.TargetID);

                    EntityFactory.CreateArrow(pos.Centre.X, pos.Centre.Y, bow.ArrowSpeed, bow.ArrowDamage, targetPos, team);
                    break;
                }
            }
        }
    }
}