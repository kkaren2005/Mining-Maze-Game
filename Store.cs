using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Store class.
    /// </summary>
    public class Store{
        private List<Item> _items;

        /// <summary>
        /// This is default constructor. Store will have a default list of items that is available in the store.
        /// </summary>
        public Store(){
            _items = new List<Item>();

            _items.Add(Program.itemFactory.GetFood("Burger"));
            _items.Add(Program.itemFactory.GetFood("Pizza"));
            _items.Add(Program.itemFactory.GetFood("Cola"));
            _items.Add(Program.itemFactory.GetBomb());
            _items.Add(Program.itemFactory.GetPickaxe(MaterialType.Gold));
            _items.Add(Program.itemFactory.GetPickaxe(MaterialType.Diamond));
        }

        /// <summary>
        /// This is readonly property for Items field
        /// </summary>
        /// <value></value>
        public List<Item> Items{
            get { return _items; }
        }

        public void DrawStore(){
            // Draw the store
                SplashKit.FillRectangle(Color.White, SplashKit.ToWorldX(0), SplashKit.ToWorldY(180), 290, 360);
                SplashKit.DrawBitmap(Program.Flag, SplashKit.ToWorldX(30), SplashKit.ToWorldY(180));

                int shiftx = 0;
                int shifty = 0;
                int columnFilled = 0;

                // Draw each item the store sells
                foreach (Item item in _items){
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