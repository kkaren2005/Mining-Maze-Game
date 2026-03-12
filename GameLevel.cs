#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is GameLevel class, which consist of 10 attributes
    /// </summary>
    public class GameLevel{
        private int _levelNo;

        private int _resourceAmount;

        private int _targetDiamond;

        private int _timeLimit;

        private int _timeLeft;

        private GameStatus _gameStatus;

        private Player _player;

        private Mine _mine;

        private Monster _monster;

        private bool _spawnMonster;

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="levelNo"></param>
        /// <param name="resourceAmount"></param>
        /// <param name="targetDiamond"></param>
        /// <param name="timeLimit"></param>
        public GameLevel(int levelNo, int resourceAmount, int targetDiamond, int timeLimit){
            _levelNo = levelNo;
            _resourceAmount = resourceAmount;
            _targetDiamond = targetDiamond;
            _timeLimit = timeLimit;
            _timeLeft = _timeLimit;
            _gameStatus = GameStatus.Ready;
            _mine = null;
            _player = new Player();
            _monster = new Monster();
            _spawnMonster = true;
        }

        /// <summary>
        /// This is property for game status field
        /// </summary>
        /// <value></value>
        public GameStatus GameStatus{
            get { return _gameStatus; }
            set { _gameStatus = value; }
        }

        /// <summary>
        /// This is a method to initialize the game, which will set all game elements to default value
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void Initialize(int rows, int cols){
            // Initialize the time
            _timeLeft = _timeLimit;
            Program._lastMineTime = DateTime.Now;
            Program._lastMoveTime = DateTime.Now;
            Program._lastRestTime = DateTime.Now;
            Program._secondTick = DateTime.Now;

            // Initialize the mine
            GenerateMaze(rows, cols);

            // Place player at the centre of the mine
            _player.Location = _mine.Cells[(_mine.Rows - 1)/2][(_mine.Cols - 1)/2];
            _mine.MarkEmptyCell(_player.Location);

            // Place monster at the centre of the mine
            _monster.Location = _mine.Cells[(_mine.Rows - 1)/2][(_mine.Cols - 1)/2];

            // Clear player's inventory
            _player.Inventory.Clear();

            // Close the inventory, map and store
            _player.InventoryOpen = false;
            _player.MapOpen = false;
            _player.StoreOpen = false;

            // Initialize player's stamina, money and score
            _player.Stamina = 100;
            _player.Money = 100;
            _player.Score = 0;

            // By default, player will have a pickaxe and a bomb
            Pickaxe pickaxe = Program.itemFactory.GetPickaxe(MaterialType.Iron);
            _player.CollectItem(pickaxe);
            _player.CollectItem(Program.itemFactory.GetBomb());

            // By default, pickaxe is selected to be used
            _player.ToolSelected = pickaxe;

            // By default, no item is selected
            _player.SelectedItem = null;

            // Game start
            _gameStatus = GameStatus.Start;

            // Spawn monster status
            _spawnMonster = true;
        }

        /// <summary>
        /// This is method is to count down the time left. If the time limit is reached, the game will end.
        /// </summary>
        public void CountDown(){
            if (DateTime.Now - Program._secondTick >= Program._secondCountDown){
                _timeLeft -= 1; // Decrease every 1 second
                Program._secondTick = DateTime.Now;
            }

            // If time end, game end
            if (_timeLeft <= 0){
                _gameStatus = GameStatus.End;
            }
        }

        /// <summary>
        /// This is method is to run the game.
        /// </summary>
        public void Game(){

            // Center the camera on the player
            int x = _player.Location.X * Program.CELL_SIZE - 1200 / 2;
            int y = _player.Location.Y * Program.CELL_SIZE - 600 / 2;

            // Keep the camera within bounds of the mine
            x = Math.Max(-300, Math.Min(x, _mine.Rows * Program.CELL_SIZE - 1000));
            y = Math.Max(0, Math.Min(y, _mine.Cols * Program.CELL_SIZE - 600));

            SplashKit.SetCameraX(x);
            SplashKit.SetCameraY(y);

            // Draw the whole screen in black colour
            SplashKit.ClearScreen(Color.Black);

            // Draw mine
            _mine.DrawMine();

            // Draw player
            _player.DrawPlayer();
            
            if (_gameStatus != GameStatus.Pause){
                // Player's mechanism
                _player.Move();
                _player.Rest();
                _player.UseTool(_mine, _player.SelectCell());
            }
            
            // Spawn monster
            if (_spawnMonster){
                SpawnMonster();
            }

            // Draw area for game elements (at left), where right side is the game area
            SplashKit.FillRectangle(Color.White, SplashKit.ToWorldX(0), SplashKit.ToWorldY(0), 300, 600);
            SplashKit.DrawBitmap(Program.Board, SplashKit.ToWorldX(0), SplashKit.ToWorldY(0));
            
            // Draw a border to separate game and game elements
            SplashKit.FillRectangle(Color.DarkOliveGreen, SplashKit.ToWorldX(290), SplashKit.ToWorldY(0), 10, 600);

            // Draw game element
            DrawGameElement();
            
            // Draw player's inventory
            _player.ViewInventory();

            // Draw the map
            _player.ViewMap(_mine);

            // Draw the store
            _player.ViewStore(Program.store);

            // Player can select item when inventory or the store is opened
            _player.UseItem();

            // Start to count down the time
            if (_gameStatus != GameStatus.Pause)
            CountDown();

            // If game ended, determine player win or lose
            WinOrLose();
        }

        public void WinOrLose(){
            if (_gameStatus == GameStatus.End){
                if (_player.Inventory.DiamondCount() >= _targetDiamond){
                    _gameStatus = GameStatus.Win; // If diamond collected hits the target, player win
                    Program.Win_Sound.Play(); // Play sound effect
                } else {
                    _gameStatus = GameStatus.Lose; // Otherwise, player lose
                    Program.Lose_Sound.Play(); // Play sound effect
                }
            }
        }

        /// <summary>
        /// This is a method to generate maze, which is the structure of the mine
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void GenerateMaze(int rows, int cols){
            MazeGenerator mazeGenerator = new MazeGenerator(rows, cols); // Generate maze
            mazeGenerator.GenerateMaze(1, 1);
            _mine = new Mine(rows, cols, mazeGenerator.Grid); // Generate mine according to the maze
            int CoalAmount = _resourceAmount / 2;
            int IronAmount = (_resourceAmount - CoalAmount) / 3 * 2;
            int GoldAmount = _resourceAmount - CoalAmount - IronAmount;
            _mine.GenerateResource(_targetDiamond + 2, MaterialType.Diamond);
            _mine.GenerateResource(CoalAmount, MaterialType.Coal);
            _mine.GenerateResource(IronAmount, MaterialType.Iron);
            _mine.GenerateResource(GoldAmount, MaterialType.Gold);
        }

        /// <summary>
        /// This is a method to spawn monster
        /// </summary>
        public void SpawnMonster(){
            // Monster will appear in the game when the time pass half of the time limit
            if (_timeLeft == _timeLimit/2){
                if (!Program.Warning_Sound.IsPlaying)
                Program.Warning_Sound.Play(); // Play sound effect to warn player
            }

            if (_timeLeft <= _timeLimit/2){
                _monster.DrawMonster(); // Draw the monster

                // Monster's mechanism
                if (DateTime.Now - Program._monsterMoveTick >= Program._monsterMove && _gameStatus != GameStatus.Pause){
                    _monster.Move(_player);
                    // Monster will randomly attack or steal items from player. After it finish the action, it will disappear.
                    Random random = new Random();
                    int num = random.Next(0,2);
                    if (num == 0){
                        if(_monster.Attack(_player)){
                            _spawnMonster = false;
                            Program.Monster_Sound.Play(); // Play sound effect
                        }
                    } else {
                        if (_monster.Steal(_player)){
                            _spawnMonster = false;
                            Program.Monster_Sound.Play(); // Play sound effect
                        }
                    }
                    Program._monsterMoveTick = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// This is a method to draw the game element.
        /// </summary>
        public void DrawGameElement(){
            // Draw time count down
            SplashKit.DrawText("Time left: " + _timeLeft.ToString(), Color.Black, Program.font, 60, SplashKit.ToWorldX(10), SplashKit.ToWorldY(85));
            
            // Draw amount of target diamond
            SplashKit.DrawText("Target Diamond: ", Color.Black, Program.font, 56, SplashKit.ToWorldX(10), SplashKit.ToWorldY(115));
            SplashKit.DrawText(_player.Inventory.DiamondCount().ToString() + " /" + _targetDiamond.ToString(), Color.Black, Program.font, 60, SplashKit.ToWorldX(180), SplashKit.ToWorldY(115));
            
            // Draw player's money
            SplashKit.DrawText("Money: " + _player.Money.ToString(), Color.Black, Program.font, 60, SplashKit.ToWorldX(10), SplashKit.ToWorldY(145));
            
            // Draw Map Button
            Rectangle mapButton = new Rectangle();
            mapButton.X = SplashKit.ToWorldX(10);
            mapButton.Y = SplashKit.ToWorldY(560);
            mapButton.Width = 80;
            mapButton.Height = 30;
            SplashKit.FillRectangle(Color.Aquamarine, mapButton);
            SplashKit.DrawText("MAP", Color.Black, Program.font, 50, SplashKit.ToWorldX(30), SplashKit.ToWorldY(558));

            // Draw Store Button
            Rectangle storeButton = new Rectangle();
            storeButton.X = SplashKit.ToWorldX(100);
            storeButton.Y = SplashKit.ToWorldY(560);
            storeButton.Width = 80;
            storeButton.Height = 30;
            SplashKit.FillRectangle(Color.Beige, storeButton);
            SplashKit.DrawText("STORE", Color.Black, Program.font, 50, SplashKit.ToWorldX(110), SplashKit.ToWorldY(558));

            // Draw Inventory Button
            Rectangle inventoryButton = new Rectangle();
            inventoryButton.X = SplashKit.ToWorldX(190);
            inventoryButton.Y = SplashKit.ToWorldY(560);
            inventoryButton.Width = 80;
            inventoryButton.Height = 30;
            SplashKit.FillRectangle(Color.BurlyWood, inventoryButton);
            SplashKit.DrawText("INVENTORY", Color.Black, Program.font, 30, SplashKit.ToWorldX(193), SplashKit.ToWorldY(564));

            // If button is clicked, open the map/store/inventory accordingly
            
            if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), inventoryButton)){
                SplashKit.DrawRectangle(Color.Black, inventoryButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _player.InventoryOpen = !_player.InventoryOpen;
                    _player.MapOpen = false;
                    _player.StoreOpen = false;
                }
                
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), mapButton)){
                SplashKit.DrawRectangle(Color.Black, mapButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _player.MapOpen = !_player.MapOpen;
                    _player.InventoryOpen = false;
                    _player.StoreOpen = false;
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), storeButton)){
                SplashKit.DrawRectangle(Color.Black, storeButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _player.StoreOpen = !_player.StoreOpen;
                    _player.InventoryOpen = false;
                    _player.MapOpen = false;
                }
            }
            // Draw Level No
            SplashKit.DrawText("LEVEL " + _levelNo.ToString(), Color.Black, Program.font, 75, SplashKit.ToWorldX(130), SplashKit.ToWorldY(0));


            // Draw Stamina Bar (green, shrinking with stamina)
            SplashKit.FillRectangle(Color.Gray, SplashKit.ToWorldX(80), SplashKit.ToWorldY(40), 200, 20);
            if (_player.Stamina > 0){
                SplashKit.FillRectangle(Color.Green, SplashKit.ToWorldX(80), SplashKit.ToWorldY(40), _player.Stamina * 2, 20);
            }
            SplashKit.DrawBitmap(Program.Player_right, SplashKit.ToWorldX(5), SplashKit.ToWorldY(5));
            
            // Draw tool selected
            if (_player.ToolSelected != null){
                SplashKit.DrawBitmap(_player.ToolSelected.Image, SplashKit.ToWorldX(240), SplashKit.ToWorldY(110));
            }
            SplashKit.DrawRectangle(Color.Black, SplashKit.ToWorldX(240), SplashKit.ToWorldY(110), 40, 40);

            // Draw player's score
            SplashKit.DrawText("Score: " + _player.Score.ToString(), Color.White, Program.font, 75, SplashKit.ToWorldX(750), SplashKit.ToWorldY(0));
        }

        public void MainMenu(){
            SplashKit.ClearScreen(Color.Black);
            SplashKit.DrawBitmap(Program.MainMenu, SplashKit.ToWorldX(0), SplashKit.ToWorldY(0));
            SplashKit.DrawText("MINING-MAZE GAME", Color.Black, Program.font, 120, SplashKit.ToWorldX(285), SplashKit.ToWorldY(150));

            // Draw Level Button
            Rectangle Level1 = new Rectangle(){X = SplashKit.ToWorldX(415), Y = SplashKit.ToWorldY(250), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, Level1);
            SplashKit.DrawText("LEVEL 1", Color.White, Program.font, 60, SplashKit.ToWorldX(470), SplashKit.ToWorldY(260));

            Rectangle Level2 = new Rectangle(){X = SplashKit.ToWorldX(415), Y = SplashKit.ToWorldY(330), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, Level2);
            SplashKit.DrawText("LEVEL 2", Color.White, Program.font, 60, SplashKit.ToWorldX(470), SplashKit.ToWorldY(340));

            Rectangle Level3 = new Rectangle(){X = SplashKit.ToWorldX(415), Y = SplashKit.ToWorldY(410), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, Level3);
            SplashKit.DrawText("LEVEL 3", Color.White, Program.font, 60, SplashKit.ToWorldX(470), SplashKit.ToWorldY(420));

            Rectangle InstructionButton = new Rectangle(){X = SplashKit.ToWorldX(395), Y = SplashKit.ToWorldY(520), Width = 240, Height = 60};
            SplashKit.FillRectangle(Color.Gold, InstructionButton);
            SplashKit.DrawText("INSTRUCTION", Color.White, Program.font, 60, SplashKit.ToWorldX(445), SplashKit.ToWorldY(530));

            if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), Level1)){
                SplashKit.DrawRectangle(Color.Black, Level1);
                if (SplashKit.MouseClicked(MouseButton.LeftButton) && !Program.showInstruction){
                    _levelNo = 1;
                    _targetDiamond = 5;
                    _resourceAmount = 15;
                    _timeLimit = 120;
                    Initialize(15, 15); // 15x15 mine
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), Level2)){
                SplashKit.DrawRectangle(Color.Black, Level2);
                if (SplashKit.MouseClicked(MouseButton.LeftButton) && !Program.showInstruction){
                    _levelNo = 2;
                    _targetDiamond = 10;
                    _resourceAmount = 19;
                    _timeLimit = 110;
                    Initialize(21, 21); // 21x21 mine
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), Level3)){
                SplashKit.DrawRectangle(Color.Black, Level3);
                if (SplashKit.MouseClicked(MouseButton.LeftButton) && !Program.showInstruction){
                    _levelNo = 3;
                    _targetDiamond = 15;
                    _resourceAmount = 39;
                    _timeLimit = 110;
                    Initialize(39, 39); // 39x39 mine
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), InstructionButton)){
                SplashKit.DrawRectangle(Color.Black, InstructionButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    Program.showInstruction = !Program.showInstruction;
                }
            }
        }

        public void GamePause(){
            SplashKit.DrawText("PAUSED", Color.Black, Program.font, 120, SplashKit.ToWorldX(525), SplashKit.ToWorldY(150));
            // Draw Buttons
            Rectangle ResumeButton = new Rectangle(){X = SplashKit.ToWorldX(520), Y = SplashKit.ToWorldY(250), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, ResumeButton);
            SplashKit.DrawText("RESUME", Color.White, Program.font, 60, SplashKit.ToWorldX(575), SplashKit.ToWorldY(260));

            Rectangle EndButton = new Rectangle(){X = SplashKit.ToWorldX(520), Y = SplashKit.ToWorldY(330), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, EndButton);
            SplashKit.DrawText("END", Color.White, Program.font, 60, SplashKit.ToWorldX(595), SplashKit.ToWorldY(340));

            Rectangle ExitButton = new Rectangle(){X = SplashKit.ToWorldX(520), Y = SplashKit.ToWorldY(410), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, ExitButton);
            SplashKit.DrawText("EXIT", Color.White, Program.font, 60, SplashKit.ToWorldX(593), SplashKit.ToWorldY(420));

            if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), ResumeButton)){
                SplashKit.DrawRectangle(Color.Black, ResumeButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _gameStatus = GameStatus.Start;
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), EndButton)){
                SplashKit.DrawRectangle(Color.Black, EndButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _gameStatus = GameStatus.End;
                    WinOrLose();
                }
            } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), ExitButton)){
                SplashKit.DrawRectangle(Color.Black, ExitButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    SplashKit.CloseWindow("Mining-Maze Game");
                }
            }
        }

        public void GameEnd(){
            SplashKit.ClearScreen(Color.Black);
            SplashKit.DrawBitmap(Program.MainMenu, SplashKit.ToWorldX(0), SplashKit.ToWorldY(0));

            // Draw message
            string text = _gameStatus == GameStatus.Win ? "YOU WON" : "YOU LOSE";
            SplashKit.DrawText(text, Color.Black, Program.font, 120, SplashKit.ToWorldX(400), SplashKit.ToWorldY(150));

            // Read highest score
            string score;
            string filepath;

            if (_levelNo == 1){
                filepath = "media/Record/Level1.txt";
            } else if (_levelNo == 2){
                filepath = "media/Record/Level2.txt";
            } else {
                filepath = "media/Record/Level3.txt";
            }

            score = File.ReadAllText(filepath);

            // Update highest score
            if (_player.Score > int.Parse(score)){
                score = _player.Score.ToString();
                File.WriteAllText(filepath, score);
            }

            // Display highest score
            SplashKit.DrawText("Highest Score:" + score.ToString(), Color.Black, Program.font, 100, SplashKit.ToWorldX(370), SplashKit.ToWorldY(230));
            SplashKit.DrawText("Current Score:" + _player.Score.ToString(), Color.Black, Program.font, 80, SplashKit.ToWorldX(380), SplashKit.ToWorldY(280));

            // Draw restart button
            Rectangle RestartButton = new Rectangle(){X = SplashKit.ToWorldX(415), Y = SplashKit.ToWorldY(390), Width = 200, Height = 60};
            SplashKit.FillRectangle(Color.CornflowerBlue, RestartButton);

            SplashKit.DrawText("BACK TO MENU", Color.White, Program.font, 60, SplashKit.ToWorldX(430), SplashKit.ToWorldY(400));

            if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), RestartButton)){
                SplashKit.DrawRectangle(Color.Black, RestartButton);
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _gameStatus = GameStatus.Ready;
                }
            }
        }

        public void Instruction(){
            if (Program.showInstruction){
                // Display the instruction
                Rectangle InsturctionRect = new Rectangle();
                InsturctionRect.X = SplashKit.ToWorldX(20);
                InsturctionRect.Y = SplashKit.ToWorldY(20);
                InsturctionRect.Width = 960;
                InsturctionRect.Height = 480;

                SplashKit.FillRectangle(Color.Black, InsturctionRect);

                // How to Play:
                SplashKit.DrawText("HOW TO PLAY:", Color.White, Program.font, 55, SplashKit.ToWorldX(25), SplashKit.ToWorldY(25)); 
                SplashKit.DrawText("1. USE WASD OR ARROW KEY TO MOVE AND SELECT CELL TO MINE", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(50)); 
                SplashKit.DrawBitmap(Program.Player_right, SplashKit.ToWorldX(250),SplashKit.ToWorldY(130));
                SplashKit.DrawBitmap(Program.IronPickaxeImage, SplashKit.ToWorldX(290),SplashKit.ToWorldY(170));
                SplashKit.DrawBitmap(Program.dirtImage, SplashKit.ToWorldX(330),SplashKit.ToWorldY(130));
                SplashKit.DrawText("2", Color.Black, Program.font, 32,  SplashKit.ToWorldX(362), SplashKit.ToWorldY(160));
                SplashKit.DrawRectangle(Color.Red, SplashKit.ToWorldX(330),SplashKit.ToWorldY(130), 80, 80);
                SplashKit.DrawBitmap(Program.arrowKey, SplashKit.ToWorldX(150),SplashKit.ToWorldY(130));
                SplashKit.DrawText("2. PRESS SPACE BAR TO MINE", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(70));
                SplashKit.DrawText("   (KEEP HOLDING THE KEY TO SELECT CELL)", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(90));
                SplashKit.DrawText("HOW TO WIN:", Color.White, Program.font, 55, SplashKit.ToWorldX(25), SplashKit.ToWorldY(215)); 
                SplashKit.DrawText("1. COLLECT TARGETED AMOUNT OF DIAMOND WITHIN TIME LIMIT", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(240)); 
                SplashKit.DrawText("THINGS TO KNOW:", Color.White, Program.font, 55, SplashKit.ToWorldX(25), SplashKit.ToWorldY(300)); 
                SplashKit.DrawText("1. YOU CAN SELL RESOURCE COLLECTED", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(325)); 
                SplashKit.DrawText("2. YOU CAN BUY FOOD TO REGAIN STAMINA AT STORE", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(345));
                SplashKit.DrawText("3. YOU CAN BUY ADVANCED TOOL TO MINE AT STORE", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(365));  
                SplashKit.DrawText("4. YOU CAN UPGRADE TOOL", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(385)); 
                SplashKit.DrawText("5. MONSTER WILL APPEAR AFTER TIME LIMIT PASS HALF", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(405)); 
                SplashKit.DrawText("6. MONSTER WILL ATTACK YOU OR STEAL YOUR ITEMS", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(425));
                SplashKit.DrawText("7. YOU CAN VIEW A MAP", Color.White, Program.font, 50, SplashKit.ToWorldX(25), SplashKit.ToWorldY(445));
                
                SplashKit.FillRectangle(Color.White, SplashKit.ToWorldX(680), SplashKit.ToWorldY(20), 300, 480);
                SplashKit.DrawText("GAME ELEMENTS:", Color.Black, Program.font, 50, SplashKit.ToWorldX(680), SplashKit.ToWorldY(20));

                SplashKit.DrawBitmap(Program.IronPickaxeImage, SplashKit.ToWorldX(725), SplashKit.ToWorldY(70));
                SplashKit.DrawBitmap(Program.GoldPickaxeImage, SplashKit.ToWorldX(775), SplashKit.ToWorldY(70));
                SplashKit.DrawBitmap(Program.DiamondPickaxeImage, SplashKit.ToWorldX(825), SplashKit.ToWorldY(70));
                SplashKit.DrawBitmap(Program.BombImage, SplashKit.ToWorldX(875), SplashKit.ToWorldY(70));
                SplashKit.DrawText("TOOLS", Color.Black, Program.font, 50, SplashKit.ToWorldX(785), SplashKit.ToWorldY(110));

                SplashKit.DrawBitmap(Program.CoalImage, SplashKit.ToWorldX(725), SplashKit.ToWorldY(150));
                SplashKit.DrawBitmap(Program.IronImage, SplashKit.ToWorldX(775), SplashKit.ToWorldY(150));
                SplashKit.DrawBitmap(Program.GoldImage, SplashKit.ToWorldX(825), SplashKit.ToWorldY(150));
                SplashKit.DrawBitmap(Program.DiamondImage, SplashKit.ToWorldX(875), SplashKit.ToWorldY(150));
                SplashKit.DrawText("RESOURCES", Color.Black, Program.font, 50, SplashKit.ToWorldX(770), SplashKit.ToWorldY(190));

                SplashKit.DrawBitmap(Program.Player_right, SplashKit.ToWorldX(775),SplashKit.ToWorldY(230));
                SplashKit.DrawText("PLAYER", Color.Black, Program.font, 50, SplashKit.ToWorldX(778), SplashKit.ToWorldY(305));

                SplashKit.DrawBitmap(Program.MonsterImage, SplashKit.ToWorldX(775), SplashKit.ToWorldY(370));
                SplashKit.DrawText("MONSTER", Color.Black, Program.font, 50, SplashKit.ToWorldX(775), SplashKit.ToWorldY(445));
            }
        }
    }
}