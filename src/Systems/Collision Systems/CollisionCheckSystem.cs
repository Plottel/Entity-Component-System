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
        private int _cellSize = 120;

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
        private Dictionary<int, List<ulong>> _playerCells = new Dictionary<int, List<ulong>>();

        /// <summary>
        /// Represents the Dictionary containing the Enemy Entities residing in each cell.
        /// Maps a cell ID to a list of Entities residing in that cell.
        /// </summary>
        private Dictionary<int, List<ulong>> _enemyCells = new Dictionary<int, List<ulong>>();

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
            _playerEnts = new EntityHolderSystem(new List<Type> {typeof(CPlayerTeam), typeof(CCollidable)}, new List<Type> {}, world);
            _enemyEnts = new EntityHolderSystem(new List<Type> {typeof(CEnemyTeam), typeof(CCollidable)}, new List<Type> {}, world);
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
                _playerCells.Add(i, new List<ulong>());
                _enemyCells.Add(i, new List<ulong>());
            }
        }

        /// <summary>
        /// Gets the Player Spatial Hash cells and the Entities residing in each cell.
        /// </summary>
        /// <value>A Dictionary mapping Spatial Hash cells to Entities residing in that cell.</value>
        public Dictionary<int, List<ulong>> PlayerCells
        {
            get {return _playerCells;}
        }

        /// <summary>
        /// Gets the Enemy Spatial Hash cells and the Entities residing in each cell.
        /// </summary>
        /// <value>A Dictionary mapping Spatial Hash cells to Entities residing in that cell.</value>
        public Dictionary<int, List<ulong>> EnemyCells
        {
            get {return _enemyCells;}
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
            /// For each Spatial Hash cell, check collisions against Entities on the
            /// other team in the same Spatial Hash cell.
            /// </summary>
            for (int i = 0; i < _numCells; i++)
            {
                foreach (ulong playerEnt in _playerCells[i])
                {
                    playerPos = World.GetComponent<CPosition>(playerEnt);

                    foreach (ulong enemyEnt in _enemyCells[i])
                    {
                        enemyPos = World.GetComponent<CPosition>(enemyEnt);

                        if (AreColliding(playerPos, enemyPos))
                            RegisterCollision(playerEnt, enemyEnt);
                    }
                }
            }
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
        /// Registers a collision between two passed in Entities by adding a 
        /// reference to the other Entity in its Collision Component. If the Entity
        /// does not have a Collision Component, one is added.
        /// </summary>
        /// <param name="entOne">The first Entity in the collision.</param>
        /// <param name="entTwo">The second Entity in the collision..</param>
        private void RegisterCollision(ulong entOne, ulong entTwo)
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
            for (int i = 0; i < _playerEnts.Entities.Count; i++)
                AddEntityToCells(_playerEnts.Entities[i], _playerCells);

            for (int i = 0; i < _enemyEnts.Entities.Count; i++)
                AddEntityToCells(_enemyEnts.Entities[i], _enemyCells);
        }

        /// <summary>
        /// Adds the passed in Entity to each Spatial Hash Player cell it resides in.
        /// </summary>
        /// <param name="entID">The Entity to add to Player cells.</param>
        private void AddEntityToCells(ulong entID, Dictionary<int, List<ulong>> cells)
        {
            CPosition pos = World.GetComponent<CPosition>(entID);
            List<int> cellsEntityIsIn;

            if (pos.Width > _cellSize || pos.Height > _cellSize)
                cellsEntityIsIn = GetCellsBigEntityIsIn(pos);
            else
                cellsEntityIsIn = GetCellsNormalSizeEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (i >= 0 && i < _numCells)
                {
                    if (!cells[i].Contains(entID))
                        cells[i].Add(entID);
                }               
            }
        }

        /// <summary>
        /// Checks each corner of the passed in Position Component and adds the corresponding
        /// cell ID to a List of cell IDs the Entity resides in.
        /// </summary>
        /// <returns>A List of cell IDs the Entity resides in.</returns>
        /// <param name="pos">The Position Component of the Entity.</param>
        private List<int> GetCellsNormalSizeEntityIsIn(CPosition pos)
        {
            List<int> result = new List<int>();

            result.Add(CellAt(pos.X, pos.Y)); //Top Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y)); //Top Right
            result.Add(CellAt(pos.X, pos.Y + pos.Height)); //Bottom Left
            result.Add(CellAt(pos.X + pos.Width, pos.Y + pos.Height)); //Bottom Right

            return result;
        }

        /// <summary>
        /// Checks the Entity in chunks equal to _cellSize and adds the corresponding
        /// cell ID to a list of cell IDs the Entity resides in.
        /// </summary>
        /// <returns>The cells the Entity is in.</returns>
        /// <param name="pos">The position of the Entity.</param>
        private List<int> GetCellsBigEntityIsIn(CPosition pos)
        {
            List<int> result = new List<int>();
            float x;
            float y;
            int numXCells = (int)Math.Ceiling((double)pos.Width / _cellSize);
            int numYCells = (int)Math.Ceiling((double)pos.Height / _cellSize);

            for (int i = 0; i < numXCells; i++)
            {
                //If not checking final X-Axis cell
                if (i < numXCells - 1)
                    x = pos.X + (i * _cellSize);
                else
                    x = pos.X + pos.Width; //Check right edge
                
                for (int j = 0; j < numYCells; j++)
                {
                    //If not checking final Y-Axis cell
                    if (j < numYCells - 1)
                        y = pos.Y + (j * _cellSize);
                    else
                        y = pos.Y + pos.Height; //Check bottom edge

                    result.Add(CellAt(x, y));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the corresponding cell ID for a given x and y coordinate.
        /// </summary>
        /// <returns>The cell ID for a given coordinate.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int CellAt(float x, float y)
        {
            int col = (int)Math.Floor(x / _cellSize);
            int row = (int)Math.Floor(y / _cellSize);

            return row * _cols + col;
        }

        /// <summary>
        /// Gets the centre position of the given cell ID. 
        /// </summary>
        /// <returns>The coordinates of the centre of given cell ID</returns>
        /// <param name="cellID">The cell ID.</param>
        public Point2D CentreOfCell(int cellID)
        {
            int col = cellID % _cols;
            int row = cellID / _cols;

            float x = (col * _cellSize) + (_cellSize / 2);
            float y = (row * _cellSize) + (_cellSize / 2);

            return SwinGame.PointAt(x, y);
        }
    }
}