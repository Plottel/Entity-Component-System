using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class CollisionCheckSystem : System
    {
        private System _playerEnts;
        private System _enemyEnts;

        public CollisionCheckSystem (World world) : base (new List<Type> {}, new List<Type> {typeof(CExcludeAll)}, world)
        {
            _playerEnts= new EntityHolderSystem(new List<Type> {typeof(CPlayerTeam)}, new List<Type> {}, world);
            _enemyEnts = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam)}, new List<Type> {}, world);

            world.AddSystem(_playerEnts);
            world.AddSystem(_enemyEnts);
        }

        private bool AreColliding(CPosition p1, CPosition p2)
        {
            return SwinGame.RectanglesIntersect(p1.Rect, p2.Rect);
        }

        public override void Process()
        {
            CPosition playerEntPos;
            CCollision playerEntCollision;
            CPosition enemyEntPos;
            CCollision enemyEntCollision;

            for (int i = 0; i < _playerEnts.Entities.Count; i++)
            {
                playerEntPos = World.GetComponentOfEntity(_playerEnts.Entities[i], typeof(CPosition)) as CPosition;

                for (int j = 0; j < _enemyEnts.Entities.Count; j++)
                {
                    enemyEntPos = World.GetComponentOfEntity(_enemyEnts.Entities[j], typeof(CPosition)) as CPosition;

                    //If first collision - add a collision component
                    //Otherwise - add a new entity to the collision list
                    if (AreColliding(playerEntPos, enemyEntPos))
                    {
                        if (!World.EntityHasComponent(_playerEnts.Entities[i], typeof(CCollision)))
                        {
                            World.AddComponentToEntity(_playerEnts.Entities[i], new CCollision(_enemyEnts.Entities[j]));
                        }
                        else
                        {
                            playerEntCollision = World.GetComponentOfEntity(_playerEnts.Entities[i], typeof(CCollision)) as CCollision;
                            playerEntCollision.CollidedWith.Add(_enemyEnts.Entities[j]);
                        }

                        if (!World.EntityHasComponent(_enemyEnts.Entities[j], typeof(CCollision)))
                        {
                            World.AddComponentToEntity(_enemyEnts.Entities[j], new CCollision(_playerEnts.Entities[i]));
                        }
                        else
                        {
                            enemyEntCollision = World.GetComponentOfEntity(_playerEnts.Entities[i], typeof(CCollision)) as CCollision;
                            enemyEntCollision.CollidedWith.Add(_enemyEnts.Entities[j]);
                        }
                    }
                }
            }
        }
    }
}

