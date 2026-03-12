#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Inventory class
    /// </summary>
    public class Inventory{
        private List<Item> _items;

        private int _availableSlots;

        /// <summary>
        /// This is a constructor. Inventory will have a total of 25 slots.
        /// </summary>
        public Inventory(){
            _items = new List<Item>();
            _availableSlots = 25;
        }

        /// <summary>
        /// This is a readonly property for items count
        /// </summary>
        /// <value></value>
        public int ItemsCount{
            get { return _items.Count; }
        }

        /// <summary>
        /// This is a readonly property for Items field
        /// </summary>
        /// <value></value>
        public List<Item> Items{
            get { return _items; }
            set {_items = value;}
        }

        /// <summary>
        /// This method is to add item into the inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item){
            if (_availableSlots > 0){
                _items.Add(item);
                _availableSlots -= 1;
            }
        }

        /// <summary>
        /// This method is to remove item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item){
            if (_items.Contains(item)){
                _items.Remove(item);
                _availableSlots += 1;
            }
        }

        /// <summary>
        /// This method is to check whether the item is found in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool hasItem(Item item){
            if (_items.Contains(item)){
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is a method to clear all items in the inventory
        /// </summary>
        public void Clear(){
            _items.Clear();
        }

        /// <summary>
        /// This is a method that return the number of diamond collected in the inventory
        /// </summary>
        /// <returns></returns>
        public int DiamondCount(){
            int diamondCount = 0;
            foreach (Item item in _items)
            {
                if (item.Name == "Diamond"){
                    diamondCount += 1;
                }
            }
            return diamondCount;
        }

        /// <summary>
        /// This is a method to draw the inventory
        /// </summary>
        public void DrawInventory(){
            // Draw inventory box
            SplashKit.FillRectangle(Color.White, SplashKit.ToWorldX(0), SplashKit.ToWorldY(180), 290, 360);
            SplashKit.DrawBitmap(Program.Bag, SplashKit.ToWorldX(230), SplashKit.ToWorldY(180));

            int shiftx = 0;
            int shifty = 0;
            int columnFilled = 0;

            // Draw each item
            foreach(Item item in _items){
                SplashKit.DrawBitmap(item.Image, SplashKit.ToWorldX(22 + shiftx), SplashKit.ToWorldY(250 + shifty));
                SplashKit.DrawRectangle(Color.Black, SplashKit.ToWorldX(22 + shiftx), SplashKit.ToWorldY(250 + shifty), 40, 40);
                shiftx += 50;
                columnFilled += 1;
                if (columnFilled == 5){
                    shiftx = 0;
                    shifty += 50;
                    columnFilled = 0;
                }
            }
        }
    }
}