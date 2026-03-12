#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Player class, which consists of 11 attributes
    /// </summary>
    public class Player{
        private int _stamina;

        private Inventory _inventory;

        private int _money;

        private Tool _toolSelected;

        private Cell _location;

        private Direction _direction;

        private bool _inventoryOpen;

        private bool _mapOpen;

        private bool _storeOpen;

        private Item _selectedItem;

        private int _score;

        /// <summary>
        /// This is default constructor. The default value of stamina is 100, money is 100.
        /// </summary>
        public Player(){
            _stamina = 100;
            _inventory = new Inventory();
            _money = 100;
            _toolSelected = null;
            _location = null;
            _direction = Direction.Right;
            _inventoryOpen = false;
            _mapOpen = false;
            _storeOpen = false;
            _selectedItem = null;
            _score = 0;
        }

        /// <summary>
        /// This is property for Stamina field
        /// </summary>
        /// <value></value>
        public int Stamina{
            get { return _stamina; }
            set { _stamina = value; }
        }

        
        /// <summary>
        /// This is a readonly property for Inventory field
        /// </summary>
        /// <value></value>
        public Inventory Inventory{
            get { return _inventory; }
        }

        /// <summary>
        /// This is property for Money field
        /// </summary>
        /// <value></value>
        public int Money{
            get { return _money; }
            set { _money = value; }
        }

        /// <summary>
        /// This is a property for ToolSelected field
        /// </summary>
        /// <value></value>
        public Tool ToolSelected{
            get { return _toolSelected; }
            set { _toolSelected = value; }
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
        /// This is property for InventoryOpen field
        /// </summary>
        /// <value></value>
        public bool InventoryOpen{
            get { return _inventoryOpen; }
            set { _inventoryOpen = value; }
        }

        /// <summary>
        /// This is property for MapOpen field
        /// </summary>
        /// <value></value>
        public bool MapOpen{
            get { return _mapOpen; }
            set { _mapOpen = value; }
        }

        /// <summary>
        /// This is property for StoreOpen field
        /// </summary>
        /// <value></value>
        public bool StoreOpen{
            get { return _storeOpen; }
            set { _storeOpen = value; }
        }

        /// <summary>
        /// This is property for SelectedItem field
        /// </summary>
        /// <value></value>
        public Item SelectedItem{
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        public int Score{
            get { return _score; }
            set { _score = value; }
        }

        /// <summary>
        /// This method enables player to collect item and store it into inventory
        /// </summary>
        /// <param name="item"></param>
        public void CollectItem(Item item){
            _inventory.AddItem(item);
            if (item.Name == "Diamond" || item.Name == "Gold" || item.Name == "Iron" || item.Name == "Coal")
            _score += 10;
        }

        /// <summary>
        /// This is a method that enables player to remove item from his inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item){
            _inventory.RemoveItem(item);
        }

        /// <summary>
        /// This method enables player to buy item from the store
        /// </summary>
        /// <param name="item"></param>
        public void BuyItem(Item item){
            if (_money >= item.Value){
                CollectItem(item);
                _money -= item.Value;
                Program.BuyItemSound.Play();
            }
        }

        /// <summary>
        /// This method enables player to sell item collected, especially the resources collected
        /// </summary>
        /// <param name="item"></param>
        public void SellItem(Item item){
            if (_inventory.hasItem(item)){
                _money += item.Value;
                RemoveItem(item);
                Program.Sell_Resource_Sound.Play();
            }
        }

        /// <summary>
        /// This method enables player to select tool used to mine the cell
        /// </summary>
        /// <param name="tool"></param>
        public void SelectTool(Tool tool){
            _toolSelected = tool;
            Program.Item_Equip.Play();
        }

        /// <summary>
        /// This method enables player to select cell to mine by pressing the arrow key or WASD key
        /// </summary>
        /// <returns></returns>
        public Cell SelectCell(){
            Cell cell = null;

            if ((SplashKit.KeyDown(KeyCode.UpKey) || SplashKit.KeyDown(KeyCode.WKey)) && _location.Up != null)
            {
                if (!_location.Up.Mined)
                cell = _location.Up;
            }
            else if ((SplashKit.KeyDown(KeyCode.DownKey) || SplashKit.KeyDown(KeyCode.SKey)) && _location.Down != null)
            {
                if (!_location.Down.Mined)
                cell = _location.Down;
            }
            else if ((SplashKit.KeyDown(KeyCode.LeftKey) || SplashKit.KeyDown(KeyCode.AKey)) && _location.Left != null)
            {
                if (!_location.Left.Mined)
                cell = _location.Left;
            }
            else if ((SplashKit.KeyDown(KeyCode.RightKey) || SplashKit.KeyDown(KeyCode.DKey)) && _location.Right != null)
            {
                if (!_location.Right.Mined)
                cell = _location.Right;
            }
            // A red border will be drawn if a cell is selected
            if (cell != null){
                SplashKit.DrawRectangle(Color.Red, cell.X * Program.CELL_SIZE, cell.Y * Program.CELL_SIZE, Program.CELL_SIZE, Program.CELL_SIZE);
            }
           
            return cell;
        }

        /// <summary>
        /// This method enables player to select items from either inventory or the store
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public Item SelectItem(List<Item> items){
            // Get item index
            double x = SplashKit.ToWorldX(SplashKit.MouseX());
            double y = SplashKit.ToWorldY(SplashKit.MouseY());

            int shiftx = 0;
            int shifty = 0;
            int columnFilled = 0;

            for (int i = 0; i < items.Count; i++)
            {
                // Calculate item's position
                double itemX = 22 + shiftx;
                double itemY = 250 + shifty;

                // Define item's bounding rectangle
                Rectangle itemRect = new Rectangle()
                {
                    X = SplashKit.ToWorldX(itemX),
                    Y = SplashKit.ToWorldY(itemY),
                    Width = 40,
                    Height = 40
                };

                if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), itemRect)){
                    return items[i];
                }

                shiftx += 50;
                columnFilled += 1;
                if (columnFilled == 5){
                    shiftx = 0;
                    shifty += 50;
                    columnFilled = 0;
                }
            }
            return null;
        }

        /// <summary>
        /// This method enables player to eat food to regain his stamina
        /// </summary>
        /// <param name="food"></param>
        public void EatFood(Food food){
            if (_inventory.hasItem(food)){
                _stamina += food.Value;
                _stamina = Math.Min(100, _stamina); // Limit stamina to max 100
                _inventory.RemoveItem(food); // Food is one-time used only
                Program.EatFood.Play(); // Play sound effect
            }
        }

        /// <summary>
        /// This method enables player to use tool by pressing the space bar after a cell is selected
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="selectedCell"></param>
        public void UseTool(Mine mine, Cell selectedCell){
            if (DateTime.Now - Program._lastMineTime >= Program._MineCoolDown && _stamina > 0 && selectedCell != null){ // Limit the mine speed
                if (SplashKit.KeyDown(KeyCode.SpaceKey) && _toolSelected != null){
                    if (!Program.Mine_Sound.IsPlaying)
                    Program.Mine_Sound.Play();
                    _toolSelected.UseEffect(mine, this, selectedCell);
                    Program._lastMineTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// This method enables player to use, remove, upgrade, or sell the item selected depending on the type of item selected.
        /// </summary>
        public void UseItem(){
            if (_inventoryOpen){

                // Draw Buttons if item in inventory is selected
                if (_inventory.hasItem(_selectedItem)){
                    // REMOVE or UPGRADE button (at right)
                    Rectangle RightButton = new Rectangle();
                    RightButton.X = SplashKit.ToWorldX(150);
                    RightButton.Y = SplashKit.ToWorldY(510);
                    RightButton.Width = 120;
                    RightButton.Height = 30;
                    SplashKit.FillRectangle(Color.Coral, RightButton);
                    if (_selectedItem.GetType()==typeof(Resource) || _selectedItem.GetType()==typeof(Food)){
                        SplashKit.DrawText("REMOVE", Color.Black, Program.font, 40, SplashKit.ToWorldX(175), SplashKit.ToWorldY(510));
                    } else {
                        SplashKit.DrawText("UPGRADE", Color.Black, Program.font, 40, SplashKit.ToWorldX(165), SplashKit.ToWorldY(510));
                        if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), RightButton)){
                            SplashKit.FillRectangle(Color.Gray, SplashKit.ToWorldX(280), SplashKit.ToWorldY(510), 80, 30);
                            SplashKit.DrawText("$ 50", Color.White, Program.font, 40, SplashKit.ToWorldX(300), SplashKit.ToWorldY(513)); // Need $50 to upgrade the tools
                        }
                    }

                    // Use button, sell button (at left)
                    Rectangle LeftButton = new Rectangle();
                    LeftButton.X = SplashKit.ToWorldX(20);
                    LeftButton.Y = SplashKit.ToWorldY(510);
                    LeftButton.Width = 120;
                    LeftButton.Height = 30;
                    SplashKit.FillRectangle(Color.Coral, LeftButton);
                    if (_selectedItem.GetType()==typeof(Resource)){
                        SplashKit.DrawText("SELL", Color.Black, Program.font, 40, SplashKit.ToWorldX(60), SplashKit.ToWorldY(510));
                    } else {
                        SplashKit.DrawText("USE", Color.Black, Program.font, 40, SplashKit.ToWorldX(60), SplashKit.ToWorldY(510));
                    }

                    //If mouse is clicked, check whether button is clicked
                    
                        if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), RightButton)){
                            SplashKit.DrawRectangle(Color.Black, RightButton);
                            if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                                if (_selectedItem.Name == "Iron Pickaxe" || _selectedItem.Name == "Bomb" || _selectedItem.Name == "Diamond Pickaxe" || _selectedItem.Name == "Gold Pickaxe"){
                                    SelectTool((Tool)_selectedItem);
                                    _toolSelected.Upgrade(this);
                                } else {
                                    RemoveItem(_selectedItem);
                                }
                                _selectedItem = null;
                            }
                        } else if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), LeftButton)){
                            SplashKit.DrawRectangle(Color.Black, LeftButton);
                            if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                                if (_selectedItem.Name == "Iron Pickaxe" || _selectedItem.Name == "Bomb" || _selectedItem.Name == "Diamond Pickaxe" || _selectedItem.Name == "Gold Pickaxe"){
                                    SelectTool((Tool)_selectedItem);
                                } else if (_selectedItem.Name == "Burger" || _selectedItem.Name == "Pizza" || _selectedItem.Name == "Cola"){
                                    EatFood((Food)_selectedItem);
                                }
                                else {
                                    SellItem(_selectedItem);
                                }
                                _selectedItem = null;
                            }
                        }
                }

                // Select item
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _selectedItem = SelectItem(_inventory.Items);
                }
                
            } else if (_storeOpen){
                if (Program.store.Items.Contains(_selectedItem)){
                    // BUY Button
                    Rectangle BuyButton = new Rectangle();
                    BuyButton.X = SplashKit.ToWorldX(40);
                    BuyButton.Y = SplashKit.ToWorldY(510);
                    BuyButton.Width = 210;
                    BuyButton.Height = 30;
                    SplashKit.FillRectangle(Color.Coral, BuyButton);
                    SplashKit.DrawText("BUY", Color.Black, Program.font, 40, SplashKit.ToWorldX(120), SplashKit.ToWorldY(510));

                    // If button is clicked
                    if (SplashKit.PointInRectangle(SplashKit.ToWorld(SplashKit.MousePosition()), BuyButton)){
                        SplashKit.DrawRectangle(Color.Black, BuyButton);
                        if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                            if (_selectedItem.Name == "Bomb"){
                                BuyItem(Program.itemFactory.GetBomb());
                            } else if (_selectedItem.Name == "Diamond Pickaxe"){
                                BuyItem(Program.itemFactory.GetPickaxe(MaterialType.Diamond));
                            } else if (_selectedItem.Name == "Gold Pickaxe"){
                                BuyItem(Program.itemFactory.GetPickaxe(MaterialType.Gold));
                            } else {
                                BuyItem(Program.itemFactory.GetFood(_selectedItem.Name));
                                _selectedItem = null;
                            }
                        }
                    }
                }
                // Select item
                if (SplashKit.MouseClicked(MouseButton.LeftButton)){
                    _selectedItem = SelectItem(Program.store.Items);
                }
            }
        }

        /// <summary>
        /// This method enables player to rest and regain stamina automatically
        /// </summary>
        public void Rest(){
            if (_stamina < 100){
                if (DateTime.Now - Program._lastRestTime >= Program._RestCoolDown){
                    _stamina += 2;
                    Program._lastRestTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// This method enables player to move by pressing arrow key or WASD key
        /// </summary>
        public void Move(){
            if (DateTime.Now - Program._lastMoveTime >= Program._MoveCoolDown) // Limit player's move speed
            {
                if ((SplashKit.KeyDown(KeyCode.UpKey) || SplashKit.KeyDown(KeyCode.WKey)) && _location.Up != null)
                {
                    if (_location.Up.Mined)
                    _location = _location.Up;
                    Program._lastMoveTime = DateTime.Now;
                }
                else if ((SplashKit.KeyDown(KeyCode.DownKey) || SplashKit.KeyDown(KeyCode.SKey)) && _location.Down != null)
                {
                    if (_location.Down.Mined)
                    _location = _location.Down;
                    Program._lastMoveTime = DateTime.Now;
                }
                else if (SplashKit.KeyDown(KeyCode.LeftKey) || SplashKit.KeyDown(KeyCode.AKey))
                {
                    Program._lastMoveTime = DateTime.Now;
                    _direction = Direction.Left;
                    if (_location.Left != null){
                        if (_location.Left.Mined)
                        _location = _location.Left;
                    }
                }
                else if (SplashKit.KeyDown(KeyCode.RightKey) || SplashKit.KeyDown(KeyCode.DKey))
                {
                    Program._lastMoveTime = DateTime.Now;
                    _direction = Direction.Right;
                    if (_location.Right != null){
                        if (_location.Right.Mined)
                        _location = _location.Right;
                    }
                }
            }
        }

        /// <summary>
        /// This is a method that enables player to view the map
        /// </summary>
        /// <param name="mine"></param>
        public void ViewMap(Mine mine){
            mine.Map(this);
        }

        /// <summary>
        /// This is a method that enables player to view the inventory
        /// </summary>
        public void ViewInventory(){
            if (_inventoryOpen){
                // Draw the inventory
                _inventory.DrawInventory();

                // Draw the selected item (with red border)
                if (_inventory.hasItem(_selectedItem)){
                    DrawSelectedItem(_inventory.Items);
                }
            }
        }

        /// <summary>
        /// This is a method that enables player to view the store
        /// </summary>
        /// <param name="store"></param>
        public void ViewStore(Store store){
            if (_storeOpen){
                // Draw the store
                store.DrawStore();

                // Draw the selected item
                if (Program.store.Items.Contains(_selectedItem)){
                    DrawSelectedItem(Program.store.Items);
                }
            }
        }

        /// <summary>
        /// This method is do draw the selected item with red border. The details of the item will be displayed too, e.g. name and description.
        /// </summary>
        /// <param name="items"></param>
        public void DrawSelectedItem(List<Item> items){
            // Draw the selected item
            if (_selectedItem != null){
                if (items.Contains(_selectedItem)){
                    int index = items.IndexOf(_selectedItem);

                    int shiftx = 0;
                    int shifty = 0;
                    
                    for (int j = 0; j < 5; j++){
                        if (index / 5 == j){
                            shifty = 50 * j;
                        }
                    }

                    if ((index + 1) %5 == 1){
                        shiftx = 0;
                    } else if ((index + 1) %5 == 2){
                        shiftx = 50;
                    } else if ((index + 1) %5 == 3){
                        shiftx = 100;
                    } else if ((index + 1) %5 == 4){
                        shiftx = 150;
                    } else {
                        shiftx = 200;
                    }

                    SplashKit.DrawRectangle(Color.Red, SplashKit.ToWorldX(22 + shiftx), SplashKit.ToWorldY(250 + shifty), 40, 40);
                }
                // Display the details
                SplashKit.DrawText("Name: " + _selectedItem.Name, Color.Black, Program.font, 30, SplashKit.ToWorldX(20), SplashKit.ToWorldY(465));
                SplashKit.DrawText("Decription: " + _selectedItem.Description, Color.Black, Program.font, 30, SplashKit.ToWorldX(20), SplashKit.ToWorldY(480));
                SplashKit.DrawText("Value: " + _selectedItem.Value.ToString(), Color.Black, Program.font, 30, SplashKit.ToWorldX(190), SplashKit.ToWorldY(465));
                // If the item selected is a tool, display its efficiency
                if (_selectedItem.Name == "Iron Pickaxe" || _selectedItem.Name == "Gold Pickaxe" || _selectedItem.Name == "Diamond Pickaxe" || _selectedItem.Name == "Bomb"){
                    Tool tool = (Tool)_selectedItem;
                    SplashKit.DrawText("Efficiency: " + tool.Efficiency.ToString(), Color.Black, Program.font, 30, SplashKit.ToWorldX(190), SplashKit.ToWorldY(480));
                }
            }
        }

        /// <summary>
        /// This method is to draw the player
        /// </summary>
        public void DrawPlayer(){
            Bitmap toolImage = null;
            if (_toolSelected != null){
                if (_toolSelected.Name == "Iron Pickaxe"){
                    toolImage = Program.IronPickaxeImage;
                } else if (_toolSelected.Name == "Diamond Pickaxe"){
                    toolImage = Program.DiamondPickaxeImage;
                } else if (_toolSelected.Name == "Gold Pickaxe"){
                    toolImage = Program.GoldPickaxeImage;
                } else {
                    toolImage = Program.BombImage;
                }
            }
            
            if (_direction == Direction.Left){
                SplashKit.DrawBitmap(Program.Player_left, _location.X * Program.CELL_SIZE, _location.Y * Program.CELL_SIZE);
                if (_toolSelected != null)
                SplashKit.DrawBitmap(toolImage, _location.X * Program.CELL_SIZE, _location.Y * Program.CELL_SIZE + Program.CELL_SIZE / 2, SplashKit.OptionFlipY());
            } else {
                SplashKit.DrawBitmap(Program.Player_right, _location.X * Program.CELL_SIZE, _location.Y * Program.CELL_SIZE);
                if (_toolSelected != null)
                SplashKit.DrawBitmap(toolImage, _location.X * Program.CELL_SIZE + Program.CELL_SIZE / 2, _location.Y * Program.CELL_SIZE + Program.CELL_SIZE / 2);
            }
        }
        
    }
}