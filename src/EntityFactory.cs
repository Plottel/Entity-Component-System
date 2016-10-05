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
            newEntity.Mask = (int)ComponentType.Player | (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Health;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(SwinGame.RGBAColor(50, 50, 50, 140)));
            components.Add(new PositionComponent(5, 5, 100, 590));
            components.Add(new PlayerComponent());
            components.Add(new HealthComponent(99999));

            _world.AddEntity(newEntity, components);           
        }

        public static void CreateWalker(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
            newEntity.Mask = (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Velocity | (int)ComponentType.Health | (int)ComponentType.AI | (int)ComponentType.Damage;

            int speed = 2;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(Color.DarkBlue));
            components.Add(new PositionComponent(x, y, 20));
            components.Add(new VelocityComponent(-speed, 0, speed));
            components.Add(new HealthComponent(3));
            components.Add(new AIComponent(5, 1000, AttackType.Melee));
            components.Add(new DamageComponent(1));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateShooter(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
            newEntity.Mask = (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Velocity | (int)ComponentType.Health | (int)ComponentType.AI | (int)ComponentType.Gun;

            int speed = 1;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(Color.Red));
            components.Add(new PositionComponent(x, y, 30));
            components.Add(new VelocityComponent(-speed, 0, speed));
            components.Add(new HealthComponent(3));
            components.Add(new AIComponent(100, 2000, AttackType.Gun));
            components.Add(new GunComponent(5, 2));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateBullet(float x, float y, int speed, int damage)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
            newEntity.Mask = (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Velocity | (int)ComponentType.Damage | (int)ComponentType.Projectile;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(Color.Black));
            components.Add(new PositionComponent(x, y, 5));
            components.Add(new VelocityComponent(-speed, 0, speed));
            components.Add(new DamageComponent(damage));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateFreezingBullet(float x, float y, float targetX, float targetY, int speed)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
            newEntity.Mask = (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Velocity | (int)ComponentType.Projectile;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(Color.DarkBlue));
            components.Add(new PositionComponent(x, y, 15));


            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * 3;
            float yVel = (yOffset / vectorLength) * 3;

            components.Add(new VelocityComponent(xVel, yVel, speed));
            components.Add(new ProjectileComponent(SwinGame.CreateRectangle(targetX - 5, targetY - 5, 10, 10)));

            _world.AddEntity(newEntity, components);
        }

        public static void CreatePoisonPool(float x, float y, int size)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
            newEntity.Mask = (int)ComponentType.Renderable | (int)ComponentType.Position | (int)ComponentType.Poison | (int)ComponentType.ApplyPoison;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new RenderableComponent(SwinGame.RGBAColor(148, 0, 211, 100)));
            components.Add(new PositionComponent(x, y, size));
            components.Add(new PoisonComponent(1, 5000, _world.GameTime));

            _world.AddEntity(newEntity, components);
        }
    }
}