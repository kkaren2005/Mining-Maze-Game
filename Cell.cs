#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is a Cell class, which contains 11 attributes
    /// </summary>
    public class Cell{
        private MaterialType _type;

        private int _strength;

        private Cell _up;

        private Cell _down;

        private Cell _left;

        private Cell _right;

        private bool _mined;

        private int _x;

        private int _y;

        private Bitmap _cellImage;

        private bool _showText;

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cellImage"></param>
        public Cell(MaterialType type, int x, int y, Bitmap cellImage){
            _type = type;

            if (type == MaterialType.Dirt){
                _strength = 5;
            } else if (type == MaterialType.Stone){
                _strength = 400;
            } else if (type == MaterialType.Diamond){
                _strength = 50;
            } else if (type == MaterialType.Gold){
                _strength = 30;
            } else if (type == MaterialType.Iron){
                _strength = 20;
            } else if (type == MaterialType.Coal){
                _strength = 10;
            } else {
                _strength = 400;
            }

            // Set its neighbour to null initially
            _up = null;
            _down = null;
            _left = null;
            _right = null;
            _mined = false;
            _x = x;
            _y = y;
            _cellImage = cellImage;
            _showText = false;
        }

        /// <summary>
        /// This is property for Type field
        /// </summary>
        /// <value></value>
        public MaterialType Type{
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// This is property for Strength field
        /// </summary>
        /// <value></value>
        public int Strength{
            get { return _strength; }
            set { _strength = value; }
        }

        /// <summary>
        /// This is property for Cell (Up) field
        /// </summary>
        /// <value></value>
        public Cell Up{
            get { return _up; }
            set { _up = value; }
        }

        /// <summary>
        /// This is property for Cell (Down) field
        /// </summary>
        /// <value></value>
        public Cell Down{
            get { return _down; }
            set { _down = value; }
        }

        /// <summary>
        /// This is property for Cell (Left) field
        /// </summary>
        /// <value></value>
        public Cell Left{
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// This is property for Cell (Right) field
        /// </summary>
        /// <value></value>
        public Cell Right{
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// This is property for Mined status field
        /// </summary>
        /// <value></value>
        public bool Mined{
            get { return _mined; }
            set { _mined = value; }
        }

        /// <summary>
        /// This is property for X position field
        /// </summary>
        /// <value></value>
        public int X{
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// This is property for Y position field
        /// </summary>
        /// <value></value>
        public int Y{
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// This is property for Show Text status field
        /// </summary>
        /// <value></value>
        public bool ShowText{
            get { return _showText; }
            set { _showText = value; }
        }

        /// <summary>
        /// This is property for Cell image field
        /// </summary>
        /// <value></value>
        public Bitmap CellImage{
            get { return _cellImage; }
            set { _cellImage = value; }
        }

        /// <summary>
        /// This is a method to draw the cell according to its mined status and image. Text will be shown to display the strength of cell.
        /// </summary>
        public void DrawCell(){
            
            if (_mined){
                SplashKit.FillRectangle(Color.Black, _x * Program.CELL_SIZE, _y * Program.CELL_SIZE, Program.CELL_SIZE, Program.CELL_SIZE);
            } else {
                SplashKit.DrawBitmap(_cellImage, _x * Program.CELL_SIZE, _y * Program.CELL_SIZE);
            }

            if (_showText){
                SplashKit.DrawText(_strength.ToString(), Color.Black, Program.font, 32, _x * Program.CELL_SIZE + Program.CELL_SIZE/2 - 8, _y * Program.CELL_SIZE + Program.CELL_SIZE/2 - 10);
            }
        }
    }
}