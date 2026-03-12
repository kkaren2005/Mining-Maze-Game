#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Monster class, which consist of 1 attribute.
    /// </summary>
    public class Monster{

        private Cell _location;

        /// <summary>
        /// This is default consturctor
        /// </summary>
        public Monster(){
            _location = null;
        }

        /// <summary>
        /// This is property for Location field
        /// </summary>
        /// <value></value>
        public Cell Location{
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// This is a method that the monster will move towards the player
        /// </summary>
        /// <param name="player"></param>
        public void Move(Player player){
            if (_location == null || player.Location == null) return;

            // Determine the direction to move
            int dx = player.Location.X - _location.X;
            int dy = player.Location.Y - _location.Y;

            // Prioritize horizontal or vertical movement
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                // Move horizontally
                if(Math.Sign(dx) == 1){
                    _location = _location.Right;
                } else if (Math.Sign(dx) == -1){
                    _location = _location.Left;
                }
            } else {
                // Move vertically
                if (Math.Sign(dy) == 1){
                    _location = _location.Down;
                } else if (Math.Sign(dy) == -1){
                    _location = _location.Up;
                }
            }
        }

        /// <summary>
        /// This is a method that monster attacks player and player's stamina will decrease
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool Attack(Player player){
            if (_location == player.Location){
                player.Stamina -= 30;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is a method that monster steals player's item in the inventory
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool Steal(Player player){
            if (_location == player.Location && player.Inventory.ItemsCount > 1){
                Random random = new Random();
                int index = random.Next(1, player.Inventory.ItemsCount);
                player.RemoveItem(player.Inventory.Items[index]);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is a method to draw the monster
        /// </summary>
        public void DrawMonster(){
            SplashKit.DrawBitmap(Program.MonsterImage, _location.X * Program.CELL_SIZE, _location.Y * Program.CELL_SIZE);
        }
    }
}