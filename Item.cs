using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is a Item class, which consists of 4 attributes.
    /// </summary>
    public class Item{
        private string _name;

        private string _description;

        private int _value;

        private Bitmap _image;

        /// <summary>
        /// This is a pass by value constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="image"></param>
        public Item(string name, string description, int value, Bitmap image){
            _name = name;
            _description = description;
            _value = value;
            _image = image;
        }

        /// <summary>
        /// This is a readonly property for name field
        /// </summary>
        /// <value></value>
        public string Name{
            get { return _name; }
        }

        /// <summary>
        /// This is a readonly property for description field
        /// </summary>
        /// <value></value>
        public string Description{
            get { return _description; }
        }

        /// <summary>
        /// This is a readonly property for value field
        /// </summary>
        /// <value></value>
        public int Value{
            get { return _value; }
        }

        /// <summary>
        /// This is a property for image field
        /// </summary>
        /// <value></value>
        public Bitmap Image{
            get { return _image; }
        }
    }
}