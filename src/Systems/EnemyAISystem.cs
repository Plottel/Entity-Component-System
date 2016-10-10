using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
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

            for (int i = 0; i < Entities.Count; i++)
            {
                enemyAI = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;
                enemyPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (!enemyAI.IsInRange)
                {
                    CheckRange(Entities[i], enemyAI, enemyPos);
                }
                else if (!enemyAI.AttackIsReady)
                {
                    CheckCooldown(Entities[i]);
                }
                else
                {
                    enemyAnim = World.GetComponentOfEntity(Entities[i], typeof(CAnimation)) as CAnimation;

                    if (SwinGame.AnimationEnded(enemyAnim.Anim)) //Attack at end of Attack Animation so it visually makes sense
                    {
                        Attack(Entities[i]);
                        SwinGame.AssignAnimation(enemyAnim.Anim, "Still", enemyAnim.AnimScript);
                    }
                }
            }
        }

        private void CheckRange(int entID, CAI enemyAI, CPosition enemyPos)
        {
            Circle enemyAttackRadius = SwinGame.CreateCircle(enemyPos.Centre.X, enemyPos.Centre.Y, enemyAI.Range + (enemyPos.Width / 2));
            CPosition targetPos = World.GetComponentOfEntity(enemyAI.TargetID, typeof(CPosition)) as CPosition;

            enemyAI.IsInRange = SwinGame.CircleRectCollision(enemyAttackRadius, targetPos.Rect);

            if (enemyAI.IsInRange)
            {
                World.RemoveComponentFromEntity(entID, typeof(CVelocity));
            }
        }

        private void CheckCooldown(int entID)
        {
            CAI enemyAI = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;

            enemyAI.AttackIsReady = World.GameTime - enemyAI.LastAttackTime >= enemyAI.Cooldown;

            if (enemyAI.AttackIsReady)
            {
                CAnimation enemyAnim = World.GetComponentOfEntity(entID, typeof(CAnimation)) as CAnimation;
                SwinGame.AssignAnimation(enemyAnim.Anim, "Attack", enemyAnim.AnimScript);
            }
        }

        private void Attack(int entID)
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
                                                                                                                                                10), "Enemy");
                    break;
                }
            }
        }
    }
}