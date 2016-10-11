using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class PlayerAISystem : EnemyAISystem
    {
        private EntityHolderSystem _enemies;
        Dictionary<int, Point2D> _enemyPositions = new Dictionary<int, Point2D>();

        public PlayerAISystem (World world) : base (world)
        {
            Include.Remove(typeof(CEnemyTeam));
            Include.Add(typeof(CPlayerTeam));

            _enemies = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam), typeof(CHealth)}, new List<Type> {}, world);
            World.AddSystem(_enemies);
        }

        public override void Remove(int entID)
        {
            CAI playerAI;

            for (int i = 0; i < Entities.Count; i++)
            {
                playerAI = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;

                if (playerAI.TargetID == entID)
                {
                    playerAI.HasTarget = false;
                    playerAI.IsInRange = false;
                }
            }
            base.Remove(entID);
        }

        private void GetEnemyPositions()
        {
            _enemyPositions.Clear();
            CPosition enemyPos;

            foreach (int enemyID in _enemies.Entities)
            {
                enemyPos = World.GetComponentOfEntity(enemyID, typeof(CPosition)) as CPosition;
                _enemyPositions.Add(enemyID, SwinGame.PointAt(enemyPos.Centre.X, enemyPos.Centre.Y));
            }
        }

        private void GetClosestTarget(CAI playerAI, CPosition playerPos)
        {
            float xOffset;
            float yOffset;
            float distance;
            int closestTarget;
            Dictionary<int, float> _targetDistances = new Dictionary<int, float>(); 

            foreach (KeyValuePair<int, Point2D> enemy in _enemyPositions)
            {
                xOffset = enemy.Value.X - playerPos.X;
                yOffset = enemy.Value.Y - playerPos.Y;
                distance = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

                if (distance <= 800)
                {
                    _targetDistances.Add(enemy.Key, distance);
                }
            }

            if (_targetDistances.Count > 0)
            {
                closestTarget = closestTarget = _targetDistances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; //Finds key for smallest value
                playerAI.TargetID = closestTarget;
                playerAI.HasTarget = true;
            }
        }

        public override void Process()
        {
            CAI playerAI;
            CPosition playerPos;
            CAnimation playerAnim;

            GetEnemyPositions();

            for (int i = 0; i < Entities.Count; i++)
            {
                playerAI = World.GetComponentOfEntity(Entities[i], typeof(CAI)) as CAI;
                playerPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (!playerAI.HasTarget)
                {
                    GetClosestTarget(playerAI, playerPos);
                }
                else if (!playerAI.IsInRange)
                {
                    CheckRange(Entities[i], playerAI, playerPos);
                }
                else if (!playerAI.AttackIsReady)
                {
                    CheckCooldown(Entities[i]);
                }
                else
                {
                    playerAnim = World.GetComponentOfEntity(Entities[i], typeof(CAnimation)) as CAnimation;

                    if (SwinGame.AnimationEnded(playerAnim.Anim)) //Attack at end of Attack animation
                    {
                        Attack(Entities[i], "Player");
                        SwinGame.AssignAnimation(playerAnim.Anim, "Still", playerAnim.AnimScript);
                    }
                }
            }
        }
    }
}