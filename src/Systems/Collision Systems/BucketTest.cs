using System;
using System.Collections.Generic;
using System.Collections;
using SwinGameSDK;

namespace MyGame
{
    public class BucketTest
    {
        private int _numCells;
        private int _cols;
        private int _rows;
        private int _cellSize;
        private Dictionary<int, List<CPosition>> _cells;

        public BucketTest ()
        {
            _cells = new Dictionary<int, List<CPosition>>();
        }

        public void DoCollisions()
        {
            foreach (List<CPosition> entitiesInCell in _cells.Values)
            {
                foreach (CPosition pos in entitiesInCell)
                {
                    //Do Collision checking;
                }
            }
        }

        public void Setup()
        {
            _cellSize = 50;

            _cols = (int)Math.Floor((double)SwinGame.ScreenWidth() / _cellSize);
            _rows = (int)Math.Floor((double)SwinGame.ScreenHeight() / _cellSize);

            _numCells = _cols * _rows;

            //Index each bucket and initialise the list
            for (int i = 0; i < _numCells; i++)
                _cells.Add(i, new List<CPosition>());
        }

        private void ClearBuckets()
        {
            //Empty the buckets and lists
            _cells.Clear();

            //Re-initialise the buckets
            for (int i = 0; i < _numCells; i++)
                _cells.Add(i, new List<CPosition>());
        }

        //Adds the entity to each bucket it is in
        private void AddEntityToBuckets(CPosition pos)
        {
            List<int> cellsEntityIsIn = GetCellsEntityIsIn(pos);

            foreach (int i in cellsEntityIsIn)
            {
                if (!_cells[i].Contains(pos))
                    _cells[i].Add(pos);
            }
        }

        //Calculates corresponding bucket for each entity corner
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
    }
}

