using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling behaviour for the Explosion Men Entities in the program.
    /// This System assigns the Explosion Men a target and, when the Spawn animation is finished, 
    /// this System creates the Explosion Entity.
    /// </summary>
    public class ExplosionManSystem : System
    {
        public ExplosionManSystem (World world) : base (new List<Type> {typeof(CExplosionMan)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Evaluates the state of each Explosion Man. Two key events occur with Explosion Men:
        ///     -When the Spawn Animation finishes, the explosion is created.
        ///     -When the Explosion Animation finishes, the Entity is removed.
        /// This System checks the state and animation of each Explosion Man.
        /// </summary>
        public override void Process()
        {
            CExplosionMan explosion;
            CAnimation anim;
            CPosition pos;

            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                anim = World.GetComponent<CAnimation>(Entities[i]);

                /// <summary>
                /// If one of the two key events are happening.
                /// </summary>
                if (SwinGame.AnimationEnded(anim.Anim))
                {
                    explosion = World.GetComponent<CExplosionMan>(Entities[i]);

                    /// <summary>
                    /// If not ready to explode, the Spawn animation has just finished.
                    /// Therefore, create the explosion.
                    /// </summary>
                    if (!explosion.ReadyToExplode)
                    {
                        explosion.ReadyToExplode = true;
                        pos = World.GetComponent<CPosition>(Entities[i]);

                        /// <summary>
                        /// Update size details as the Entity is now an Explosion.
                        /// </summary>
                        pos.Width = EntityFactory.EXPLOSION_SIZE;
                        pos.Height = EntityFactory.EXPLOSION_SIZE;

                        /// <summary>
                        /// Adjust position details so explosion is in centre of the cell.
                        /// </summary>
                        CollisionCheckSystem collisions = World.GetSystem<CollisionCheckSystem>();
                        Point2D cellPos = collisions.CentreOfCell(explosion.TargetCell);
                        pos.X = cellPos.X - (pos.Width / 2);
                        pos.Y = cellPos.Y - (pos.Height / 2);

                        /// <summary>
                        /// Create explosion animation.
                        /// </summary>
                        anim.Img = SwinGame.BitmapNamed("Explosion");
                        SwinGame.AssignAnimation(anim.Anim, "Explode", anim.AnimScript);

                        /// <summary>
                        /// Add new components to the Entity.
                        /// </summary>
                        World.AddComponent(Entities[i], new CDamagesOnImpact(false));
                        World.AddComponent(Entities[i], new CDamage(EntityFactory.EXPLOSION_DAMAGE));
                        World.AddComponent(Entities[i], new CCollidable());
                    }
                    else //If ready to explode, the Explode animation has just finished. Therefore, remove the Entity.
                    {
                        World.RemoveEntity(Entities[i]);
                    }
                }
            }
        }
    }
}