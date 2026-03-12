using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is a MazeGenerator class which consist of 3 attributes, which is grid, numbers of rows and columns
    /// </summary>
    public class MazeGenerator
    {
        private int[,] grid;

        private int _rows, _cols;
        
        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public MazeGenerator(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            grid = new int[rows, cols];
        }

        /// <summary>
        /// This is a method to generate maze
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void GenerateMaze(int startX, int startY)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((startX, startY));
            grid[startX, startY] = 1;

            while (stack.Count > 0)
            {
                (int x, int y) = stack.Peek();
                List<(int, int)> neighbors = GetUnvisitedNeighbors(x, y);

                if (neighbors.Count > 0)
                {
                    (int nx, int ny) = neighbors[new Random().Next(neighbors.Count)];
                    RemoveWall(x, y, nx, ny);
                    grid[nx, ny] = 1;
                    stack.Push((nx, ny));
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        /// <summary>
        /// This is a method used in GenerateMaze() to get the unvisited neighbours of a cell by returning a list of cells
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private List<(int, int)> GetUnvisitedNeighbors(int x, int y)
        {
            List<(int, int)> neighbors = new List<(int, int)>();
            
            if (x > 1 && grid[x - 2, y] == 0) neighbors.Add((x - 2, y)); // Up
            if (x < _rows - 2 && grid[x + 2, y] == 0) neighbors.Add((x + 2, y)); // Down
            if (y > 1 && grid[x, y - 2] == 0) neighbors.Add((x, y - 2)); // Left
            if (y < _cols - 2 && grid[x, y + 2] == 0) neighbors.Add((x, y + 2)); // Right

            return neighbors;
        }

        /// <summary>
        /// This method is used in GenerateMaze() to remove the wall and mark the cell as path
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void RemoveWall(int x1, int y1, int x2, int y2)
        {
            grid[(x1 + x2) / 2, (y1 + y2) / 2] = 1;
        }

        /// <summary>
        /// This is a readonly property for grid field. It consist of 0 and 1, where 0 is the wall and 1 is the path
        /// </summary>
        /// <value></value>
        public int[,] Grid{
            get { return grid; }
        }
    }
}