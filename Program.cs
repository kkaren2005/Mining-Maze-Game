using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

namespace testCustom
{
    public static class Program
    {
        public static readonly int CELL_SIZE = 80;

        public static readonly Font font = SplashKit.LoadFont("Font", "media/Font/game_over.ttf");

        public static DateTime _lastMoveTime = DateTime.Now;
        public static DateTime _lastMineTime = DateTime.Now;

        public static DateTime _lastRestTime = DateTime.Now;

        public static DateTime _secondTick = DateTime.Now;

        public static DateTime _monsterMoveTick = DateTime.Now;

        public static readonly TimeSpan _MoveCoolDown = TimeSpan.FromMilliseconds(200);

        public static readonly TimeSpan _MineCoolDown = TimeSpan.FromMilliseconds(200);

        public static readonly TimeSpan _RestCoolDown = TimeSpan.FromMilliseconds(2500);

        public static readonly TimeSpan _secondCountDown = TimeSpan.FromMilliseconds(1000);

        public static readonly TimeSpan _monsterMove = TimeSpan.FromMilliseconds(1000);

        public static Bitmap DiamondImage = new Bitmap("Diamond", "media/Image/diamond.png");

        public static Bitmap GoldImage = new Bitmap("Gold", "media/Image/gold.png");

        public static Bitmap IronImage = new Bitmap("Iron", "media/Image/Iron.jpg");

        public static Bitmap CoalImage = new Bitmap("Coal", "media/Image/Coal.jpg");

        public static Bitmap DiamondBlock = new Bitmap("Diamond_block", "media/Image/Diamond_block.png");

        public static Bitmap GoldBlock = new Bitmap("Gold_block", "media/Image/Gold_block.jpg");

        public static Bitmap IronBlock = new Bitmap("Iron_block", "media/Image/Iron_block.jpg");

        public static Bitmap CoalBlock = new Bitmap("Coal_block", "media/Image/Coal_block.jpg");

        public static Bitmap IronPickaxeImage = new Bitmap("IronPickaxe_image", "media/Image/Pickaxe.png");

        public static Bitmap GoldPickaxeImage = new Bitmap("GoldPickaxe_image", "media/Image/Gold_pickaxe.png");

        public static Bitmap DiamondPickaxeImage = new Bitmap("DiamondPickaxe_image", "media/Image/Diamond_pickaxe.png");

        public static Bitmap BombImage = new Bitmap("Bomb_image", "media/Image/Bomb.PNG");

        public static Bitmap MonsterImage = new Bitmap("Monster_image", "media/Image/monster.PNG");

        public static Bitmap Player_left = new Bitmap("Player_Left", "media/Image/Player_Left.png");

        public static Bitmap Player_right = new Bitmap("Player_Right", "media/Image/Player_Right.png");

        public static Bitmap BurgerImage = new Bitmap("Burger", "media/Image/Burger.JPG");

        public static Bitmap ColaImage = new Bitmap("Cola", "media/Image/Cola.JPG");

        public static Bitmap PizzaImage = new Bitmap("Pizza", "media/Image/Pizza.JPG");

        public static ItemFactory itemFactory = new ItemFactory();

        public static Store store = new Store();

        public static SoundEffect Mine_Sound = SplashKit.LoadSoundEffect("Mine Sound", "media/Sound/mine_sound.mp3");

        public static SoundEffect Collect_Resource_Sound = SplashKit.LoadSoundEffect("Collect Resource", "media/Sound/Collect_Resource.mp3");

        public static SoundEffect Sell_Resource_Sound = SplashKit.LoadSoundEffect("Sell Resource", "media/Sound/Sell_Resource.mp3");

        public static SoundEffect BombSound = SplashKit.LoadSoundEffect("Bomb Effect", "media/Sound/BombSound.mp3");

        public static SoundEffect BuyItemSound = SplashKit.LoadSoundEffect("Buy Item Sound", "media/Sound/BuyItemSound.mp3");

        public static SoundEffect EatFood = SplashKit.LoadSoundEffect("Eat food", "media/Sound/Eat_Sound.mp3");

        public static SoundEffect Upgrade_Sound = SplashKit.LoadSoundEffect("Upgrade", "media/Sound/Upgrade_Sound.mp3");

        public static SoundEffect Win_Sound = SplashKit.LoadSoundEffect("Win", "media/Sound/Win_Sound.mp3");

        public static SoundEffect Lose_Sound = SplashKit.LoadSoundEffect("Lose", "media/Sound/Lose_Sound.mp3");

        public static SoundEffect Monster_Sound = SplashKit.LoadSoundEffect("Monster sound", "media/Sound/Monster_sound.mp3");

        public static SoundEffect Warning_Sound = SplashKit.LoadSoundEffect("Warning sound", "media/Sound/Warning_sound.mp3");

        public static SoundEffect GameBGM = SplashKit.LoadSoundEffect("BGM", "media/Sound/Game_BGM.mp3");

        public static SoundEffect Item_Equip = SplashKit.LoadSoundEffect("Item equip", "media/Sound/Item_Equip.mp3");

        public static Bitmap MenuButton = SplashKit.LoadBitmap("Menu Button", "media/Image/MenuButton.jpg");

        public static Bitmap MusicButtonOn = SplashKit.LoadBitmap("Music Button On", "media/Image/MusicButtonOn.PNG");

        public static Bitmap MusicButtonOff = SplashKit.LoadBitmap("Music Button Off", "media/Image/MusicButtonOff.PNG");

        public static Bitmap MainMenu = SplashKit.LoadBitmap("Main Menu", "media/Image/MainMenu.JPG");

        public static Bitmap Map = SplashKit.LoadBitmap("Map", "media/Image/Map.JPG");

        public static Bitmap Board = SplashKit.LoadBitmap("Board", "media/Image/board.png");

        public static Bitmap Flag = SplashKit.LoadBitmap("Flag", "media/Image/flag.png");

        public static Bitmap Bag = SplashKit.LoadBitmap("Bag", "media/Image/bag.png");

        public static bool showInstruction = false;

        public static Bitmap dirtImage = new Bitmap("Dirt", "media/Image/dirt.jpg");
        public static Bitmap stoneImage = new Bitmap("Stone", "media/Image/stone.jpg");

        public static Bitmap arrowKey = new Bitmap("Arrow Key", "media/Image/arrowKey.png");

        public static void Main()
        {
            Window window = new Window("Mining-Maze Game", 1000, 600); // Create window
           
            GameLevel gameLevel = new GameLevel(1, 10, 10, 20); // GameStatus: Ready

            bool playMusic = true;

            while (!window.CloseRequested)
            {
                SplashKit.ProcessEvents();
                SplashKit.RefreshScreen();

                // Draw screen when the gameStatus is ready, which is the menu
                if (gameLevel.GameStatus == GameStatus.Ready){
                    gameLevel.MainMenu();
                    gameLevel.Instruction();

                // If the gameStatus is start, run the game
                } else if (gameLevel.GameStatus == GameStatus.Start || gameLevel.GameStatus == GameStatus.Pause){
                    if (!GameBGM.IsPlaying && playMusic){
                        GameBGM.Play();
                    }
                    gameLevel.Game();

                    Bitmap MusicButton = playMusic == true ? MusicButtonOn : MusicButtonOff;

                    SplashKit.DrawBitmap(MenuButton, SplashKit.ToWorldX(962), SplashKit.ToWorldY(0));
                    SplashKit.DrawBitmap(MusicButton, SplashKit.ToWorldX(922), SplashKit.ToWorldY(0));

                    if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                        if (SplashKit.PointInRectangle(SplashKit.ToWorldX(SplashKit.MouseX()), SplashKit.ToWorldY(SplashKit.MouseY()), SplashKit.ToWorldX(962), SplashKit.ToWorldY(0), 38, 38)){
                            gameLevel.GameStatus = GameStatus.Pause;
                        } else if (SplashKit.PointInRectangle(SplashKit.ToWorldX(SplashKit.MouseX()), SplashKit.ToWorldY(SplashKit.MouseY()), SplashKit.ToWorldX(922), SplashKit.ToWorldY(0), 38, 38)){
                            playMusic = !playMusic;
                            if (!playMusic){
                                GameBGM.Stop();
                            }
                        }
                    }

                    if (gameLevel.GameStatus == GameStatus.Pause){
                       gameLevel.GamePause();
                    }

                // If player win or lose the game, draw the screen accordingly
                } else if (gameLevel.GameStatus == GameStatus.Win || gameLevel.GameStatus == GameStatus.Lose){
                    gameLevel.GameEnd();
                }
            }
        }
    }
}