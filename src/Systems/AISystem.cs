using System;
using SwinGameSDK;

namespace MyGame
{
    public class AISystem : System
    {
        public AISystem (World world) : base ((int)ComponentType.AI, world)
        {
        }      

        public override void Process()
        {
            AIComponent AIComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                AIComp = World.GetComponentOfEntity(Entities[i], typeof(AIComponent)) as AIComponent;

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

        private void GetTarget(AIComponent AIComp)
        {
            int playerID = World.GetAllEntitiesWithTag(typeof(PlayerComponent))[0];
            AIComp.TargetID = playerID;

            AIComp.State = AIState.CheckRange;
        }

        private void CheckRange(int entID)
        {
            AIComponent AIComp;
            PositionComponent AIPosComp;
            PositionComponent targetPosComp;
            Rectangle targetRect;
            Rectangle AIRect;

            AIComp = World.GetComponentOfEntity(entID, typeof(AIComponent)) as AIComponent;
            AIPosComp = World.GetComponentOfEntity(entID, typeof(PositionComponent)) as PositionComponent;
            targetPosComp = World.GetComponentOfEntity(AIComp.TargetID, typeof(PositionComponent)) as PositionComponent;

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

        private void CheckCooldown(AIComponent AIComp)
        {
            if (World.GameTime - AIComp.LastAttackTime >= AIComp.Cooldown)
            {
                AIComp.State = AIState.Ready;
            }
        }

        private void Attack(int entID)
        {
            AIComponent AIComp;
            DamageComponent AIDamComp;
            PositionComponent AIPosComp;
            GunComponent AIGunComp;
            HealthComponent targetHealthComp;

            AIComp = World.GetComponentOfEntity(entID, typeof(AIComponent)) as AIComponent;

            if (AIComp.AttackType == AttackType.Melee)
            {
                AIDamComp = World.GetComponentOfEntity(entID, typeof(DamageComponent)) as DamageComponent;
                targetHealthComp = World.GetComponentOfEntity(AIComp.TargetID, typeof(HealthComponent)) as HealthComponent;

                targetHealthComp.Damage += AIDamComp.Damage;
            }

            if (AIComp.AttackType == AttackType.Gun)
            {
                AIPosComp = World.GetComponentOfEntity(entID, typeof(PositionComponent)) as PositionComponent;
                AIGunComp = World.GetComponentOfEntity(entID, typeof(GunComponent)) as GunComponent;

                EntityFactory.CreateBullet(AIPosComp.X, AIPosComp.Y, AIGunComp.BulletSpeed, AIGunComp.BulletDamage);
            }

            AIComp.LastAttackTime = World.GameTime;
            AIComp.State = AIState.CheckRange;
        }

        private void UpdateVelocity(int entID)
        {
            AIComponent AIComp = World.GetComponentOfEntity(entID, typeof(AIComponent)) as AIComponent;
            VelocityComponent velComp = World.GetComponentOfEntity(entID, typeof(VelocityComponent)) as VelocityComponent;

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