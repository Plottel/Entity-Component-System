using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling collisions with Arrows. Each Entity
    /// this System operates on will be an Arrow with a Collision Component. The Arrow will deal
    /// damage to the first Entity it has collided with and then be removed from the World.
    /// </summary>
    public class ArrowCollisionHandlerSystem: System
    {
        private List<int> _deadBullets = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.ArrowCollisionHandlerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public ArrowCollisionHandlerSystem (World world) : base(new List<Type> {typeof(CArrow), typeof(CDamage), typeof(CCollision)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Checks the collisions of each Arrow. If it has collided with something containing a Health Component,
        /// damage is applied and the Arrow is removed from the world.
        /// </summary>
        public override void Process()
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                CDamage damage;
                CCollision collision;
                CHealth collidedHealth;

                collision = World.GetComponent<CCollision>(Entities[i]);

                /// <summary>
                /// Apply damage to the first Entity the Arrow has collided with that still exists in
                /// the World and also has a Health Component. If none are found, the Arrow remains live.
                /// </summary>
                for (int j = 0; j < collision.CollidedWith.Count; j++)
                {
                    if (World.HasEntity(collision.CollidedWith[j]))
                    {
                        if (World.EntityHasComponent(collision.CollidedWith[j], typeof(CHealth)))
                        {
                            collidedHealth = World.GetComponent<CHealth>(collision.CollidedWith[j]);
                            damage = World.GetComponent<CDamage>(Entities[i]);
                            collidedHealth.Damage += damage.Damage;

                            World.RemoveEntity(Entities[i]);

                            break;
                        }
                    }          
                }
            }
        }
    }
}