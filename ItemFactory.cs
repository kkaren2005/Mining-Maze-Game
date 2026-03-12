#nullable disable
using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is factory method that create items used in the game
    /// </summary>
    public class ItemFactory{

        /// <summary>
        /// This method is to create resource according to the material type
        /// </summary>
        /// <param name="materialType"></param>
        /// <returns></returns>
        public Resource GetResource(MaterialType materialType){
            if (materialType == MaterialType.Diamond){
                return new Resource("Diamond", "Shiny and hard", 100, MaterialType.Diamond, Program.DiamondImage);
            } else if (materialType == MaterialType.Gold){
                return new Resource("Gold", "Shiny and valuable", 80, MaterialType.Gold, Program.GoldImage);
            } else if (materialType == MaterialType.Iron){
                return new Resource("Iron", "Hard", 50, MaterialType.Iron, Program.IronImage);
            } else if (materialType == MaterialType.Coal){
                return new Resource("Coal", "Make a fire", 20, MaterialType.Coal, Program.CoalImage);
            }
            return null;
        }

        /// <summary>
        /// This is a default constructor
        /// </summary>
        public ItemFactory(){}

        /// <summary>
        /// This is a method to create bomb object
        /// </summary>
        /// <returns></returns>
        public Bomb GetBomb(){
            return new Bomb("Bomb", "BOOM", 50, 500, Program.BombImage);
        }

        public Pickaxe GetPickaxe(MaterialType materialType){
            if (materialType == MaterialType.Diamond){
                return new Pickaxe("Diamond Pickaxe", "Mine", 300, 50, Program.DiamondPickaxeImage);
            } else if (materialType == MaterialType.Gold){
                return new Pickaxe("Gold Pickaxe", "Mine", 200, 30, Program.GoldPickaxeImage);
            } else if (materialType == MaterialType.Iron){
                return new Pickaxe("Iron Pickaxe", "Mine", 10, 3, Program.IronPickaxeImage);
            }
            return null;
        }

        /// <summary>
        /// This method is to create food
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Food GetFood(string name){
            if (name == "Burger"){
                return new Food("Burger", "Yummy", 50, Program.BurgerImage);
            } else if (name == "Pizza"){
                return new Food("Pizza", "Tasty", 30, Program.PizzaImage);
            } else if (name == "Cola"){
                return new Food("Cola", "Tasty", 20, Program.ColaImage);
            }
            return null;
        }
    }
}