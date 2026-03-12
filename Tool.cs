using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Tool class, which inherits from Item class
    /// </summary>
    public abstract class Tool:Item{
        private int _efficiency;

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="efficiency"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Tool(string name, string description, int value, int efficiency, Bitmap image):base(name, description, value, image){
            _efficiency = efficiency;
        }

        /// <summary>
        /// This is a property for Efficiency field
        /// </summary>
        /// <value></value>
        public int Efficiency{
            get { return _efficiency; }
            set { _efficiency = value; }
        }

        /// <summary>
        /// This is an abstract method, which will show the effect on the cell selected when the tool is used to mine the cell.
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="player"></param>
        /// <param name="selectedCell"></param>
        public abstract void UseEffect(Mine mine, Player player, Cell selectedCell);

        /// <summary>
        /// This method is to upgrade the tool
        /// </summary>
        /// <param name="player"></param>
        public abstract void Upgrade(Player player);
    }
}