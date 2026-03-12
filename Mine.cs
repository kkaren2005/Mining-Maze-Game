using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Mine class, which consist of 3 attributes
    /// </summary>
    public class Mine{
        private List<List<Cell>> _cells;
        private int _rows;

        private int _cols;

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="grid"></param>
        public Mine(int rows, int cols, int[,] grid){
            _cells = new List<List<Cell>>();
            _rows = rows;
            _cols = cols;

            // Initialize the cell according to the maze generated
            for (int x = 0; x < rows; x++)
            {
                List<Cell> row = new List<Cell>();
                for (int y = 0; y < cols; y++)
                {
                    if (grid[x, y] == 1){
                        row.Add(new Cell(MaterialType.Dirt, x, y, Program.dirtImage));
                    }else{
                        row.Add(new Cell(MaterialType.Stone, x, y, Program.stoneImage));
                    }
                }
                _cells.Add(row);
            }

            // Initialize neighbors for each cells
            for (int i = 0; i < rows; ++i) {
                for (int j = 0; j < cols; ++j) {
                    // Down neighbor
                    if (j + 1 < cols) {
                        _cells[i][j].Down = _cells[i][j + 1];
                    } else {
                        _cells[i][j].Down = null;
                    }

                    // Up neighbor
                    if (j - 1 >= 0) {
                        _cells[i][j].Up = _cells[i][j - 1];
                    } else {
                        _cells[i][j].Up = null;
                    }

                    // Right neighbor
                    if (i + 1 < rows) {
                        _cells[i][j].Right = _cells[i + 1][j];
                    } else {
                        _cells[i][j].Right = null;
                    }

                    // Left neighbor
                    if (i - 1 >= 0) {
                        _cells[i][j].Left = _cells[i - 1][j];
                    } else {
                        _cells[i][j].Left = null;
                    }
                }
            }
        }

        /// <summary>
        /// This is a property for Cells field
        /// </summary>
        /// <value></value>
        public List<List<Cell>> Cells{
            get { return _cells; }
            set { _cells = value; }
        }

        /// <summary>
        /// This is a readonly property for Rows field
        /// </summary>
        /// <value></value>
        public int Rows{
            get { return _rows; }
        }

        /// <summary>
        /// This is a readonly property for Columns field
        /// </summary>
        /// <value></value>
        public int Cols{
            get { return _cols; }
        }

        /// <summary>
        /// This is a method to generate resource randomly within the mine according to the amount and the type
        /// </summary>
        /// <param name="numberOfResrouce"></param>
        /// <param name="type"></param>
        public void GenerateResource(int numberOfResrouce, MaterialType type){
            Random random = new Random();
            int x;
            int y;
            int num = 0;
             
            do{
                x = random.Next(0, _rows);
                y = random.Next(0, _cols);

                if (x != 0 && y != 0){
                    if (_cells[x][y].Type == MaterialType.Dirt || _cells[x][y].Type == MaterialType.Stone){
                        _cells[x][y].Type = type;
                        if (type == MaterialType.Diamond){
                            _cells[x][y].Strength = 30;
                            _cells[x][y].CellImage = Program.DiamondBlock;
                        } else if (type == MaterialType.Gold){
                            _cells[x][y].Strength = 25;
                            _cells[x][y].CellImage = Program.GoldBlock;
                        } else if (type == MaterialType.Iron){
                            _cells[x][y].Strength = 20;
                            _cells[x][y].CellImage = Program.IronBlock;
                        } else if (type == MaterialType.Coal){
                            _cells[x][y].Strength = 10;
                            _cells[x][y].CellImage = Program.CoalBlock;
                        }
                        
                        num += 1;
                    }
                }
            } while(num < numberOfResrouce);
        }

        /// <summary>
        /// This method is to mark the cell as empty, which is mined by the player
        /// </summary>
        /// <param name="cell"></param>
        public void MarkEmptyCell(Cell cell){
            _cells[cell.X][cell.Y].Mined = true;
            _cells[cell.X][cell.Y].ShowText = false;
        }

        /// <summary>
        /// This method is to generate map which displays the structure of the mine, and player's location
        /// </summary>
        /// <param name="player"></param>
        public void Map(Player player){
            if (player.MapOpen){
                //SplashKit.FillRectangle(Color.Aquamarine, SplashKit.ToWorldX(0), SplashKit.ToWorldY(180), 290, 360);
                SplashKit.DrawBitmap(Program.Map, SplashKit.ToWorldX(3), SplashKit.ToWorldY(190));

                // Fixed map dimensions
                const int mapWidth = 260;
                const int mapHeight = 260;

                // Calculate cell dimensions based on mine size
                int cellWidth = mapWidth / _cols;
                int cellHeight = mapHeight / _rows;

                // Draw the grid
                for (int i = 0; i < _rows; ++i)
                {
                    for (int j = 0; j < _cols; ++j)
                    {
                        // Calculate cell's position
                        double x = SplashKit.ToWorldX(23 + i * cellWidth);  // Column determines X
                        double y = SplashKit.ToWorldY(235 + j * cellHeight);  // Row determines Y

                        // Determine cell color based on material type
                        Color cellColor = _cells[i][j].Type == MaterialType.Stone ? Color.Black : Color.White;

                        // Draw the cell
                        SplashKit.FillRectangle(cellColor, x, y, cellWidth, cellHeight);
                    }
                }
                SplashKit.FillRectangle(Color.Red, SplashKit.ToWorldX(23 + player.Location.X * cellWidth), SplashKit.ToWorldY(235 + player.Location.Y * cellHeight), cellWidth, cellHeight);
            }
        }
        
        /// <summary>
        /// This method is to draw the mine
        /// </summary>
        public void DrawMine(){
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _cols; y++)
                {
                    _cells[x][y].DrawCell();
                }
            }
        }
    }
}