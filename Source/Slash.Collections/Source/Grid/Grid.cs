// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Grid.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Grid
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class Grid<T> : IEnumerable
    {
        #region Fields

        private T[,] grid;

        #endregion

        #region Constructors and Destructors

        public Grid(int width, int height)
        {
            this.grid = new T[width,height];
        }

        public Grid(T[,] grid)
        {
            this.grid = grid;
        }

        public Grid(Grid<T> grid)
        {
            this.grid = new T[grid.Width,grid.Height];

            for (int i = 0; i < grid.Width; i++)
            {
                for (int j = 0; j < grid.Height; j++)
                {
                    this.grid[i, j] = grid[i, j];
                }
            }
        }

        #endregion

        #region Public Properties

        public int Height
        {
            get
            {
                return this.grid.GetUpperBound(1) + 1;
            }
        }

        public int Width
        {
            get
            {
                return this.grid.GetUpperBound(0) + 1;
            }
        }

        #endregion

        #region Public Indexers

        public T this[int x, int y]
        {
            get
            {
                return this.GetObjectAt(x, y);
            }
            set
            {
                this.SetObjectAt(value, x, y);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Grid<T>)obj);
        }

        public IEnumerator GetEnumerator()
        {
            return this.grid.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return (this.grid != null ? this.grid.GetHashCode() : 0);
        }

        public T GetObjectAt(int x, int y)
        {
            if (x < 0 || x >= this.grid.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(
                    "x",
                    x,
                    string.Format("Value has to be between 0 and {0}, but was {1}.", this.grid.GetLength(0) - 1, x));
            }
            if (y < 0 || y >= this.grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(
                    "y",
                    y,
                    string.Format("Value has to be between 0 and {0}, but was {1}.", this.grid.GetLength(1) - 1, y));
            }

            return this.grid[x, y];
        }

        public void Resize(int newWidth, int newHeight)
        {
            T[,] newGrid = new T[newWidth,newHeight];
            for (int x = 0; x < newWidth && x < this.Width; x++)
            {
                for (int y = 0; y < newHeight && y < this.Height; y++)
                {
                    newGrid[x, y] = this.grid[x, y];
                }
            }
            this.grid = newGrid;
        }

        public void SetObjectAt(T gridObject, int x, int y)
        {
            if (x < 0 || x >= this.grid.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(
                    "x",
                    x,
                    string.Format("Value has to be between 0 and {0}, but was {1}.", this.grid.GetLength(0) - 1, x));
            }
            if (y < 0 || y >= this.grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(
                    "y",
                    y,
                    string.Format("Value has to be between 0 and {0}, but was {1}.", this.grid.GetLength(1) - 1, y));
            }

            this.grid[x, y] = gridObject;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < this.grid.GetLength(0); i++)
            {
                stringBuilder.Append("[ ");

                for (int j = 0; j < this.grid.GetLength(1); j++)
                {
                    stringBuilder.AppendFormat("{0}, ", this.grid[i, j]);
                }

                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.AppendLine("]");
            }

            return stringBuilder.ToString();
        }

        public bool TryGetObject(int x, int y, out T gridObject)
        {
            if (x < 0 || x >= this.grid.GetLength(0) || y < 0 || y >= this.grid.GetLength(1))
            {
                gridObject = default(T);
                return false;
            }

            gridObject = this.grid[x, y];
            return true;
        }

        #endregion

        #region Methods

        protected bool Equals(Grid<T> other)
        {
            bool equal = this.grid.Rank == other.grid.Rank
                         && Enumerable.Range(0, this.grid.Rank)
                                      .All(
                                          dimension => this.grid.GetLength(dimension) == other.grid.GetLength(dimension))
                         && this.grid.Cast<T>().SequenceEqual(other.grid.Cast<T>());

            return equal;
        }

        #endregion
    }
}