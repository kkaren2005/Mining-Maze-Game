using System;
using SplashKitSDK;

namespace testCustom
{
    /// <summary>
    /// This is Resource class, which inherits from Item class
    /// </summary>
    public class Resource:Item{
        private MaterialType _type;

        public Resource(string name, string description, int value, MaterialType type, Bitmap image):base(name, description, value, image){
            _type = type;
        }

        /// <summary>
        /// This is readonly property for Type field
        /// </summary>
        /// <value></value>
        public MaterialType Type{
            get { return _type; }
        }
    }
}