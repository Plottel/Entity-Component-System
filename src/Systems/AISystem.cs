using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class AISystem : System
    {
        public AISystem (World world) : base (new List<Type> {typeof(CAI)}, new List<Type> {}, world)
        {
        }      

        public override void Process()
        {
            CAI AIComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                AIComp = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;

                //Singular if statements rather than else-if to allow AI to update state multiple times per frame
                if (AIComp.State == AIState.GetTarget)
                {
                    GetTarget(AIComp);
                }

                if (AIComp.State == AIState.CheckRange)
                {
                    CheckRange(Entities[i]);
                }

                if (AIComp.State == AIState.CheckCooldown)
                {
                    CheckCooldown(AIComp);
                }

                if (AIComp.State == AIState.Ready)
                {
                    Attack(Entities[i]);
                }

                UpdateVelocity(Entities[i]);
            }
        }

        private void GetTarget(CAI AIComp)
        {
            int playerID = World.GetAllEntitiesWithTag(typeof(CPlayer))[0];
            AIComp.TargetID = playerID;

            AIComp.State = AIState.CheckRange;
        }

        private void CheckRange(int entID)
        {
            CAI AIComp;
            CPosition AIPosComp;
            CPosition targetPosComp;
            Rectangle targetRect;
            Rectangle AIRect;

            AIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
            AIPosComp = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
            targetPosComp = World.GetComponentOfEntity(AIComp.TargetID, typeof(CPosition)) as CPosition;

            targetRect = SwinGame.CreateRectangle(targetPosComp.X, targetPosComp.Y, targetPosComp.Width, targetPosComp.Height);
            AIRect = SwinGame.CreateRectangle(AIPosComp.X - AIComp.Range, 
                                              AIPosComp.Y - AIComp.Range, 
                                              AIPosComp.Width + (AIComp.Range * 2), 
                                              AIPosComp.Height + (AIComp.Range * 2));

            //If AI is in range
            if (SwinGame.RectanglesIntersect(AIRect, targetRect))
            {
                AIComp.State = AIState.CheckCooldown;
                AIComp.IsInRange = true;
            }
            else
            {
                AIComp.IsInRange = false;
            }
        }

        private void CheckCooldown(CAI AIComp)
        {
            if (World.GameTime - AIComp.LastAttackTime >= AIComp.Cooldown)
            {
                AIComp.State = AIState.Ready;
            }
        }

        private void Attack(int entID)
        {
            CAI AIComp;
            CDamage AIDamComp;
            CPosition AIPosComp;
            CGun AIGunComp;
            CHealth targetHealthComp;

            AIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;

            if (AIComp.AttackType == AttackType.Melee)
            {
                AIDamComp = World.GetComponentOfEntity(entID, typeof(CDamage)) as CDamage;
                targetHealthComp = World.GetComponentOfEntity(AIComp.TargetID, typeof(CHealth)) as CHealth;

                targetHealthComp.Damage += AIDamComp.Damage;
            }

            if (AIComp.AttackType == AttackType.Gun)
            {
                AIPosComp = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
                AIGunComp = World.GetComponentOfEntity(entID, typeof(CGun)) as CGun;

                CPosition targetPos = World.GetComponentOfEntity(AIComp.TargetID, typeof(CPosition)) as CPosition;
                Rectangle targetRect = SwinGame.CreateRectangle(targetPos.X, targetPos.Y, targetPos.Width, targetPos.Height);

                EntityFactory.CreateBullet(AIPosComp.X, AIPosComp.Y, AIGunComp.BulletSpeed, AIGunComp.BulletDamage, targetRect);
            }

            AIComp.LastAttackTime = World.GameTime;
            AIComp.State = AIState.CheckRange;
        }

        private void UpdateVelocity(int entID)
        {
            CAI AIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
            CVelocity velComp = World.GetComponentOfEntity(entID, typeof(CVelocity)) as CVelocity;

            if (AIComp.IsInRange)
            {
                velComp.DX = 0;
                velComp.DY = 0;
            }
            else
            {
                velComp.DX = -velComp.Speed;
                velComp.DY = 0;
            }
        }
    }
}