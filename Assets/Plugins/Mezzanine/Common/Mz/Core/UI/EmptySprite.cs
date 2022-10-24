using UnityEngine;

namespace Mz.App.UI
{
    public static class EmptySprite
    {
        static Sprite instance;

        ///<summary>
        /// Returns the instance of a (1 x 1) white Sprite
        /// </summary>	
        public static Sprite Get()
        {
            if (instance == null)
            {
                instance = Resources.Load<Sprite>("Core/EmptySprite");
            }
            
            return instance;
        }

        public static bool IsEmptySprite(Sprite s)
        {
            return Get() == s;
        }
    }

}