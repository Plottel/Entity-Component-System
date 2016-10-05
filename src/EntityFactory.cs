using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public static class EntityFactory
    {
        private static World _world;

        public static World World
        {
            set {_world = value;}
        }

        public static void CreatePlayer()
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(SwinGame.RGBAColor(50, 50, 50, 140)));
            components.Add(new CPosition(5, 5, 100, 590));
            components.Add(new CPlayer());
            components.Add(new CHealth(99999));

            _world.AddEntity(newEntity, components);           
        }

        public static void CreateWalker(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            int speed = 2;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(Color.DarkBlue));
            components.Add(new CPosition(x, y, 20));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(5, 1000, AttackType.Melee));
            components.Add(new CDamage(1));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateShooter(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
           
            int speed = 1;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(Color.Red));
            components.Add(new CPosition(x, y, 30));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(100, 2000, AttackType.Gun));
            components.Add(new CGun(5, 2));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateBullet(float x, float y, int speed, int damage)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(Color.Black));
            components.Add(new CPosition(x, y, 5));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CProjectile(SwinGame.CreateRectangle(0, 0, 0, 0)));
            components.Add(new CDamage(damage));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateFreezingBullet(float x, float y, float targetX, float targetY, int speed)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(Color.DarkBlue));
            components.Add(new CPosition(x, y, 15));


            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * 3;
            float yVel = (yOffset / vectorLength) * 3;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(SwinGame.CreateRectangle(targetX - 5, targetY - 5, 10, 10)));

            _world.AddEntity(newEntity, components);
        }

        public static void CreatePoisonPool(float x, float y, int size)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(SwinGame.RGBAColor(148, 0, 211, 100)));
            components.Add(new CPosition(x, y, size));
            components.Add(new CPoison(1, 5000, _world.GameTime));
            components.Add(new CApplyPoison());

            _world.AddEntity(newEntity, components);
        }
    }
}