using System;
using System.Collections.Generic;

namespace MyGame
{
    //The entities in this System are Bullets that have an active Collision. 
    //They will apply damage to what they have collided with if it has a health component.
    public class BulletCollisionHandlerSystem : System
    {
        private List<int> _deadBullets = new List<int>();

        public BulletCollisionHandlerSystem (World world) : base(new List<Type> {typeof(CArrow), typeof(CDamage), typeof(CCollision)}, new List<Type> {}, world)
        {
        }

        //Bullets can only collide with one target - damage will be applied to the first target the bullet hit
        public override void Process()
        {   
            for (int i = 0; i < Entities.Count; i++)
            {
                CDamage bulletDamage;
                CCollision collision;
                CHealth targetHealth;

                collision = World.GetComponent<CCollision>(Entities[i]);

                if (World.HasEntity(collision.CollidedWith[0]))
                {
                    if (World.EntityHasComponent(collision.CollidedWith[0], typeof(CHealth)))
                    {
                        targetHealth = World.GetComponent<CHealth>(collision.CollidedWith[0]);
                        bulletDamage = World.GetComponent<CDamage>(Entities[i]);

                        targetHealth.Damage += bulletDamage.Damage;

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