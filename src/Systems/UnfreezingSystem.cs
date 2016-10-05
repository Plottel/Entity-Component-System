using System;
namespace MyGame
{
    public class UnfreezingSystem : System
    {
        public UnfreezingSystem (World world) : base((int)ComponentType.NotMoving, world)
        {
        }

        private bool StillActive(NotMovingComponent notMoving)
        {
            return !(World.GameTime - notMoving.TimeApplied >= notMoving.Duration);
        }

        public override void Process()
        {
            NotMovingComponent notMoving;

            for (int i = 0; i < Entities.Count; i++)
            {
                notMoving = World.GetComponentOfEntity(Entities[i], typeof(NotMovingComponent)) as NotMovingComponent;

                if (!StillActive(notMoving))
                {
                    World.RemoveComponentFromEntity(Entities[i], typeof(NotMovingComponent));
                }
            }
        }
    }
}