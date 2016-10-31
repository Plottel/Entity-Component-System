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
        /// Populates the Enemy Positions dictionary with the current positions of each Enemy Entity.
        /// </summary>
        private void GetEnemyPositions()
        {
            _enemyPositions.Clear();
            CPosition enemyPos;

            foreach (int enemyID in _enemies.Entities)
            {
                enemyPos = World.GetComponent<CPosition>(enemyID);
                _enemyPositions.Add(enemyID, SwinGame.PointAt(enemyPos.Centre.X, enemyPos.Centre.Y));
            }
        }

        /// <summary>
        /// Evaluates the Player AI's current position against the positions of each Enemy Entity.
        /// The Enemy which is closest to the Player AI becomes the Player AI's target.
        /// </summary>
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
                closestTarget = _targetDistances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

                if (World.EntityHasComponent(closestTarget, typeof(CPlayerTeam)))
                {
                    
                }

                playerAI.TargetID = closestTarget;
                playerAI.HasTarget = true;
            }
        }

        /// <summary>
        /// Represents the decision making process for Player AI. AI operates with the following procedure:
        /// - If I don't have a target, get a target.
        /// - If I have a target, check if I'm in range to attack.
        /// - If I'm in range to attack, check if I can attack.
        /// - If I'm in range to attack, attack.
        /// </summary>
        public override void Process()
        {
            CAI playerAI;
            CPosition playerPos;
            CAnimation playerAnim;

            /// <summary>
            /// Populates the dictionary of Enemy positions only once per frame.
            /// Each AI uses these positions to evaluate their targets.
            /// </summary>
            GetEnemyPositions();

            /// <summary>
            /// For each Player Entity.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                playerAI = World.GetComponent<CAI>(Entities[i]);
                playerPos = World.GetComponent<CPosition>(Entities[i]);

                if (!World.HasEntity(playerAI.TargetID))
                {
                    playerAI.HasTarget = false;

                    //This is needed to prevent AI from disappearing, but is buggy.
                    //CAnimation anim = World.GetComponent<CAnimation>(Entities[i]);
                    //SwinGame.AssignAnimation(anim.Anim, "Still", anim.AnimScript);
                }

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
                    /// <summary>
                    /// If ready to attack, start the attack animation. The attack will be carried out
                    /// when the attack animation has finished.
                    /// </summary>
                    playerAnim = World.GetComponent<CAnimation>(Entities[i]);

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