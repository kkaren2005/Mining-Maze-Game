using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Pickaxe class, which inherits from Tool class.
    /// </summary>
    public class Pickaxe:Tool{

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="efficiency"></param>
        /// <param name="type"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Pickaxe(string name, string description, int value, int efficiency, Bitmap image):base(name, description, value, efficiency, image){

        }

        /// <summary>
        /// This is an override method, which shows the effect on the cell when pickaxe is used to mine the cell
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="player"></param>
        /// <param name="selectedCell"></param>
        public override void UseEffect(Mine mine, Player player, Cell selectedCell)
        {
            mine.Cells[selectedCell.X][selectedCell.Y].Strength -= base.Efficiency; // Decrease cell's strength by its efficiency
            mine.Cells[selectedCell.X][selectedCell.Y].ShowText = true;
            if (mine.Cells[selectedCell.X][selectedCell.Y].Strength <= 0){
                mine.MarkEmptyCell(selectedCell); // If the cell is totally mined
                if (selectedCell.Type == MaterialType.Diamond || selectedCell.Type == MaterialType.Gold || selectedCell.Type == MaterialType.Iron || selectedCell.Type == MaterialType.Coal){
                    player.CollectItem(Program.itemFactory.GetResource(selectedCell.Type)); // If the cell is a resource block, player can collect resource
                    Program.Collect_Resource_Sound.Play(); // Play sound effect (need to be removed for unit testing)
                }
            }
            player.Stamina -= 1;
        }

        public override void Upgrade(Player player){
            if (player.Money >= 50){
                base.Efficiency += 5;
                player.Money -= 50;
                Program.Upgrade_Sound.Play(); // Play sound effect (need to be removed for unit testing)
            }
        }
    }
}               