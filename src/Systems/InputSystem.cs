using System;
using SwinGameSDK;

namespace MyGame
{
    public class InputSystem : System
    {
        public InputSystem (World world) : base((int)ComponentType.Player, world)
        {
            
        }

        public override void Process()
        {
            if (Entities.Count != 0)
            {
                Point2D pt = SwinGame.MousePosition();

                if (SwinGame.MouseClicked(MouseButton.LeftButton))
                {
                    EntityFactory.CreatePoisonPool(pt.X - 50, pt.Y - 50, 100);
                }

                if (SwinGame.MouseClicked(MouseButton.RightButton))
                {
                    PositionComponent playerPos = World.GetComponentOfEntity(Entities[0], typeof(PositionComponent)) as PositionComponent;

                    float targetX = SwinGame.MouseX();
                    float targetY = SwinGame.MouseY();

                    EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, targetX, targetY, 3);
                }
            }
        }
    }
}