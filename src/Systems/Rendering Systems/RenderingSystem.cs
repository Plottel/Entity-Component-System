using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    //Fix this shit up
    public class RenderingSystem: System
    {
        public RenderingSystem(World world) : base(new List<Type> {typeof(CRenderable), typeof(CPosition)}, new List<Type> {typeof(CPlayer)}, world)
        {
        }

        public override void Process()
        {
            CRenderable entRend;
            CPosition entPos;

            //Use components of each entity to draw their bitmaps
            for (int i = 0; i < Entities.Count; i++)
            {
                entRend = World.GetComponent<CRenderable>(Entities[i]);
                entPos = World.GetComponent<CPosition>(Entities[i]);

                SwinGame.DrawBitmap(entRend.Img, entPos.X, entPos.Y);
            }
        }
    }
}