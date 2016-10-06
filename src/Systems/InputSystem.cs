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
                CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;

                if (SwinGame.MouseClicked(MouseButton.LeftButton))
                {
                    EntityFactory.CreatePoisonPool(pt.X, pt.Y);
                }

                if (SwinGame.MouseClicked(MouseButton.RightButton))
                {
                    float targetX = SwinGame.MouseX();
                    float targetY = SwinGame.MouseY();

                    EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, targetX, targetY, 3);
                }

                if (SwinGame.KeyTyped(KeyCode.WKey))
                {
                    EntityFactory.CreateWalker(150, 300, new CTeam(Team.Player));
                }

                if (SwinGame.KeyTyped(KeyCode.SKey))
                {
                    EntityFactory.CreateShooter(150, 300, new CTeam(Team.Player));
                }
            }
        }
    }
}