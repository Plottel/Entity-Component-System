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
            List<int> playerTeamEnts = GetEntitiesOnTeam(Team.Player);
            List<int> enemyTeamEnts = GetEntitiesOnTeam(Team.Enemy);

            ProcessAI(playerTeamEnts, enemyTeamEnts);
            ProcessAI(enemyTeamEnts, playerTeamEnts);
        }

        private List<int> GetEntitiesOnTeam(Team team)
        {
            List<int> result = new List<int>();
            CTeam entTeamComp;
            foreach (int ent in Entities)
            {
                entTeamComp = World.GetComponentOfEntity(ent, typeof(CTeam)) as CTeam;
                if (entTeamComp.Team == team)
                {
                    result.Add(ent);
                }
            }
            return result;
        }

        private void ProcessAI(List<int> allyEnts, List<int> enemyEnts)
        {
            CAI allyAIComp;

            for (int i = 0; i < allyEnts.Count; i++)
            {
                allyAIComp = World.GetComponentOfEntity(allyEnts[i], typeof(CAI)) as CAI;

                if (!World.HasEntity(allyAIComp.TargetID))
                {
                    allyAIComp.State = AIState.GetTarget;
                    allyAIComp.IsInRange = false;
                }

                //Singular if statements rather than switch to allow AI to update state multiple times per frame
                if (allyAIComp.State == AIState.GetTarget)
                {
                    if (enemyEnts.Count > 0) //If there are potential targets
                    {
                        GetTarget(allyEnts[i], enemyEnts); //Get a target
                    }
                    else //Otherwise, do default movement. If enemy, assign target to Caste - If player, run back to Castle
                    {
                        DoDefaultMovement(allyEnts[i]);
                    }
                }

                if (allyAIComp.State == AIState.CheckRange)
                {
                    CheckRange(allyEnts[i]);
                }

                if (allyAIComp.State == AIState.CheckCooldown)
                {
                    CheckCooldown(allyAIComp);
                }

                if (allyAIComp.State == AIState.Ready)
                {
                    Attack(allyEnts[i]);
                }

                UpdateVelocity(allyEnts[i]);
            }
        }

        //Search for the closest Entity of the opposite team and assign it as TargetID
        private void GetTarget(int allyEnt, List<int> enemyEnts)
        {
            float xOffset;
            float yOffset;
            float distance;
            int closestTarget;

            //Maps entity IDs against distances
            Dictionary<int, float> enemyEntDistances = new Dictionary<int, float>();
            enemyEntDistances.Clear(); //Maybe don't need this anymore?

            CAI allyAIComp = World.GetComponentOfEntity(allyEnt, typeof(CAI)) as CAI;
            CPosition allyPos = World.GetComponentOfEntity(allyEnt, typeof(CPosition)) as CPosition;
            CPosition enemyPos;

            //Determine distance from each enemy 
            foreach (int enemyID in enemyEnts)
            {
                enemyPos = World.GetComponentOfEntity(enemyID, typeof(CPosition)) as CPosition;

                xOffset = (enemyPos.X - allyPos.X);
                yOffset = (enemyPos.Y - allyPos.Y);
                distance = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

                enemyEntDistances.Add(enemyID, distance); //Add to list of distances to consider
            }

            closestTarget = enemyEntDistances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; //Finds key for smallest value

            allyAIComp.TargetID = closestTarget;
            allyAIComp.State = AIState.CheckRange;
        }

        private void CheckRange(int toCheck)
        {
            CAI allyAIComp = World.GetComponentOfEntity(toCheck, typeof(CAI)) as CAI;
            CPosition allyPos = World.GetComponentOfEntity(toCheck, typeof(CPosition)) as CPosition;
            Circle allyAttackRadius = SwinGame.CreateCircle(allyPos.Centre.X, allyPos.Centre.Y, allyAIComp.Range + allyPos.Width);
            CPosition enemyPos = World.GetComponentOfEntity(allyAIComp.TargetID, typeof(CPosition)) as CPosition;

            //If AI is in range
            if (SwinGame.CircleRectCollision(allyAttackRadius, enemyPos.Rect))
            {
                allyAIComp.State = AIState.CheckCooldown;
                allyAIComp.IsInRange = true;
            }
            else
            {
                allyAIComp.IsInRange = false;
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
            CAI entAIComp;
            CDamage entDamComp;
            CPosition entPos;
            CGun entGunComp;
            CHealth enemyHealth;
            CTeam entTeamComp = World.GetComponentOfEntity(entID, typeof(CTeam))as CTeam;

            entAIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;

            if (!World.HasEntity(entAIComp.TargetID))
            {
                entAIComp.State = AIState.GetTarget;
                entAIComp.IsInRange = false;
            }
            else
            {
                if (entAIComp.AttackType == AttackType.Melee)
                {
                    entDamComp = World.GetComponentOfEntity(entID, typeof(CDamage)) as CDamage;
                    enemyHealth = World.GetComponentOfEntity(entAIComp.TargetID, typeof(CHealth)) as CHealth;

                    enemyHealth.Damage += entDamComp.Damage;
                }

                if (entAIComp.AttackType == AttackType.Gun)
                {
                    entPos = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
                    entGunComp = World.GetComponentOfEntity(entID, typeof(CGun)) as CGun;

                    CPosition targetPos = World.GetComponentOfEntity(entAIComp.TargetID, typeof(CPosition)) as CPosition;

                    EntityFactory.CreateBullet(entPos.X, entPos.Y, entGunComp.BulletSpeed, entGunComp.BulletDamage, new CPosition(targetPos.Centre.X - 5, 
                                                                                                                              targetPos.Centre.Y - 5, 
                                                                                                                              10, 
                                                                                                                              10), entTeamComp);
                }
            }
            entAIComp.LastAttackTime = World.GameTime;
            entAIComp.State = AIState.CheckRange;
        }

        private void UpdateVelocity(int entID)
        {
            CAI entAIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
            CVelocity entVelComp = World.GetComponentOfEntity(entID, typeof(CVelocity)) as CVelocity;

            if (entAIComp.IsInRange)
            {
                entVelComp.DX = 0;
                entVelComp.DY = 0;
            }
            else
            {
                if (World.HasEntity(entAIComp.TargetID))
                {
                    MoveTowardsTarget(entID);
                }
                else
                {
                    DoDefaultMovement(entID);
                }
            }
        }

        private void MoveTowardsTarget(int toMove)
        {
            CAI toMoveAI = World.GetComponentOfEntity(toMove, typeof(CAI)) as CAI;
            CVelocity toMoveVel = World.GetComponentOfEntity(toMove, typeof(CVelocity)) as CVelocity;
            CPosition toMovePos = World.GetComponentOfEntity(toMove, typeof(CPosition)) as CPosition;
            CPosition targetPos;

            targetPos = World.GetComponentOfEntity(toMoveAI.TargetID, typeof(CPosition)) as CPosition;

            //If not targeting the castle, move on both axes
            if (toMoveAI.TargetID != 1)
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

        private void DoDefaultMovement(int entID)
        {
            CTeam entTeamComp = World.GetComponentOfEntity(entID, typeof(CTeam)) as CTeam;
            if (entTeamComp.Team == Team.Enemy)
            {
                CAI AIComp = World.GetComponentOfEntity(entID, typeof(CAI)) as CAI;
                AIComp.TargetID = 1; //Assign target to the Castle
                AIComp.State = AIState.CheckRange;
            }
            else
            {
                //Move back to the Castle
                CVelocity velComp = World.GetComponentOfEntity(entID, typeof(CVelocity)) as CVelocity;
                CPosition posComp = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;

                if (posComp.X > 200)
                {
                    velComp.DX = -velComp.Speed;
                }
                else
                {
                    velComp.DX = 0;
                }
                velComp.DY = 0;
            }
        }
    }
}