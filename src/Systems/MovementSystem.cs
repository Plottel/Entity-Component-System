using System;

namespace MyGame
{
    public class MovementSystem : System
    {
        private int _exclusionMask;

        public MovementSystem (World world) : base((int)ComponentType.Position | (int)ComponentType.Velocity, world)
        {
            _exclusionMask = (int)ComponentType.NotMoving;
        }

        public override bool EntityHasRequiredComponents(int entMask)
        {
            //Movement System will NOT operate on Entities with a NotMoving component
            return (base.EntityHasRequiredComponents(entMask)) && ((entMask & _exclusionMask) != _exclusionMask);
        }

        public override void Process()
        {
            PositionComponent posComp;
            VelocityComponent velComp;

            for (int i = 0; i < Entities.Count; i++)
            {
                posComp = World.GetComponentOfEntity(Entities[i], typeof(PositionComponent)) as PositionComponent;
                velComp = World.GetComponentOfEntity(Entities[i], typeof(VelocityComponent)) as VelocityComponent;

                posComp.X += velComp.DX;
                posComp.Y += velComp.DY;
            }
        }
    }
}