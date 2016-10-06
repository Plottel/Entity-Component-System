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

        private static Bitmap CreatePlayerImg()
        {
            Bitmap result = SwinGame.CreateBitmap(100, 590);
            SwinGame.ClearSurface(result, SwinGame.RGBAColor(50, 50, 40, 140));

            return result;
        }

        private static Bitmap CreateWalkerImg()
        {
            Bitmap result = SwinGame.CreateBitmap(20, 20);
            SwinGame.ClearSurface(result, Color.DarkBlue);

            return result;
        }

        private static Bitmap CreateShooterImg()
        {
            Bitmap result = SwinGame.CreateBitmap(30, 30);
            SwinGame.ClearSurface(result, Color.Red);

            return result;
        }

        private static Bitmap CreateBulletImg()
        {
            Bitmap result = SwinGame.CreateBitmap(5, 5);
            SwinGame.ClearSurface(result, Color.Black);

            return result;
        }

        private static Bitmap CreateFreezingBulletImg()
        {
            Bitmap result = SwinGame.CreateBitmap(15, 15);
            SwinGame.ClearSurface(result, Color.MediumAquamarine);

            return result;
        }

        private static Bitmap CreatePoisonPoolImg()
        {
            Bitmap result = SwinGame.CreateBitmap(100, 100);
            SwinGame.ClearSurface(result, SwinGame.RGBAColor(148, 0, 211, 100));

            return result;
        }

        public static void CreatePlayer()
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreatePlayerImg()));
            components.Add(new CPosition(5, 5, 100, 590));
            components.Add(new CPlayer());
            components.Add(new CHealth(100));
            components.Add(new CTeam(Team.Player));

            _world.AddEntity(newEntity, components);           
        }

        public static void CreateWalker(float x, float y, Team team)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            int speed = 2;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateWalkerImg()));
            components.Add(new CPosition(x, y, 20));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(5, 1000, AttackType.Melee));
            components.Add(new CDamage(1));
            components.Add(new CTeam(team));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateShooter(float x, float y, Team team)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
           
            int speed = 1;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateShooterImg()));
            components.Add(new CPosition(x, y, 30));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(100, 2000, AttackType.Gun));
            components.Add(new CGun(5, 2));
            components.Add(new CTeam(team));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateBullet(float x, float y, int speed, int damage, CPosition target, Team team)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateBulletImg()));

            float xOffset = (target.X - x);
            float yOffset = (target.Y - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * 3;
            float yVel = (yOffset / vectorLength) * 3;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(target.X - 5, target.Y - 5, 10, 10)));

            components.Add(new CPosition(x, y, 5));

            components.Add(new CDamage(damage));
            components.Add(new CTeam(team));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateFreezingBullet(float x, float y, float targetX, float targetY, int speed)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateFreezingBulletImg()));
            components.Add(new CPosition(x, y, 15));


            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * 3;
            float yVel = (yOffset / vectorLength) * 3;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(targetX - 5, targetY - 5, 10, 10)));
            components.Add(new CTeam(Team.Player));

            _world.AddEntity(newEntity, components);
        }

        public static void CreatePoisonPool(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreatePoisonPoolImg()));
            components.Add(new CPosition(x - 50, y - 50, 100));
            components.Add(new CPoison(1, 5000, _world.GameTime));
            components.Add(new CAppliesDebuff());
            components.Add(new CTeam(Team.Player));

            _world.AddEntity(newEntity, components);
        }
    }
}