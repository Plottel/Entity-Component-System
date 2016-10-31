using System;
using System.Collections.Generic;

namespace MyGame
{
    //The entities in this System are Bullets that have an active Collision. 
    //They will apply damage to what they have collided with if it has a health component.

    /// <summary>
    /// Represents the System responsible for handling collisions with Arrows. Each Entity
    /// this System operates on will be an Arrow with a Collision Component. The Arrow will deal
    /// damage to the first Entity it has collided with and then be removed from the World.
    /// </summary>
    public class BulletCollisionHandlerSystem : System
    {
        private List<int> _deadBullets = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.BulletCollisionHandlerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public BulletCollisionHandlerSystem (World world) : base(new List<Type> {typeof(CArrow), typeof(CDamage), typeof(CCollision)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Checks the collisions of each Arrow. If it has collided with something containing a Health Component,
        /// damage is applied and the Arrow is removed from the world;
        /// </summary>
        public override void Process()
        {   
            for (int i = 0; i < Entities.Count; i++)
            {
                CDamage damage;
                CCollision collision;
                CHealth collidedHealth;

                collision = World.GetComponent<CCollision>(Entities[i]);

                //Make sure the Entity still exists.
                if (World.HasEntity(collision.CollidedWith[0]))
                {
                    if (World.EntityHasComponent(collision.CollidedWith[0], typeof(CHealth)))
                    {
                        collidedHealth = World.GetComponent<CHealth>(collision.CollidedWith[0]);
                        damage = World.GetComponent<CDamage>(Entities[i]);

                        collidedHealth.Damage += damage.Damage;

                        _deadBullets.Add(Entities[i]);
                    }
                }                
            }

            //Bullets are destroyed once they've collided - so the Entity list is emptied after being processed
            for (int i = 0; i < _deadBullets.Count; i++)
            {
                World.RemoveEntity(_deadBullets[i]);
            }
        }
    }
}