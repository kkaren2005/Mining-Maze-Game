using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Bomb class, which inherits from Tool class.
    /// </summary>
    public class Bomb:Tool{

        /// <summary>
        /// This is a pass by value constructor, which will set the value of name, description, value, efficiency and image accordingly
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="efficiency"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Bomb(string name, string description, int value, int efficiency, Bitmap image):base(name, description, value, efficiency, image){

        }

        /// <summary>
        /// This method shows the effect of bomb on one cell
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="player"></param>
        /// <param name="selectedCell"></param>
        public void EffectOnOneCell(Mine mine, Player player, Cell selectedCell){
            if (selectedCell != null){
                mine.Cells[selectedCell.X][selectedCell.Y].Strength -= base.Efficiency;
                mine.Cells[selectedCell.X][selectedCell.Y].ShowText = true;
                if (mine.Cells[selectedCell.X][selectedCell.Y].Strength <= 0){
                    mine.MarkEmptyCell(selectedCell);
                    if (selectedCell.Type == MaterialType.Diamond || selectedCell.Type == MaterialType.Gold || selectedCell.Type == MaterialType.Iron || selectedCell.Type == MaterialType.Coal){
                        player.CollectItem(Program.itemFactory.GetResource(selectedCell.Type));
                        Program.Collect_Resource_Sound.Play(); // Play sound effect (need to be removed for unit testing)
                    }
                }
                
                if (!Program.BombSound.IsPlaying){
                    Program.BombSound.Play(); // Play sound effect (need to be removed for unit testing)
                }
            }
        }

        /// <summary>
        /// This method shows the use effect of bomb, which will impact the selected cell and cells surrounding it
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="player"></param>
        /// <param name="selectedCell"></param>
        public override void UseEffect(Mine mine, Player player, Cell selectedCell)
        {
            // mine selected cell and cell around it
            EffectOnOneCell(mine, player, selectedCell);

            if (mine.Cells[selectedCell.X][selectedCell.Y].Left != null){
                if (!mine.Cells[selectedCell.X][selectedCell.Y].Left.Mined)
                EffectOnOneCell(mine, player, selectedCell.Left);

                if (mine.Cells[selectedCell.X][selectedCell.Y].Left.Up != null){
                    if (!mine.Cells[selectedCell.X][selectedCell.Y].Left.Up.Mined)
                    EffectOnOneCell(mine, player, selectedCell.Left.Up);
                }

                if (mine.Cells[selectedCell.X][selectedCell.Y].Left.Down != null){
                    if (!mine.Cells[selectedCell.X][selectedCell.Y].Left.Down.Mined)
                    EffectOnOneCell(mine, player, selectedCell.Left.Down);
                }
            }
            if (mine.Cells[selectedCell.X][selectedCell.Y].Right != null){
                if (!mine.Cells[selectedCell.X][selectedCell.Y].Right.Mined)
                EffectOnOneCell(mine, player, selectedCell.Right);

                if (mine.Cells[selectedCell.X][selectedCell.Y].Right.Up != null){
                    if (!mine.Cells[selectedCell.X][selectedCell.Y].Right.Up.Mined)
                    EffectOnOneCell(mine, player, selectedCell.Right.Up);
                }

                if (mine.Cells[selectedCell.X][selectedCell.Y].Right.Down != null){
                    if (!mine.Cells[selectedCell.X][selectedCell.Y].Right.Down.Mined)
                    EffectOnOneCell(mine, player, selectedCell.Right.Down);
                }
            }
            if (mine.Cells[selectedCell.X][selectedCell.Y].Up != null){
                if (!mine.Cells[selectedCell.X][selectedCell.Y].Up.Mined)
                EffectOnOneCell(mine, player, selectedCell.Up);
            }
            if (mine.Cells[selectedCell.X][selectedCell.Y].Down != null){
                if (!mine.Cells[selectedCell.X][selectedCell.Y].Down.Mined)
                EffectOnOneCell(mine, player, selectedCell.Down);
            }

            player.Stamina -= 1;
            player.ToolSelected = null;
            player.RemoveItem(this); // Bomb is one time used only
        }

        public override void Upgrade(Player player)
        {
            if (player.Money >= 50){
                base.Efficiency += 10;
                player.Money -= 50;
                Program.Upgrade_Sound.Play(); // Play sound effect (need to be removed for unit testing)
            }
        }
    }
}