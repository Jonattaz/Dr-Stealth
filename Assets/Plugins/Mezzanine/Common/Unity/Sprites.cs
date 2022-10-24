using UnityEngine;

namespace Mz.Unity
{
    public static class Sprites
    {
        public static Sprite Load(
            string filePath, 
            float pixelsPerUnit = 128f,
            SpriteMeshType spriteType = SpriteMeshType.Tight
        )
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            var spriteTexture = Textures.Load(filePath);
            
            var newSprite = Sprite.Create(
                spriteTexture, 
                new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                new Vector2(0.5f, 0.5f), 
                pixelsPerUnit, 
                0, 
                spriteType
            );

            return newSprite;
        }
        
        public static Sprite Create(
            int width,
            int height,
            float pixelsPerUnit = 128f,
            SpriteMeshType spriteType = SpriteMeshType.Tight
        )
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            var spriteTexture = Textures.Create(width, height, Color.white);
            
            var newSprite = Sprite.Create(
                spriteTexture, 
                new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                new Vector2(0.5f, 0.5f), 
                pixelsPerUnit, 
                0, 
                spriteType
            );
            
            return newSprite;
        }
    }
}