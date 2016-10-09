using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PoisonedSystem : System
    {
        private uint _tickInterval;
        private uint _lastTick;

        public PoisonedSystem (World world) : base(new List<Type> {typeof(CPoison), typeof(CHealth)}, new List<Type> {}, world)
        {
            _tickInterval = 2000;
        }

        public override void Process()
        {
            if (World.GameTime - _lastTick >= _tickInterval)
            {
                _lastTick = World.GameTime;

                CPoison entPoison;
                CHealth entHealth;

                for (int i = 0; i < Entities.Count; i++)
                {
                    entPoison = World.GetComponentOfEntity(Entities[i], typeof(CPoison)) as CPoison;

                    if (!Utils.EffectHasEnded(World.GameTime, entPoison.TimeApplied, entPoison.Duration))
                    {
                        entHealth = World.GetComponentOfEntity(Entities[i], typeof(CHealth)) as CHealth;
                        entHealth.Damage += entPoison.Strength;
                    }
                    else
                    {
                        World.RemoveComponentFromEntity(Entities[i], typeof(CPoison));
                    }
                }
            }
        }
    }
}