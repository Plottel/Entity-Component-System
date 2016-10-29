using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class CollisionCheckSystem : System
    {
        private int _numCells;
        private int _cols;
        private int _rows;
        private int _cellSize;
        private Dictionary<int, List<int>> _playerCells = new Dictionary<int, List<int>>(); //Maps cell numbers to list of entity IDs
        private Dictionary<int, List<int>> _enemyCells = new Dictionary<int, List<int>>();
        
        private System _playerEnts;
        private System _enemyEnts;
        private int collisionsChecked;
        private int maxCollisionsChecked;

        public CollisionCheckSystem (World world) : base (new List<Type> {}, new List<Type> {typeof(CExcludeAll)}, world)
        {
            _playerEnts = new EntityHolderSystem(new List<Type> {typeof(CPlayerTeam)}, new List<Type> {}, world);
            _enemyEnts = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam)}, new List<Type> {}, world);

            world.AddSystem(_playerEnts);
            world.AddSystem(_enemyEnts);

            _cellSize = 100;

            _cols = (int)Math.Floor((double)SwinGame.ScreenWidth() / _cellSize);
            _rows = (int)Math.Floor((double)SwinGame.ScreenHeight() / _cellSize);

            _numCells = _cols * _rows;

            for (int i = 0; i < _numCells; i++)
            {
                _playerCells.Add(i, new List<int>());
                _enemyCells.Add(i, new List<int>());
            }
        }

        private bool AreColliding(CPosition p1, CPosition p2)
        {
            return SwinGame.RectanglesIntersect(p1.Rect, p2.Rect);
        }

        private void ClearCells()
        {
            //Empty the buckets and lists
            /*_playerCells.Clear();
            _enemyCells.Clear();

            for (int i = 0; i < _numCells; i++)
            {
                _playerCells.Add(i, new List<int>());
                _enemyCells.Add(i, new List<int>());
            }*/

            foreach (List<int> list in _playerCells.Values)
                list.Clear();

            foreach (List<int> list in _enemyCells.Values)
                list.Clear();
        }

        private void PopulateCells()
        {
            for (int i = 0; i < _playerEnts.Entities.Count; i++)
            {
                AddEntityToPlayerCells(_playerEnts.Entities[i]);
            }

            for (int i = 0; i < _enemyEnts.Entities.Count; i++)
            {
                AddEntityToEnemyCells(_enemyEnts.Entities[i]);
            }
        }

        private void AddEntityToPlayerCells(int ent)
        {
            CPosition pos = World.GetComponentOfEntity(ent, typeof(CPosition)) as CPosition;
            List<int> cellsEntityIsIn = GetCellsEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (i >= 0 && i <= _numCells)
                {
                    if (!_playerCells[i].Contains(ent))
                        _playerCells[i].Add(ent);
                }               
            }
        }

        private void AddEntityToEnemyCells(int ent)
        {
            CPosition pos = World.GetComponentOfEntity(ent, typeof(CPosition)) as CPosition;
            List<int> cellsEntityIsIn = GetCellsEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (i >= 0 && i <= _numCells)
                {
                    if (!_enemyCells[i].Contains(ent))
                        _enemyCells[i].Add(ent);
                }               
            }
        }

        private List<int> GetCellsEntityIsIn(CPosition pos)
        {
            List<int> result = new List<int>();

            result.Add(CellAt(pos.X, pos.Y)); //Top Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y)); //Top Right
            result.Add(CellAt(pos.X, pos.Y + pos.Height)); //Bottom Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y + pos.Height)); //Bottom Right

            return result;
        }

        private int CellAt(float x, float y)
        {
            int col = (int)Math.Floor(x / _cellSize);
            int row = (int)Math.Floor(y / _cellSize);

            return row * _cols + col;
        }

        public override void Process()
        {
            ClearCells();
            PopulateCells();

            CPosition playerPos;
            CPosition enemyPos;

            CCollision playerCollision;
            CCollision enemyCollision;

            collisionsChecked = 0;

            for (int i = 0; i < _numCells; i++)
            {
                foreach (int playerEnt in _playerCells[i])
                {
                    playerPos = World.GetComponentOfEntity(playerEnt, typeof(CPosition)) as CPosition;

                    foreach (int enemyEnt in _enemyCells[i])
                    {
                        collisionsChecked++;

                        enemyPos = World.GetComponentOfEntity(enemyEnt, typeof(CPosition)) as CPosition;     

                        if (AreColliding(playerPos, enemyPos))
                        {
                            if (!World.EntityHasComponent(playerEnt, typeof(CCollision)))
                            {
                                World.AddComponentToEntity(playerEnt, new CCollision(enemyEnt));
                            }
                            else
                            {
                                playerCollision = World.GetComponentOfEntity(playerEnt, typeof(CCollision)) as CCollision;
                                playerCollision.CollidedWith.Add(enemyEnt);
                            }

                            if (!World.EntityHasComponent(enemyEnt, typeof(CCollision)))
                            {
                                World.AddComponentToEntity(enemyEnt, new CCollision(playerEnt));
                            }
                            else
                            {
                                enemyCollision = World.GetComponentOfEntity(enemyEnt, typeof(CCollision)) as CCollision;
                                enemyCollision.CollidedWith.Add(playerEnt);
                            }
                        }
                    }
                }
            }

            if (collisionsChecked > maxCollisionsChecked)
                maxCollisionsChecked = collisionsChecked;
            
            SwinGame.DrawText("Collisions Checked: " + maxCollisionsChecked, Color.Black, 100, 200);
        }
    }
}