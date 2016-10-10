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

        public static Bitmap CreatePlayerImg()
        {
            Bitmap result = SwinGame.CreateBitmap(100, 590);
            SwinGame.ClearSurface(result, SwinGame.RGBAColor(50, 50, 40, 140));

            return result;
        }

        public static Bitmap CreateWalkerImg()
        {
            Bitmap result = SwinGame.CreateBitmap(20, 20);
            SwinGame.ClearSurface(result, Color.DarkBlue);

            return result;
        }

        public static Bitmap CreateShooterImg()
        {
            Bitmap result = SwinGame.CreateBitmap(30, 30);
            SwinGame.ClearSurface(result, Color.Red);

            return result;
        }

        public static Bitmap CreateArrowImg()
        {
            Bitmap result = SwinGame.CreateBitmap(5, 5);
            SwinGame.ClearSurface(result, Color.Black);

            return result;
        }

        public static Bitmap CreateFreezingBulletImg()
        {
            Bitmap result = SwinGame.CreateBitmap(15, 15);
            SwinGame.ClearSurface(result, Color.MediumAquamarine);

            return result;
        }

        public static Bitmap CreatePoisonPoolImg()
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
            components.Add(new CHealth(200));
            components.Add(new CPlayerTeam());

            _world.AddEntity(newEntity, components);           
        }

        public static void CreateWalker(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            int speed = 2;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 21, 30));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(1, 1000, AttackType.Melee, 1)); //Targets the Castle by default
            components.Add(new CDamage(1));
            components.Add(new CLoot(5));
            components.Add(new CEnemyTeam());

            Bitmap walkerBmp = SwinGame.BitmapNamed("SwordMan");
            Animation walkerAnim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("SwordManAnims"));
            AnimationScript walkerAnimScript = SwinGame.AnimationScriptNamed("SwordManAnims");

            components.Add(new CAnimation(walkerBmp, walkerAnim, walkerAnimScript));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateShooter(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();
           
            int speed = 1;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 30));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(100, 2000, AttackType.Gun, 1)); //Targets the Castle by default
            components.Add(new CGun(5, 2));
            components.Add(new CLoot(10));
            components.Add(new CEnemyTeam());
                
            Bitmap archerManBmp = SwinGame.BitmapNamed("ArcherMan");
            Animation archerManAnim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("ArcherManAnims"));
            AnimationScript archerManAnimScript = SwinGame.AnimationScriptNamed("ArcherManAnims");

            components.Add(new CAnimation(archerManBmp, archerManAnim, archerManAnimScript));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateBatteringRam(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Not really 5 - will be much slower
            int speed = 5;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 64, 58));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(1, 5000, AttackType.Melee, 1)); //Targets the Castle by default
            components.Add(new CDamage(5));
            components.Add(new CLoot(20));
            components.Add(new CEnemyTeam());

            Bitmap batteringRamBmp = SwinGame.BitmapNamed("BatteringRam");
            Animation batteringRamAnim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("BatteringRamAnims"));
            AnimationScript batteringRamAnimScript = SwinGame.AnimationScriptNamed("BatteringRamAnims");

            components.Add(new CAnimation(batteringRamBmp, batteringRamAnim, batteringRamAnimScript));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateArrow(float x, float y, int speed, int damage, CPosition target, string team)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(SwinGame.BitmapNamed("Arrow")));

            float xOffset = (target.X - x);
            float yOffset = (target.Y - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * 3;
            float yVel = (yOffset / vectorLength) * 3;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(target.X - 5, target.Y - 5, 10, 10)));
            components.Add(new CPosition(x, y, 5));
            components.Add(new CDamage(damage));

            if (team == "Player")
            {
                components.Add(new CPlayerTeam());
            }
            else
            {
                components.Add(new CEnemyTeam());
            }


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
            components.Add(new CFreezingBullet());

            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * speed;
            float yVel = (yOffset / vectorLength) * speed;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(targetX - 5, targetY - 5, 10, 10)));

            _world.AddEntity(newEntity, components);
        }

        public static void CreateFreezeZone(CPosition pos)
        {
            CreateAnimation(pos.X - 50, 
                            pos.Y - 50, 
                            SwinGame.CreateAnimation("Splash", SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim")), 
                            SwinGame.BitmapNamed("FreezingBulletSplash"), SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim"));

            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(pos.Centre.X - 50, pos.Centre.Y - 50, 100));
            components.Add(new CCollidable());
            components.Add(new CPlayerTeam());
            components.Add(new CAppliesDebuff());
            components.Add(new CFrozen(3000, 0));

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
            components.Add(new CPlayerTeam());
            components.Add(new CCollidable());

            _world.AddEntity(newEntity, components);
        }

        public static void CreateAnimation(float x, float y, Animation anim, Bitmap img, AnimationScript animScript)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            int width = SwinGame.BitmapWidth(img);
            int height = SwinGame.BitmapHeight(img);

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, width, height));
            components.Add(new CAnimation(img, anim, animScript));

            _world.AddEntity(newEntity, components);
        }
    }
}