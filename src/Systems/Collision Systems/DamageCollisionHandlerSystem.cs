using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling collisions where damage should be dealt.
    /// Each Entity this System operates on deals damage on impact. After damage has been dealt,
    /// the Entity is removed from the World.
    /// </summary>
    public class DamageCollisionHandlerSystem: System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.DamageCollisionHandlerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public DamageCollisionHandlerSystem (World world) : base(new List<Type> {typeof(CDamagesOnImpact), typeof(CDamage), typeof(CCollision)}, 
                                                                new List<Type> {}, 
                                                                world)
        {
        }

        /// <summary>
        /// Checks the collisions of each Entity which deals damage on impact. If it has collided with something containing a Health Component,
        /// an Attack is registered with the Damage System.
        /// </summary>
        public override void Process()
        {
            /// <summary>
            /// The System where Attacks are registered.
            /// </summary>
            DamageSystem damageSystem = World.GetSystem<DamageSystem>();

            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                CDamage damage;
                CCollision collision;
                CDamagesOnImpact dmgOnImp;

                collision = World.GetComponent<CCollision>(Entities[i]);

                /// <summary>
                /// Apply damage to the first Entity the Entity has collided with that still exists in
                /// the World and also has a Health Component. If none are found, the Arrow remains live.
                /// </summary>
                for (int j = 0; j < collision.Count; j++)
                {
                    if (World.HasEntity(collision[j]))
                    {
                        if (World.EntityHasComponent(collision[j], typeof(CHealth)))
                        {
                            damage = World.GetComponent<CDamage>(Entities[i]);
                            damageSystem.RegisterAttack(collision[j], damage.Damage);
                        }
                    }          
                }

                /// <summary>
                /// If the Entity is set to die after impact, remove it from the World.
                /// </summary>
                dmgOnImp = World.GetComponent<CDamagesOnImpact>(Entities[i]);

                if (dmgOnImp.DiesAfterImpact)
                    World.RemoveEntity(Entities[i]);
            }
        }
    }
}