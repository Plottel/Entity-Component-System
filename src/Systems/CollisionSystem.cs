using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class CollisionSystem : System
    {
        public CollisionSystem (World world) : base (new List<Type> {}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            
        }

        public static bool AreColliding(CPosition p1, CPosition p2)
        {
            Rectangle r1 = SwinGame.CreateRectangle(p1.X, p1.Y, p1.Width, p1.Height);
            Rectangle r2 = SwinGame.CreateRectangle(p2.X, p2.Y, p2.Width, p2.Height);

            return SwinGame.RectanglesIntersect(r1, r2);
        }
    }
}