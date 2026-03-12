using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Food class, which inherits from Item class
    /// </summary>
    public class Food:Item{
        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Food(string name, string description, int value, Bitmap image):base(name, description, value, image){

        }
    }
}