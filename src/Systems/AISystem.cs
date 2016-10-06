using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class AISystem : System
    {
        private List<int> _playerTeamEnts;
        private List<int> _enemyTeamEnts;

        public AISystem (World world) : base (new List<Type> {typeof(CAI), typeof(CTeam)}, new List<Type> {}, world)
        {
            _playerTeamEnts = new List<int>();
            _enemyTeamEnts = new List<int>();
        }

        public override void Remove(int entID)
        {
            base.Remove(entID);
            CAI toCheckAI;

            foreach (int ID in Entities)
            {
                toCheckAI = World.GetComponentOfEntity(ID, typeof(CAI)) as CAI;

                //If entity to remove was a target of an AI, tell that AI to find another target
                if (toCheckAI.TargetID == entID)
                {
                    toCheckAI.State = AIState.GetTarget;
                }
            }
        }

        public override void Process()
        {
            CAI AIComp;

            SortEntitiesIntoTeams();

            for (int i = 0; i < Entities.Count; i++)
            {
                AIComp = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;

                //Singular if statements rather than else-if to allow AI to update state multiple times per frame
                if (AIComp.State == AIState.GetTarget)
                {
                    if (_playerTeamEnts.Contains(Entities[i]))
                    {
                        if (_enemyTeamEnts.Count > 0)
                        {
                            GetTarget(Entities[i], _enemyTeamEnts);
                        }
                    }
                    else
                    {
                        if (_playerTeamEnts.Count > 0)
                        {
                            GetTarget(Entities[i], _playerTeamEnts);
                        }                       
                    }
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

        //Search for the closest Entity of the opposite team and assign it as TargetID
        private void GetTarget(int needsTarget, List<int> enemyEnts)
        {
            //Maps entity IDs against distances
            Dictionary<int, float> distances = new Dictionary<int, float>();
            distances.Clear();

            CAI needsTargetAI = World.GetComponentOfEntity(needsTarget, typeof(CAI)) as CAI;
            CPosition needsTargetPos = World.GetComponentOfEntity(needsTarget, typeof(CPosition)) as CPosition;
            CPosition enemyPos;

            //Determine distance from each enemy 
            foreach (int enemyID in enemyEnts)
            {
                enemyPos = World.GetComponentOfEntity(enemyID, typeof(CPosition)) as CPosition;

                float xOffset = (enemyPos.X - needsTargetPos.X);
                float yOffset = (enemyPos.Y - needsTargetPos.Y);

                float distance = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

                distances.Add(enemyID, distance);
            }

            int closestTarget = distances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            needsTargetAI.TargetID = closestTarget;
            needsTargetAI.State = AIState.CheckRange;
        }

        private void CheckRange(int entID)
        {
            CAI toCheckAI;
            CPosition toCheckPos;
            CPosition targetPos;
            Rectangle targetRect;
            Rectangle AIRect;

            toCheckAI = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
            toCheckPos = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
            targetPos = World.GetComponentOfEntity(toCheckAI.TargetID, typeof(CPosition)) as CPosition;
            targetRect = targetPos.Rect;

            AIRect = SwinGame.CreateRectangle(toCheckPos.X - toCheckAI.Range, 
                                              toCheckPos.Y - toCheckAI.Range, 
                                              toCheckPos.Width + (toCheckAI.Range * 2), 
                                              toCheckPos.Height + (toCheckAI.Range * 2));

            //If AI is in range
            if (SwinGame.RectanglesIntersect(AIRect, targetRect))
            {
                toCheckAI.State = AIState.CheckCooldown;
                toCheckAI.IsInRange = true;
            }
            else
            {
                toCheckAI.IsInRange = false;
            }
        }

        private void CheckCooldown(CAI toCheckAI)
        {
            if (World.GameTime - toCheckAI.LastAttackTime >= toCheckAI.Cooldown)
            {
                toCheckAI.State = AIState.Ready;
            }
        }

        private void Attack(int entID)
        {
            CAI AIComp;
            CDamage AIDamComp;
            CPosition AIPosComp;
            CGun AIGunComp;
            CHealth targetHealthComp;
            CTeam AITeamComp = World.GetComponentOfEntity(entID, typeof(CTeam)) as CTeam;
            Team AITeam = AITeamComp.Team;

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

                EntityFactory.CreateBullet(AIPosComp.X, AIPosComp.Y, AIGunComp.BulletSpeed, AIGunComp.BulletDamage, new CPosition(targetPos.X, 
                                                                                                                                  targetPos.Y, 
                                                                                                                                  targetPos.Width, 
                                                                                                                                  targetPos.Height), AITeam);
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
                MoveTowardsTarget(entID);
            }
        }

        private void MoveTowardsTarget(int toMove)
        {
            CAI toMoveAI = World.GetComponentOfEntity(toMove, typeof(CAI)) as CAI;
            CVelocity toMoveVel = World.GetComponentOfEntity(toMove, typeof(CVelocity)) as CVelocity;
            CPosition toMovePos = World.GetComponentOfEntity(toMove, typeof(CPosition)) as CPosition;
            CPosition targetPos = World.GetComponentOfEntity(toMoveAI.TargetID, typeof(CPosition)) as CPosition;

            //If not targeting the castle, move on both axes
            if (toMoveAI.TargetID != 0)
            {
                if (toMovePos.X > targetPos.Centre.X) {toMoveVel.DX = -toMoveVel.Speed;} //If to the right of target, move left
                if (toMovePos.X < targetPos.Centre.X) {toMoveVel.DX = toMoveVel.Speed;} //If to the left of target, move right
                if (toMovePos.Y > targetPos.Centre.Y) {toMoveVel.DY = -toMoveVel.Speed;} //If below target, move up
                if (toMovePos.Y < targetPos.Centre.Y) {toMoveVel.DY = toMoveVel.Speed;} //If above target, move down
            }
            else //If targeting the castle, just move on X axis
            {
                if (toMovePos.X > targetPos.Centre.X) {toMoveVel.DX = -toMoveVel.Speed;} //If to the right of target, move left
                if (toMovePos.X < targetPos.Centre.X) {toMoveVel.DX = toMoveVel.Speed;} //If to the left of target, move right
            }
        }

        private void SortEntitiesIntoTeams()
        {
            //Empty the lists so it's fresh each frame
            _playerTeamEnts.Clear();
            _playerTeamEnts.Add(0); //0 is ID for castle because it's made first
            _enemyTeamEnts.Clear();

            CTeam team;

            foreach (int entID in Entities)
            {
                team = World.GetComponentOfEntity(entID, typeof(CTeam)) as CTeam;

                if (team.Team == Team.Player)
                {
                    _playerTeamEnts.Add(entID);
                }
                else
                {
                    _enemyTeamEnts.Add(entID);
                }
            }
        }
    }
}