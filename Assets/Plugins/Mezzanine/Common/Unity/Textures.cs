using UnityEngine;

namespace Mz.Unity
{
    public static class Textures
    {
        public static Texture2D Load(string filePath)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D texture;
            byte[] fileData;

            if (!System.IO.File.Exists(filePath)) return null; // Return null if load failed
            fileData = System.IO.File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2); // Create new "empty" texture
            return texture.LoadImage(fileData) // (size is set automatically)
                ? texture
                : Texture2D.blackTexture;
        }

        public static Texture2D Create(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);
            
            // set the pixel values
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            // Apply all SetPixel calls
            texture.Apply();
            
            return texture;
        }
    }
}