using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class InputSystem : System
    {
        public InputSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
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
                    CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;

                    float targetX = SwinGame.MouseX();
                    float targetY = SwinGame.MouseY();

                    EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, targetX, targetY, 3);
                }
            }
        }
    }
}