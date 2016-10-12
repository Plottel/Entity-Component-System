using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System that handles AI for the Player team. This inherits from the Enemy AI System
    /// because Player AI have the same core functionality; with the added feature of finding targets.
    /// </summary>
    public class PlayerAISystem : EnemyAISystem
    {
        /// <summary>
        /// This System holds all Enemy team Entities. This speeds up processing as
        /// Entities have already been vetted and will not need to be fetched fresh from the World.
        /// </summary>
        private EntityHolderSystem _enemies;

        /// <summary>
        /// Maps Entity IDs to their current coordinates. This is populated once per frame and is used
        /// by Player AI Entities to determine the closest target.
        /// </summary>
        Dictionary<int, Point2D> _enemyPositions = new Dictionary<int, Point2D>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PlayerAISystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PlayerAISystem (World world) : base (world)
        {
            /// <summary>
            /// The Player AI System's Component Masks are:
            /// Include - CAI, CPlayerTeam
            /// Exclude - CENemyTeam
            /// </summary>
            /// <param name="entID">Ent identifier.</param>
            Include.Remove(typeof(CEnemyTeam));
            Include.Add(typeof(CPlayerTeam));

            _enemies = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam), typeof(CHealth)}, new List<Type> {}, world);
            World.AddSystem(_enemies);
        }

        /// <summary>
        /// Checks if the Entity to be removed is a target of any AI. If it is, the AI is set to determine a new target.
        /// </summary>
        /// <param name="entID">The Entity to remove.</param>
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

        /// <summary>
        /// Populates the Enemy Positions dictionary with the current positions of each Enemy Entity.
        /// </summary>
        /// <returns>The enemy positions.</returns>
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

        /// <summary>
        /// Evaluates the Player AI's current position against the positions of each Enemy Entity.
        /// The Enemy which is closest to the Player AI becomes the Player AI's target.
        /// </summary>
        /// <returns>The closest target.</returns>
        /// <param name="playerAI">The Player AI Entity's AI Component.</param>
        /// <param name="playerPos">The Player AI Entity's Position Component.</param>
        private void GetClosestTarget(CAI playerAI, CPosition playerPos)
        {
            /// <summary>
            /// How far away the Enemy Entity is on the x-axis.
            /// </summary>
            float xOffset;

            /// <summary>
            /// How far away the Enemy Entity is on the y-axis.
            /// </summary>
            float yOffset;

            /// <summary>
            /// The direct diagonal distance from the Enemy Entity.
            /// </summary>
            float distance;

            /// <summary>
            /// The Enemy Entity closest to the Player AI. This will become the AI's target.
            /// </summary>
            int closestTarget;

            /// <summary>
            /// Represents the Entities to be considered as targets and their distance from the Player AI.
            /// </summary>
            Dictionary<int, float> _targetDistances = new Dictionary<int, float>(); 

            foreach (KeyValuePair<int, Point2D> enemy in _enemyPositions)
            {
                xOffset = enemy.Value.X - playerPos.X;
                yOffset = enemy.Value.Y - playerPos.Y;
                distance = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

                //Only consider Entities which are in range for the AI to attack.
                if (distance <= 800)
                {
                    _targetDistances.Add(enemy.Key, distance);
                }
            }

            //If there are targets to consider.
            if (_targetDistances.Count > 0)
            {
                /// <summary>
                /// Returns the corresponding key for the smallest value in the dictionary.
                /// </summary>
                closestTarget = closestTarget = _targetDistances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

                playerAI.TargetID = closestTarget;
                playerAI.HasTarget = true;
            }
        }

        /// <summary>
        /// Represents the decision making process for Player AI. AI operates with the following procedure:
        /// - If I don't have a 
        /// </summary>
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