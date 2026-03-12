#nullable disable
using System;
using SplashKitSDK;
using NUnit.Framework;

namespace testCustom
{
    [TestFixture]
    /// <summary>
    /// This is TestTool class, which will test the behaviour of its child class, which is Pickaxe class and Bomb class. The two abtract method (UseEffect() and Upgrade()) in Tool class will be tested.
    /// </summary>
    public class TestTool{
        [Test]
        /// <summary>
        /// This test is to test the use effect of pickaxe on selected cell
        /// </summary>
        public void TestUsePickaxe(){
            Pickaxe pickaxe = new Pickaxe("Iron Pickaxe", "Mine", 10, 1, null); // null is for the Bitmap

            // initialize the maze as 3x3, all of them are dirt
            int[,] grid = new int[3, 3];
            for (int i = 0; i < 3; i++){
                for (int j = 0 ; j < 3; j++){
                    grid[i, j] = 1;
                }
            }

            // Initiate mine and player object
            Mine mine = new Mine(3, 3, grid);
            Player player = new Player();

            // Set player's tool selected as pickaxe
            player.ToolSelected = pickaxe;

            // Set selected cell to be mined is the centre of the mine
            Cell selectedCell = mine.Cells[1][1];

            // Execute UseEffect() method to mine the cell
            pickaxe.UseEffect(mine, player, selectedCell); // cell strength now is 4
            Assert.AreEqual(4, selectedCell.Strength);
            pickaxe.UseEffect(mine, player, selectedCell); // cell strength now is 3
            Assert.AreEqual(3, selectedCell.Strength);
            pickaxe.UseEffect(mine, player, selectedCell); // cell strength now is 2
            Assert.AreEqual(2, selectedCell.Strength);
            pickaxe.UseEffect(mine, player, selectedCell); // cell strength now is 1
            Assert.AreEqual(1, selectedCell.Strength);
            pickaxe.UseEffect(mine, player, selectedCell); // cell strength now is 0
            Assert.AreEqual(0, selectedCell.Strength);

            Assert.AreEqual(95, player.Stamina); // Player's stamina should deduct by 5
            Assert.IsTrue(mine.Cells[1][1].Mined); // Dirt will be mined
        }

        [Test]
        /// <summary>
        /// This test is to test the use effect of bomb on selected cell and cells surrounded it 
        /// </summary>
        public void TestUseBomb(){
            Bomb bomb = new Bomb("Bomb", "BOOM", 10, 10, null); // null is for the Bitmap

            // initialize the maze as 3x3, all of them are dirt
            int[,] grid = new int[3, 3];
            for (int i = 0; i < 3; i++){
                for (int j = 0 ; j < 3; j++){
                    grid[i, j] = 1;
                }
            }

            // Initiate mine and player object
            Mine mine = new Mine(3, 3, grid);
            Player player = new Player();

            // Set player's tool selected as bomb
            player.ToolSelected = bomb;

            // Set selected cell to be mined is the centre of the mine
            Cell selectedCell = mine.Cells[1][1];

            // Execute UseEffect() method to mine the cell
            bomb.UseEffect(mine, player, selectedCell);

            Assert.AreEqual(99, player.Stamina); // Player's stamina should deduct 1
            
             // All dirt will be mined (selected cell + cells surround it)
            for (int i = 0; i < 3; i++){
                for (int j = 0 ; j < 3; j++){
                    Assert.IsTrue(mine.Cells[i][j].Mined);
                }
            }
        }

        [Test]
        /// <summary>
        /// This test is to test the upgrade of the pickaxe
        /// </summary>
        public void TestUpgradePickaxe(){
            // Initiate pickaxe and player object
            Pickaxe pickaxe = new Pickaxe("Iron Pickaxe", "Mine", 10, 1, null); // null is for the Bitmap
            Player player = new Player();

            // Upgrade the pickaxe
            pickaxe.Upgrade(player);

            Assert.AreEqual(6, pickaxe.Efficiency); // Efficiency increase by 5
            Assert.AreEqual(50, player.Money); // Player's money will deduct by 50
        }

        [Test]
        /// <summary>
        /// This test is to test the upgrade of the bomb
        /// </summary>
        public void TestUpgradeBomb(){
            // Initiate pickaxe and player object
            Bomb bomb = new Bomb("Bomb", "BOOM", 10, 10, null); // null is for the Bitmap
            Player player = new Player();

            // Upgrade the pickaxe
            bomb.Upgrade(player);

            Assert.AreEqual(20, bomb.Efficiency); // Efficiency increase by 10
            Assert.AreEqual(50, player.Money); // Player's money will deduct by 50
        }
    }
}