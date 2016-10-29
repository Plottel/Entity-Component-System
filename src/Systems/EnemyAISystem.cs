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
        public EnemyAISystem (World world) : base (new List<Type> {typeof(CAI), typeof(CEnemyTeam)}, new List<Type> {}, world)
        {
        }      

        public override void Process()
        {
            CAI enemyAI;
            CPosition enemyPos;
            CAnimation enemyAnim;

            /// <summary>
            /// For each Enemy AI Entity.
            /// </summary>
            /// <returns>The range.</returns>
            for (int i = 0; i < Entities.Count; i++)
            {
                enemyAI = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;
                enemyPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (!enemyAI.IsInRange)
                {
                    CheckRange(Entities[i], enemyAI, enemyPos);

                    /// <summary>
                    /// If the AI is now in range, stop moving and begin attacking.
                    /// </summary>
                    if (enemyAI.IsInRange)
                        World.RemoveComponentFromEntity(Entities[i], typeof(CVelocity));          
                }
                else if (!enemyAI.AttackIsReady)
                {
                    CheckCooldown(Entities[i]);
                }
                else
                {
                    enemyAnim = World.GetComponentOfEntity(Entities[i], typeof(CAnimation)) as CAnimation;

                    /// <summary>
                    /// Attack will be carried out when the Attack animation has ended.
                    /// </summary>
                    if (SwinGame.AnimationEnded(enemyAnim.Anim))
                    {
                        /// <summary>
                        /// Carry out the attack.
                        /// </summary>
                        Attack(Entities[i], "Enemy");

                        /// <summary>
                        /// If Attacking, stand still during the cooldown period.
                        /// </summary>
                        SwinGame.AssignAnimation(enemyAnim.Anim, "Still", enemyAnim.AnimScript);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a Circle to represent the attack radius of the AI. If the Position Component of the
        /// AI's target is within this Circle, then the AI is in range.
        /// </summary>
        /// <param name="entID">The Entity to check range for.</param>
        /// <param name="enemyAI">AI component of the Entity to check range for.</param>
        /// <param name="enemyPos">Position component of the Entity to check range for.</param>
        protected void CheckRange(int entID, CAI enemyAI, CPosition enemyPos)
        {
            Circle enemyAttackRadius = SwinGame.CreateCircle(enemyPos.Centre.X, enemyPos.Centre.Y, enemyAI.Range + (enemyPos.Width / 2));
            CPosition targetPos = World.GetComponentOfEntity(enemyAI.TargetID, typeof(CPosition)) as CPosition;

            enemyAI.IsInRange = SwinGame.CircleRectCollision(enemyAttackRadius, targetPos.Rect);
        }

        /// <summary>
        /// Checks the Game Time against the last time the AI attacked. If the time difference is greater
        /// than the attack cooldown of the AI, then the AI is ready to attack and their Attack animation begins.
        /// </summary>
        /// <returns>The cooldown.</returns>
        /// <param name="entID">Ent identifier.</param>
        protected void CheckCooldown(int entID)
        {
            CAI enemyAI = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;

            enemyAI.AttackIsReady = World.GameTime - enemyAI.LastAttackTime >= enemyAI.Cooldown;

            if (enemyAI.AttackIsReady)
            {
                /// <summary>
                /// Begin the Attack animation for the AI
                /// </summary>
                CAnimation enemyAnim = World.GetComponentOfEntity(entID, typeof(CAnimation)) as CAnimation;
                SwinGame.AssignAnimation(enemyAnim.Anim, "Attack", enemyAnim.AnimScript);
            }
        }

        protected void Attack(int entID, string team)
        {
            CAI enemyAI = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
            CDamage enemyDamage; 
            CGun enemyGun;
            CPosition enemyPos;

            CHealth targetHealth = World.GetComponentOfEntity(enemyAI.TargetID, typeof(CHealth)) as CHealth;
            CPosition targetPos;

            enemyAI.LastAttackTime = World.GameTime;
            enemyAI.AttackIsReady = false;

            switch (enemyAI.AttackType)
            {
                case AttackType.Melee:
                {
                    enemyDamage = World.GetComponentOfEntity(entID, typeof(CDamage)) as CDamage;

                    targetHealth.Damage += enemyDamage.Damage;
                    break;
                }

                case AttackType.Gun:
                {
                    enemyPos = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
                    enemyGun = World.GetComponentOfEntity(entID, typeof(CGun)) as CGun;

                    targetPos = World.GetComponentOfEntity(enemyAI.TargetID, typeof(CPosition)) as CPosition;

                    EntityFactory.CreateArrow(enemyPos.Centre.X, enemyPos.Centre.Y, enemyGun.BulletSpeed, enemyGun.BulletDamage, new CPosition(targetPos.Centre.X - 5, 
                                                                                                                                                targetPos.Centre.Y - 5, 
                                                                                                                                                10, 
                                                                                                                                                10), team);
                    break;
                }
            }
        }
    }
}