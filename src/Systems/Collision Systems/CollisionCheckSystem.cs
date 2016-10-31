using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for detecting all collisions in the program.
    /// Entities are divided by team and collisions are only checked between Entities on opposite teams.
    /// This System makes use of Spatial Hashing to greatly reduce the number of collisions checked.
    /// </summary>
    public class CollisionCheckSystem : System
    {
        /// <summary>
        /// The size of each cell in the Spatial Hash. Each cell is a square.
        /// </summary>
        private int _cellSize = 100;

        /// <summary>
        /// The number of cells in the Spatial Hash.
        /// </summary>
        private int _numCells;

        /// <summary>
        /// The number of columns in the Spatial Hash.
        /// </summary>
        private int _cols;

        /// <summary>
        /// The number of rows in the Spatial Hash.
        /// </summary>
        private int _rows;

        /// <summary>
        /// Represents the Dictionary containing the Player Entities residing in each cell.
        /// Maps a cell ID to a list of Entities residing in that cell.
        /// </summary>
        private Dictionary<int, List<int>> _playerCells = new Dictionary<int, List<int>>();

        /// <summary>
        /// Represents the Dictionary containing the Enemy Entities residing in each cell.
        /// Maps a cell ID to a list of Entities residing in that cell.
        /// </summary>
        private Dictionary<int, List<int>> _enemyCells = new Dictionary<int, List<int>>();

        /// <summary>
        /// This System holds all Player team Entities. This speeds up processing as
        /// Entities have already been vetted and will not need to be fetched fresh from the World.
        /// </summary>
        private EntityHolderSystem _playerEnts;

        /// <summary>
        /// This System holds all Enemy team Entities. This speeds up processing as
        /// Entities have already been vetted and will not need to be fetched fresh from the World.
        /// </summary>
        private EntityHolderSystem _enemyEnts;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CollisionCheckSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public CollisionCheckSystem (World world) : base (new List<Type> {}, new List<Type> {typeof(CExcludeAll)}, world)
        {
            /// <summary>
            /// Initialise Entity Holder Systems and add them to the World.
            /// </summary>
            _playerEnts = new EntityHolderSystem(new List<Type> {typeof(CPlayerTeam)}, new List<Type> {}, world);
            _enemyEnts = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam)}, new List<Type> {}, world);
            world.AddSystem(_playerEnts);
            world.AddSystem(_enemyEnts);

            /// <summary>
            /// Generate Spatial Hash cell details and initialise cells.
            /// </summary>
            /// <value>The player cells.</value>
            _cols = (int)Math.Floor((double)SwinGame.ScreenWidth() / _cellSize);
            _rows = (int)Math.Floor((double)SwinGame.ScreenHeight() / _cellSize);
            _numCells = _cols * _rows;

            for (int i = 0; i < _numCells; i++)
            {
                _playerCells.Add(i, new List<int>());
                _enemyCells.Add(i, new List<int>());
            }
        }

        /// <summary>
        /// Gets the Player Spatial Hash cells and the Entities residing in each cell.
        /// </summary>
        /// <value>A Dictionary mapping Spatial Hash cells to Entities residing in that cell.</value>
        public Dictionary<int, List<int>> PlayerCells
        {
            get {return _playerCells;}
        }

        /// <summary>
        /// Gets the Enemy Spatial Hash cells and the Entities residing in each cell.
        /// </summary>
        /// <value>A Dictionary mapping Spatial Hash cells to Entities residing in that cell.</value>
        public Dictionary<int, List<int>> EnemyCells
        {
            get {return _enemyCells;}
        }

        /// <summary>
        /// Indicates whether or not two Entities are colliding, according to their Position Components.
        /// </summary>
        /// <returns><c>true</c> if the Entities are colliding, <c>false</c> otherwise.</returns>
        /// <param name="p1">The first Entity's Position Component.</param>
        /// <param name="p2">The second Entity's Position Component.</param>
        private bool AreColliding(CPosition p1, CPosition p2)
        {
            return SwinGame.RectanglesIntersect(p1.Rect, p2.Rect);
        }

        /// <summary>
        /// Clears the List of Entities in each Spatial Hash cell. This is done at the start of each frame
        /// so that positions can be re-evaluated.
        /// </summary>
        private void ClearCells()
        {
            for (int i = 0; i < _numCells; i++)
            {
                _playerCells[i].Clear();
                _enemyCells[i].Clear();
            }
        }

        /// <summary>
        /// Populates the Spatial Hash cells with the Entities residing in them.
        /// </summary>
        private void PopulateCells()
        {
            AddCastleToPlayerCells();

            for (int i = 0; i < _playerEnts.Entities.Count; i++)
                AddEntityToPlayerCells(_playerEnts.Entities[i]);

            for (int i = 0; i < _enemyEnts.Entities.Count; i++)
                AddEntityToEnemyCells(_enemyEnts.Entities[i]);
        }

        /// <summary>
        /// Spatial Hash doesn't work well for Entities larger than the size of a cell.
        /// Therefore, the Castle is manually added to each cell it resides in to allow accurate collision checking.
        /// </summary>
        private void AddCastleToPlayerCells()
        {
            for (int i = 0; i < _numCells; i += _cols)
                _playerCells[i].Add(1);
        }

        /// <summary>
        /// Adds the passed in Entity to each Spatial Hash Player cell it resides in.
        /// </summary>
        /// <param name="entID">The Entity to add to Player cells.</param>
        private void AddEntityToPlayerCells(int entID)
        {
            CPosition pos = World.GetComponent<CPosition>(entID);
            List<int> cellsEntityIsIn = GetCellsEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (i >= 0 && i < _numCells)
                {
                    if (!_playerCells[i].Contains(entID))
                        _playerCells[i].Add(entID);
                }               
            }
        }

        /// <summary>
        /// Adds the passed in Entity to each Spatial Hash Enemy cell it resides in.
        /// </summary>
        /// <param name="ent">The Entity to add to Enemy cells.</param>
        private void AddEntityToEnemyCells(int ent)
        {
            CPosition pos = World.GetComponent<CPosition>(ent);
            List<int> cellsEntityIsIn = GetCellsEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (i >= 0 && i <= _numCells - 1)
                {
                    if (!_enemyCells[i].Contains(ent))
                        _enemyCells[i].Add(ent);
                }               
            }
        }

        /// <summary>
        /// Checks each corner of the passed in Position Component and adds the corresponding
        /// cell ID to a List of cell IDs the Entity resides in.
        /// </summary>
        /// <returns>A List of cell IDs the Entity resides in.</returns>
        /// <param name="pos">The Position Component of the Entity.</param>
        private List<int> GetCellsEntityIsIn(CPosition pos)
        {
            List<int> result = new List<int>();

            result.Add(CellAt(pos.X, pos.Y)); //Top Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y)); //Top Right
            result.Add(CellAt(pos.X, pos.Y + pos.Height)); //Bottom Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y + pos.Height)); //Bottom Right

            return result;
        }

        /// <summary>
        /// Gets the corresponding cell ID for a given x and y coordinate.
        /// </summary>
        /// <returns>The cell ID for a given coordinate.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        private int CellAt(float x, float y)
        {
            int col = (int)Math.Floor(x / _cellSize);
            int row = (int)Math.Floor(y / _cellSize);

            return row * _cols + col;
        }

        /// <summary>
        /// Registers a collision between two passed in Entities by adding a 
        /// reference to the other Entity in its Collision Component. If the Entity
        /// does not have a Collision Component, one is added.
        /// </summary>
        /// <param name="entOne">The first Entity in the collision.</param>
        /// <param name="entTwo">The second Entity in the collision..</param>
        private void RegisterCollision(int entOne, int entTwo)
        {
            CCollision entOneCollision;
            CCollision entTwoCollision;

            if (!World.EntityHasComponent(entOne, typeof(CCollision)))
            {
                World.AddComponent(entOne, new CCollision(entTwo));
            }
            else
            {
                entOneCollision = World.GetComponent<CCollision>(entOne);
                entOneCollision.CollidedWith.Add(entTwo);
            }

            if (!World.EntityHasComponent(entTwo, typeof(CCollision)))
            {
                World.AddComponent(entTwo, new CCollision(entOne));
            }
            else
            {
                entTwoCollision = World.GetComponent<CCollision>(entTwo);
                entTwoCollision.CollidedWith.Add(entOne);
            }
        }

        /// <summary>
        /// Checks collisions. If a collision is detected, a Collision Component is added to both
        /// of the colliding Entities. If these Entities already have a Collision Component, a new
        /// EntityID is added to the list of collisions inside the Collision Component.
        /// </summary>
        public override void Process()
        {
            CPosition playerPos;
            CPosition enemyPos;

            /// <summary>
            /// Empty the Spatial Hash cell details from last frame
            /// and populate them with the new positions.
            /// </summary>
            ClearCells();
            PopulateCells();

            /// <summary>
            /// For each Spatial Hash cell.
            /// </summary>
            for (int i = 0; i < _numCells; i++)
            {
                foreach (int playerEnt in _playerCells[i])
                {
                    playerPos = World.GetComponent<CPosition>(playerEnt);

                    foreach (int enemyEnt in _enemyCells[i])
                    {
                        enemyPos = World.GetComponent<CPosition>(enemyEnt);

                        if (AreColliding(playerPos, enemyPos))
                        {
                            RegisterCollision(playerEnt, enemyEnt);
                        }
                    }
                }
            }
        }
    }
}